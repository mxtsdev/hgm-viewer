using HgmViewer.Service;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace HgmViewer.Classes
{
    public static class GltfMeshBuilder
    {
        public static void ToFile(string fileNamePath, SceneBuilderDefinition def)
        {
            var model = def.Scene.ToGltf2();
            
            // set armature name (can't be set in toolkit)
            foreach (var skin in model.LogicalSkins)
            {
                var m = skin.VisualParents.FirstOrDefault();
                if (m == null) continue;

                skin.Name = def.ArmatureName ?? (m.Mesh.Name + "_Armature");
            }
            model.SaveGLB(fileNamePath);
        }

        public static SceneBuilderDefinition CreateModel(string name, HgmModelInternal model)
        {
            var scene = new SceneBuilder(name);

            // process bones
            var bonesMapping = new Dictionary<int, NodeBuilder>();
            for (var i = 0; i < model.Joints.Count; i++)
            {
                bonesMapping[i] = CreateBoneHierarchy(model.Joints, i, bonesMapping);
            }

            foreach (var (mesh, meshIdx) in model.Meshes.Select((x, i) => (x, i)))
            {
                var meshName = $"{name}_{meshIdx + 1:000}";

                var fakeNormal = new Vector3(1f, 1f, 1f);
                var fakeTangent = new Vector4(1f, 1f, 1f, 1f);

                var material1 = new MaterialBuilder($"{meshName}_Mtl")
                    .WithMetallicRoughnessShader()
                    .WithMetallicRoughness(0.0f, 0.5f)
                    .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(0.8f, 0.8f, 0.8f, 1));

                if (mesh.Textures.TryGetValue(SltmTextureType.BaseMap, out var baseMapFilePath))
                {
                    var imgBuilder = (ImageBuilder)baseMapFilePath;
                    imgBuilder.Name = $"{meshName}_BaseMap";
                    material1.WithBaseColor(imgBuilder);
                }

                if (mesh.Textures.TryGetValue(SltmTextureType.RMMap, out var rmMapFilePath))
                {
                    var imgBuilder = (ImageBuilder)rmMapFilePath;
                    imgBuilder.Name = $"{meshName}_MetallicRoughness";
                    material1.WithMetallicRoughness(imgBuilder);
                }

                if (mesh.Textures.TryGetValue(SltmTextureType.NormalMap, out var normalMapFilePath))
                {
                    var imgBuilder = (ImageBuilder)normalMapFilePath;
                    imgBuilder.Name = $"{meshName}_NormalMap";
                    material1.WithNormal(imgBuilder);
                }

                if (mesh.Textures.TryGetValue(SltmTextureType.AOMap, out var aoMapFilePath))
                {
                    var imgBuilder = (ImageBuilder)aoMapFilePath;
                    imgBuilder.Name = $"{meshName}_AmbientOcclusion";
                    material1.WithOcclusion(imgBuilder);
                }

                if (mesh.Textures.TryGetValue(SltmTextureType.SIMap, out var siMapFilePath))
                {
                    var imgBuilder = (ImageBuilder)siMapFilePath;
                    imgBuilder.Name = $"{meshName}_SelfIllumination";
                    material1.WithEmissive(imgBuilder);
                }

                var meshBuilder = new MeshBuilder<VertexPositionNormalTangent, VertexTexture1, VertexJoints4>(meshName);
                var primitive = meshBuilder.UsePrimitive(material1);

                var verts = new IVertexBuilder[3];
                foreach (var f in mesh.Faces)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        var idx = f[i] - 1;
                        var v = mesh.Vertices[idx];

                        // fake only supposed to fill out vertexbuilder before conversion - but could potentially apply if a normal/tangent is missing for idx
                        // VertexPreprocessorLambdas.SanitizeVertexGeometry will fully strip a vertex with a zero-vector for either normal or tangent, corrupting the mesh
                        var nm = mesh.HasNormals ? mesh.Normals[idx] : fakeNormal;
                        var ta = mesh.HasTangents ? mesh.Tangents[idx] : fakeTangent;
                        var tc = mesh.HasUVs ? mesh.UVs[idx] : default;
                        var vertexJoints = mesh.HasVertexJoints ? mesh.VertexJoints[idx] : new[] { (0, 0f), (0, 0f), (0, 0f), (0, 0f) };

                        var vb = VertexBuilder<VertexPositionNormalTangent, VertexTexture1, VertexJoints4>.Create(v, nm, ta);
                        vb = vb.WithMaterial(tc);
                        vb = vb.WithSkinning(vertexJoints);

                        verts[i] = vb;
                    }

                    primitive.AddTriangle(verts[0], verts[1], verts[2]);
                }

                dynamic meshBuilder_orConverted;
                var meshConvert = true;

                if (mesh.HasTangents && mesh.HasUVs && mesh.HasVertexJoints)
                {
                    meshBuilder_orConverted = meshBuilder;
                    meshConvert = false;
                }
                else if (mesh.HasTangents && mesh.HasVertexJoints)
                {
                    meshBuilder_orConverted = new MeshBuilder<VertexPositionNormalTangent, VertexEmpty, VertexJoints4>(meshName);
                }
                else if (mesh.HasNormals && mesh.HasUVs && mesh.HasVertexJoints)
                {
                    meshBuilder_orConverted = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>(meshName);
                }
                else if (mesh.HasNormals && mesh.HasVertexJoints)
                {
                    meshBuilder_orConverted = new MeshBuilder<VertexPositionNormal, VertexEmpty, VertexJoints4>(meshName);
                }
                else if (mesh.HasUVs && mesh.HasVertexJoints)
                {
                    meshBuilder_orConverted = new MeshBuilder<VertexPosition, VertexTexture1, VertexJoints4>(meshName);
                }
                else if (mesh.HasTangents && mesh.HasUVs)
                {
                    meshBuilder_orConverted = new MeshBuilder<VertexPositionNormalTangent, VertexTexture1, VertexEmpty>(meshName);
                }
                else if (mesh.HasTangents)
                {
                    meshBuilder_orConverted = new MeshBuilder<VertexPositionNormalTangent, VertexEmpty, VertexEmpty>(meshName);
                }
                else if (mesh.HasNormals && mesh.HasUVs)
                {
                    meshBuilder_orConverted = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>(meshName);
                }
                else if (mesh.HasNormals)
                {
                    meshBuilder_orConverted = new MeshBuilder<VertexPositionNormal, VertexEmpty, VertexEmpty>(meshName);
                }
                else if (mesh.HasUVs)
                {
                    meshBuilder_orConverted = new MeshBuilder<VertexPosition, VertexTexture1, VertexEmpty>(meshName);
                }
                else
                {
                    meshBuilder_orConverted = new MeshBuilder<VertexPosition, VertexEmpty, VertexEmpty>(meshName);
                }

                if (meshConvert)
                    meshBuilder_orConverted.AddMesh(meshBuilder);

                var bbox = mesh.BBox != null ? mesh.BBox : model.BBox;
                var coordinateTransform = Matrix4x4.Multiply(Matrix4x4.CreateRotationY((float)-Math.PI / 2), Matrix4x4.CreateRotationZ((float)-Math.PI / 2));
                var worldMatrix = Matrix4x4.Identity;
                worldMatrix *= Matrix4x4.CreateTranslation(bbox.Center.X, bbox.Center.Y, bbox.Center.Z);
                worldMatrix *= coordinateTransform;

                if (mesh.HasVertexJoints)
                {
                    scene.AddSkinnedMesh(meshBuilder_orConverted, worldMatrix, bonesMapping.Values.ToArray());
                }
                else
                {
                    scene.AddRigidMesh(meshBuilder_orConverted, worldMatrix);
                }
            }

            return new SceneBuilderDefinition
            {
                Scene = scene,
                ArmatureName = model.ArmatureName
            };
        }

        // recursive helper class
        public static NodeBuilder CreateBoneHierarchy(List<HgmJointInternal> srcBones, int srcIndex, IReadOnlyDictionary<int, NodeBuilder> bonesMap)
        {
            var src = srcBones[srcIndex];
            var dstNode = new NodeBuilder(src.Name);

            var srcParentIdx = src.GroupIndex;

            if (srcParentIdx != 0xFFFF)
            {
                var dstParent = bonesMap[srcParentIdx];
                dstParent.AddNode(dstNode);

                Matrix4x4.Invert(src.Matrix, out var invMatrix);

                invMatrix.M44 = 1;

                var newMatrix = invMatrix;
                var coordinateTransform = Matrix4x4.Multiply(Matrix4x4.CreateRotationY((float)-Math.PI / 2), Matrix4x4.CreateRotationZ((float)-Math.PI / 2));
                newMatrix *= coordinateTransform;

                dstNode.WorldMatrix = newMatrix;
            }
            else
            {
                dstNode.WorldMatrix = Matrix4x4.Identity;
            }

            return dstNode;
        }
    }

    public class SceneBuilderDefinition
    {
        public SceneBuilder Scene { get; set; }
        public string ArmatureName { get; set; }
    }
}
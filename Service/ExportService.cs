using Classes;
using HgmViewer.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using static HgmViewer.Formats.Hgm;
using static HgmViewer.Formats.Hgm.Mesh;
using static HgmViewer.Service.DebugData;

namespace HgmViewer.Service
{
    /// <summary>
    /// Export Service: Converts Hgm to Glb
    /// </summary>
    public class ExportService
    {
        private readonly ConfigService _configService;

        public ExportService(ConfigService configService)
        {
            _configService = configService;
        }

        public ExportedModelDefinition ExportGlb(HgmDefinition def)
        {
            using var cultureInfoScope = new CultureInfoScope(CultureInfo.InvariantCulture);

            var filePath = Path.Combine(Path.GetFullPath(_configService.Config.ExportDirPath), $"{def.HgmFileName_WithoutExt}.glb");

            var debugData = ExportGlbInternal(def, filePath);

            var lookAt = Vector3.Transform(debugData.Model.BBox.Center, Matrix4x4.Multiply(Matrix4x4.CreateRotationY((float)-Math.PI / 2), Matrix4x4.CreateRotationZ((float)-Math.PI / 2)));
            var dist = (debugData.Model.BBox.Extent.X + debugData.Model.BBox.Extent.Y + debugData.Model.BBox.Extent.Z) / 3f;

            var exported = new ExportedModelDefinition
            {
                ModelFileName = Path.GetFileName(filePath),
                ModelFilePath = filePath,
                ModelDirPath = Path.GetDirectoryName(filePath),
                LookAt = lookAt,
                Dist = dist
            };

            return exported;
        }

        private static float ShortToFloat(short val)
        {
            return decimal.ToSingle(val / (decimal)short.MaxValue);
        }

        private static float UVShort2Float(short val)
        {
            return val / 4000f;
        }

        private static float Vec1Byte2Float(byte val)
        {
            return (val - 127) / 127f;
        }

        private DebugData ExportGlbInternal(HgmDefinition def, string filePath)
        {
            Debug.WriteLine($"[Export] Model: {filePath}");
            using var cultureInfoScope = new CultureInfoScope(CultureInfo.InvariantCulture);

            var model = new HgmModelInternal();
            var debugData = new DebugData { Model = model };

            // model bounding box
            var bboxValues = def.Hgm.Header.Bbox.Values;

            // expect [0, 1, 2] = min, [3, 4, 5] = max
            var lower = new Vector3(
                Math.Min(bboxValues[0], bboxValues[3]),
                Math.Min(bboxValues[1], bboxValues[4]),
                Math.Min(bboxValues[2], bboxValues[5])
                );
            var upper = new Vector3(
                Math.Max(bboxValues[0], bboxValues[3]),
                Math.Max(bboxValues[1], bboxValues[4]),
                Math.Max(bboxValues[2], bboxValues[5])
            );

            model.BBox = new BoundingBoxInternal(lower, upper);

            // process meshes
            foreach (var (meshInIdx, meshIn) in def.Hgm.Meshes.Select((x, j) => (j, x)))
            {
                var objectName = $"object_{meshInIdx + 1:000}";
                var meshOut = new HgmMeshInternal
                {
                    // mesh bsphere (?) (only in version >= 12)
                    BSphere = meshIn.Bsphere?.Count >= 4 ? new Vector4(meshIn.Bsphere[0], meshIn.Bsphere[1], meshIn.Bsphere[2], meshIn.Bsphere[3]) : default
                };

                if (meshIn.Bbox != null)
                {
                    // mesh bounding box (only in version >= 12)
                    var bboxValues_i = meshIn.Bbox?.Values;

                    var lower_i = new Vector3(
                        Math.Min(bboxValues_i[0], bboxValues_i[3]),
                        Math.Min(bboxValues_i[1], bboxValues_i[4]),
                        Math.Min(bboxValues_i[2], bboxValues_i[5])
                    );
                    var upper_i = new Vector3(
                        Math.Max(bboxValues_i[0], bboxValues_i[3]),
                        Math.Max(bboxValues_i[1], bboxValues_i[4]),
                        Math.Max(bboxValues_i[2], bboxValues_i[5])
                    );

                    meshOut.BBox = new BoundingBoxInternal(lower, upper);
                }

                // scale
                var scale = meshOut.BBox != null ? meshOut.BBox.Scale : model.BBox.Scale;

                model.Meshes.Add(meshOut);

                // get fields
                var pos = meshIn.GetFieldByName("pos"); // vertex
                var tc1 = meshIn.GetFieldByName("tc1"); // texture uv
                var normal = meshIn.GetFieldByName("normal");
                var binormal = meshIn.GetFieldByName("binormal");
                var tangent = meshIn.GetFieldByName("tangent");
                var normal_sign = meshIn.GetFieldByName("normal_sign");
                var biField = meshIn.GetFieldByName("bi"); // bone indices
                var bwField = meshIn.GetFieldByName("bw"); // bone weight

                var vertices = meshIn.Vertices.Select((v, idx) =>
                    {
                        var p = v.Fields[pos.Index] as VertexFieldSint16;

                        return new HgmVertexDebug { X = p.Values[0], Y = p.Values[1], Z = p.Values[2] };
                    }).ToArray();

                // mesh contained data
                var haveTextureIndex = tc1 != null;
                var haveNormalIndex = (binormal != null && tangent != null && normal_sign != null) || (normal != null);
                var haveBones = biField != null && bwField != null;

                // build vertices data
                foreach (var i in meshIn.Vertices)
                {
                    // get field data for current vertex
                    var v = i.Fields[pos.Index] as VertexFieldSint16;
                    var tc = tc1 != null ? i.Fields[tc1.Index] as VertexFieldSint16 : null;
                    var bn = binormal != null ? i.Fields[binormal.Index] as VertexFieldUnorm8 : null;
                    var ta = tangent != null ? i.Fields[tangent.Index] as VertexFieldUnorm8 : null;
                    var ns = normal_sign != null ? i.Fields[normal_sign.Index] as VertexFieldUnorm8 : null;
                    var nm = normal != null ? i.Fields[normal.Index] as VertexFieldUnorm8 : null;
                    var bi = biField != null ? i.Fields[biField.Index] as VertexFieldUint8 : null;
                    var bw = bwField != null ? i.Fields[bwField.Index] as VertexFieldUnorm8 : null;

                    // throw on unsupported data type
                    if (v.NumValues != 3) throw new ApplicationException("Expected vertex(3)");
                    if (tc != null && tc.NumValues != 2) throw new ApplicationException("Expected texture coordinate(2)");
                    if (bn != null && bn.NumValues != 3) throw new ApplicationException("Expected binormal(3)");
                    if (ta != null && ta.NumValues != 3) throw new ApplicationException("Expected tangent(3)");
                    if (ns != null && ns.NumValues != 1) throw new ApplicationException("Expected normal_sign(1)");
                    if (nm != null && nm.NumValues != 3) throw new ApplicationException("Expected normal(3)");
                    if (bi != null && bi.NumValues != 4) throw new ApplicationException("Expected bi(4)");
                    if (bw != null && bw.NumValues != 4) throw new ApplicationException("Expected bw(4)");

                    // add vertex
                    meshOut.Vertices.Add(new Vector3(
                        ShortToFloat(v.Values[0]) * scale, 
                        ShortToFloat(v.Values[1]) * -scale, // flip along Y-axis
                        ShortToFloat(v.Values[2]) * scale));

                    // texture uv
                    if (tc != null)
                    {
                        var uvU = UVShort2Float(tc.Values[0]);
                        var uvV = UVShort2Float(tc.Values[1]);

                        meshOut.UVs.Add(new Vector2(uvU, uvV));
                    }

                    // mesh contains binormal, tangent and normal sign - calculate normal
                    if (bn != null && ta != null && ns != null)
                    {
                        var binormalX = Vec1Byte2Float(bn.Values[0]);
                        var binormalY = Vec1Byte2Float(bn.Values[1]);
                        var binormalZ = Vec1Byte2Float(bn.Values[2]);

                        var tangentX = Vec1Byte2Float(ta.Values[0]);
                        var tangentY = Vec1Byte2Float(ta.Values[1]);
                        var tangentZ = Vec1Byte2Float(ta.Values[2]);

                        var tangentVector = new Vector3(tangentY, tangentZ, tangentX);
                        var binormalVector = new Vector3(binormalY, binormalZ, binormalX);

                        var normalX = binormalVector.Y * tangentVector.Z - binormalVector.Z * tangentVector.Y;
                        var normalY = binormalVector.Z * tangentVector.X - binormalVector.X * tangentVector.Z;
                        var normalZ = binormalVector.X * tangentVector.Y - binormalVector.Y * tangentVector.X;
                        var normalSign = ns.Values[0];
                        if (normalSign == 255)
                        {
                            normalX = tangentVector.Y * binormalVector.Z - tangentVector.Z * binormalVector.Y;
                            normalY = tangentVector.Z * binormalVector.X - tangentVector.X * binormalVector.Z;
                            normalZ = tangentVector.X * binormalVector.Y - tangentVector.Y * binormalVector.X;
                        }

                        var normalVector = Vector3.Normalize(new Vector3(normalX, -normalY, normalZ)); // flip along Y-axis

                        meshOut.Normals.Add(normalVector);
                        meshOut.Tangents.Add(new Vector4(new Vector3(tangentVector.X, -tangentVector.Y, tangentVector.Z), 1f)); // flip along Y-axis
                    }
                    // mesh contains normal
                    else if (nm != null)
                    {
                        var normalX = Vec1Byte2Float(nm.Values[0]);
                        var normalY = Vec1Byte2Float(nm.Values[1]);
                        var normalZ = Vec1Byte2Float(nm.Values[2]);

                        var normalVector = new Vector3(normalY, -normalZ, normalX); // flip along Y-axis

                        meshOut.Normals.Add(normalVector);
                    }

                    // process vertex joint data
                    if (haveBones)
                    {
                        meshOut.VertexJoints.Add(Enumerable.Range(0, 4).Select(n => ((int)bi.Values[n], bw.Values[n] / 255f)).ToArray());
                    }
                }

                // process faces
                foreach (var face in meshIn.Faces.Face)
                {
                    // faces must be flipped: ABC -> ACB (to match Y-axis flip)
                    meshOut.Faces.Add(new[] { face.F1 + 1, face.F3 + 1, face.F2 + 1 });
                }

                // textures
                if (def.Materials.TryGetValue((int)meshIn.MaterialIndex, out var material))
                {
                    meshOut.Textures = material.TextureFilePaths.ToDictionary(x => x.Key, x => x.Value.FilePath);
                }
            }

            // process joints
            if (def.Hgm.Armature?.Bones?.Count > 0)
            {
                model.ArmatureName = def.Hgm.Armature.Name.Str;

                foreach (var (bone, boneIdx) in def.Hgm.Armature.Bones.Select((x, i) => (x, i)))
                {
                    var joint = new HgmJointInternal
                    {
                        GroupIndex = bone.GroupIndex,
                        Name = bone.Name.Str
                    };
                    var mf = def.Hgm.Armature.Joints[boneIdx].F;

                    if (mf[12] == 0 && mf[13] == 0 && mf[14] == 0 && mf[15] == 1)
                    {
                        // SharpGLTF uses [M14, M24, M34, M44] for the affine transformation augmentation vector (last column)
                        // the hgm matrix is transposed to opposite order for row/columns

                        joint.Matrix = new Matrix4x4(
                            mf[0], mf[4], mf[8], mf[12], // rotation
                            mf[1], mf[5], mf[9], mf[13], // rotation
                            mf[2], mf[6], mf[10], mf[14], // rotation
                            mf[3], mf[7], mf[11], mf[15] // translation
                            );
                    }
                    else
                    {
                        // weirdly in some cases the hgm matrix has the same order used by SharpGLTF
                        // this is a temp fix for for those meshes (ex. Animal_Hen_mesh.glb)

                        joint.Matrix = new Matrix4x4(
                            mf[0], mf[1], mf[2], mf[3],
                            mf[4], mf[5], mf[6], mf[7],
                            mf[8], mf[9], mf[10], mf[11],
                            mf[12], mf[13], mf[14], mf[15]
                            );
                    }

                    model.Joints.Add(joint);
                }
            }

            var scene = GltfMeshBuilder.CreateModel(def.HgmFileName_WithoutExt, model);
            GltfMeshBuilder.ToFile(filePath, scene);

            return debugData;
        }
    }

    /// <summary>
    /// Data object for internal representation of parsed Hgm as provided to Glb exporter
    /// </summary>
    public class HgmModelInternal
    {
        public BoundingBoxInternal BBox { get; set; }
        public List<HgmMeshInternal> Meshes { get; set; } = new List<HgmMeshInternal>();
        public List<HgmJointInternal> Joints { get; set; } = new List<HgmJointInternal>();
        public bool HasJoints => Joints?.Count > 0;

        public string ArmatureName { get; set; }
    }

    /// <summary>
    /// Internal mesh representation (<see cref="HgmModelInternal"/>)
    /// </summary>
    public class HgmMeshInternal
    {
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public List<Vector2> UVs { get; set; } = new List<Vector2>();
        public List<Vector3> Normals { get; set; } = new List<Vector3>();
        public List<Vector4> Tangents { get; set; } = new List<Vector4>();
        public List<int[]> Faces { get; set; } = new List<int[]>();
        public List<(int, float)[]> VertexJoints { get; set; } = new List<(int, float)[]>();
        public Dictionary<SltmTextureType, string> Textures { get; set; } = new Dictionary<SltmTextureType, string>();

        public bool HasUVs => UVs?.Count > 0;
        public bool HasNormals => Normals?.Count > 0;
        public bool HasTangents => Tangents?.Count > 0;
        public bool HasVertexJoints => VertexJoints?.Count > 0;

        public BoundingBoxInternal BBox { get; set; }
        public Vector4 BSphere { get; set; }
    }

    /// <summary>
    /// Internal joint representation (<see cref="HgmModelInternal"/>)
    /// </summary>
    public class HgmJointInternal
    {
        public string Name { get; set; }
        public int GroupIndex { get; set; }
        public Matrix4x4 Matrix { get; set; }
    }

    /// <summary>
    /// Internal bounding box representation (<see cref="HgmModelInternal"/>)
    /// </summary>
    public class BoundingBoxInternal
    {
        public BoundingBoxInternal(Vector3 lower, Vector3 upper)
        {
            Lower = lower;
            Upper = upper;
            Extent = new Vector3
            {
                X = upper.X - lower.X,
                Y = upper.Y - lower.Y,
                Z = upper.Z - lower.Z
            };

            Center = new Vector3
            {
                X = upper.X - Extent.X / 2f,
                Y = upper.Y - Extent.Y / 2f,
                Z = upper.Z - Extent.Z / 2f
            };

            // scale is constrained to max(1f, x)
            // a mesh that do not extend beyond [-1, 1] (normalized vertex positions) is already to scale
            Scale = float.Max(1f, new[] { Extent.X, Extent.Y, Extent.Z }.Max() / 2f);
        }

        public Vector3 Lower { get; set; }
        public Vector3 Upper { get; set; }
        public Vector3 Extent { get; set; }
        public Vector3 Center { get; set; }
        public float Scale { get; set; }
    }

    /// <summary>
    /// Data object for exported Glb models
    /// </summary>
    public class ExportedModelDefinition
    {
        public string ModelFileName { get; set; }
        public string ModelFilePath { get; set; }
        public string ModelDirPath { get; set; }
        public Vector3? LookAt { get; set; }
        public float? Dist { get; set; }
    }

    /// <summary>
    /// Temporary storage for debug data from model export
    /// </summary>
    public class DebugData
    {
        /// <summary>
        /// Log lines to be output after export is finished
        /// </summary>
        public List<string> Logs { get; set; } = new List<string>();

        public HgmModelInternal Model { get; set; }

        public class HgmVertexDebug
        {
            public short X { get; set; }
            public short Y { get; set; }
            public short Z { get; set; }

            public override string ToString() => $"{X}, {Y}, {Z}";
        }
    }
}
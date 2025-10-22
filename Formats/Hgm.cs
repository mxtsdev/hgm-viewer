// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using Kaitai;
using System.Collections.Generic;

namespace HgmViewer.Formats
{
    public partial class Hgm : KaitaiStruct
    {
        public static Hgm FromFile(string fileName)
        {
            return new Hgm(new KaitaiStream(fileName));
        }


        public enum VertexTypeMarker
        {
            NotSet = 0,
            Set = 2,
        }

        public enum RecordType
        {
            Array = 0,
            CondensedElement = 1,
            Element = 2,
        }

        public enum RecordElementType
        {
            Field = 0,
            Padding = 2,
        }
        public Hgm(KaitaiStream p__io, KaitaiStruct p__parent = null, Hgm p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
        }
        public void _read()
        {
            __raw_header = m_io.ReadBytes(32);
            var io___raw_header = new KaitaiStream(__raw_header);
            _header = new HgmHeader(io___raw_header, this, m_root);
            _header._read();
            _numMeshes = m_io.ReadU4le();
            _meshes = new List<Mesh>();
            for (var i = 0; i < NumMeshes; i++)
            {
                Mesh _t_meshes = new Mesh(m_io, this, m_root);
                _meshes.Add(_t_meshes);
                _t_meshes._read();
            }
            _armatureElementType = m_io.ReadU1();
            if (ArmatureElementType == 1) {
                _armature = new HgmArmature(m_io, this, m_root);
                _armature._read();
            }
        }
        public partial class RecordTypeLookahead : KaitaiStruct
        {
            public static RecordTypeLookahead FromFile(string fileName)
            {
                return new RecordTypeLookahead(new KaitaiStream(fileName));
            }

            public RecordTypeLookahead(KaitaiStream p__io, KaitaiStruct p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_ofs = false;
                f_type1 = false;
            }
            public void _read()
            {
                if (Ofs < 0) {
                    _saveOfs = m_io.ReadBytes(0);
                }
            }
            private bool f_ofs;
            private int _ofs;
            public int Ofs
            {
                get
                {
                    if (f_ofs)
                        return _ofs;
                    _ofs = (int) (M_Io.Pos);
                    f_ofs = true;
                    return _ofs;
                }
            }
            private bool f_type1;
            private RecordType _type1;
            public RecordType Type1
            {
                get
                {
                    if (f_type1)
                        return _type1;
                    long _pos = m_io.Pos;
                    m_io.Seek(Ofs);
                    _type1 = ((Hgm.RecordType) m_io.ReadU1());
                    m_io.Seek(_pos);
                    f_type1 = true;
                    return _type1;
                }
            }
            private byte[] _saveOfs;
            private Hgm m_root;
            private KaitaiStruct m_parent;
            public byte[] SaveOfs { get { return _saveOfs; } }
            public Hgm M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class LenStr : KaitaiStruct
        {
            public static LenStr FromFile(string fileName)
            {
                return new LenStr(new KaitaiStream(fileName));
            }

            public LenStr(KaitaiStream p__io, KaitaiStruct p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
            }
            public void _read()
            {
                _sizeStr = m_io.ReadU4le();
                _str = System.Text.Encoding.GetEncoding("ascii").GetString(m_io.ReadBytes(SizeStr));
            }
            private uint _sizeStr;
            private string _str;
            private Hgm m_root;
            private KaitaiStruct m_parent;
            public uint SizeStr { get { return _sizeStr; } }
            public string Str { get { return _str; } }
            public Hgm M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class Bip : KaitaiStruct
        {
            public static Bip FromFile(string fileName)
            {
                return new Bip(new KaitaiStream(fileName));
            }

            public Bip(KaitaiStream p__io, Hgm.HgmArmature p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
            }
            public void _read()
            {
                _name = new LenStr(m_io, this, m_root);
                _name._read();
                _groupIndex = m_io.ReadU2le();
            }
            private LenStr _name;
            private ushort _groupIndex;
            private Hgm m_root;
            private Hgm.HgmArmature m_parent;
            public LenStr Name { get { return _name; } }
            public ushort GroupIndex { get { return _groupIndex; } }
            public Hgm M_Root { get { return m_root; } }
            public Hgm.HgmArmature M_Parent { get { return m_parent; } }
        }
        public partial class Bbox : KaitaiStruct
        {
            public static Bbox FromFile(string fileName)
            {
                return new Bbox(new KaitaiStream(fileName));
            }

            public Bbox(KaitaiStream p__io, KaitaiStruct p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
            }
            public void _read()
            {
                _values = new List<float>();
                for (var i = 0; i < 6; i++)
                {
                    _values.Add(m_io.ReadF4le());
                }
            }
            private List<float> _values;
            private Hgm m_root;
            private KaitaiStruct m_parent;
            public List<float> Values { get { return _values; } }
            public Hgm M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class RecordElement : KaitaiStruct
        {
            public static RecordElement FromFile(string fileName)
            {
                return new RecordElement(new KaitaiStream(fileName));
            }

            public RecordElement(KaitaiStream p__io, Hgm.Record p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
            }
            public void _read()
            {
                _key = new LenStr(m_io, this, m_root);
                _key._read();
                _valueId = m_io.ReadU4le();
                _value = new LenStr(m_io, this, m_root);
                _value._read();
                __unnamed3 = m_io.ReadBytes(4);
                _offset = m_io.ReadU4le();
                _type = ((Hgm.RecordElementType) m_io.ReadU1());
            }
            private LenStr _key;
            private uint _valueId;
            private LenStr _value;
            private byte[] __unnamed3;
            private uint _offset;
            private RecordElementType _type;
            private Hgm m_root;
            private Hgm.Record m_parent;
            public LenStr Key { get { return _key; } }
            public uint ValueId { get { return _valueId; } }
            public LenStr Value { get { return _value; } }
            public byte[] Unnamed_3 { get { return __unnamed3; } }
            public uint Offset { get { return _offset; } }
            public RecordElementType Type { get { return _type; } }
            public Hgm M_Root { get { return m_root; } }
            public Hgm.Record M_Parent { get { return m_parent; } }
        }
        public partial class RecordArray : KaitaiStruct
        {
            public static RecordArray FromFile(string fileName)
            {
                return new RecordArray(new KaitaiStream(fileName));
            }

            public RecordArray(KaitaiStream p__io, Hgm.Mesh p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
            }
            public void _read()
            {
                _numElements = m_io.ReadU4le();
                _elements = new List<Record>();
                for (var i = 0; i < NumElements; i++)
                {
                    Record _t_elements = new Record(m_io, this, m_root);
                    _elements.Add(_t_elements);
                    _t_elements._read();
                }
            }
            private uint _numElements;
            private List<Record> _elements;
            private Hgm m_root;
            private Hgm.Mesh m_parent;
            public uint NumElements { get { return _numElements; } }
            public List<Record> Elements { get { return _elements; } }
            public Hgm M_Root { get { return m_root; } }
            public Hgm.Mesh M_Parent { get { return m_parent; } }
        }
        public partial class Temp1 : KaitaiStruct
        {
            public static Temp1 FromFile(string fileName)
            {
                return new Temp1(new KaitaiStream(fileName));
            }

            public Temp1(KaitaiStream p__io, Hgm.HgmArmature p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
            }
            public void _read()
            {
                _f = new List<float>();
                for (var i = 0; i < 16; i++)
                {
                    _f.Add(m_io.ReadF4le());
                }
            }
            private List<float> _f;
            private Hgm m_root;
            private Hgm.HgmArmature m_parent;
            public List<float> F { get { return _f; } }
            public Hgm M_Root { get { return m_root; } }
            public Hgm.HgmArmature M_Parent { get { return m_parent; } }
        }
        public partial class Mesh : KaitaiStruct
        {
            public static Mesh FromFile(string fileName)
            {
                return new Mesh(new KaitaiStream(fileName));
            }

            public Mesh(KaitaiStream p__io, Hgm p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_fields = false;
                f_numFields = false;
            }
            public void _read()
            {
                _materialIndex = m_io.ReadU4le();
                __unnamed1 = m_io.ReadBytes(8);
                _numVertices = m_io.ReadU4le();
                _numFaces = m_io.ReadU4le();
                _haveVertexType = ((Hgm.VertexTypeMarker) m_io.ReadU1());
                if (HaveVertexType == Hgm.VertexTypeMarker.Set) {
                    _vertexType = new LenStr(m_io, this, m_root);
                    _vertexType._read();
                }
                _fieldsArr = new RecordArray(m_io, this, m_root);
                _fieldsArr._read();
                __unnamed7 = m_io.ReadBytes(4);
                _sizeVertex = m_io.ReadU4le();
                __unnamed9 = m_io.ReadBytes(1);
                __raw_vertices = new List<byte[]>();
                _vertices = new List<Vertex>();
                for (var i = 0; i < NumVertices; i++)
                {
                    __raw_vertices.Add(m_io.ReadBytes(SizeVertex));
                    var io___raw_vertices = new KaitaiStream(__raw_vertices[__raw_vertices.Count - 1]);
                    Vertex _t_vertices = new Vertex(io___raw_vertices, this, m_root);
                    _vertices.Add(_t_vertices);
                    _t_vertices._read();
                }
                _faces = new FacesStruct(m_io, this, m_root);
                _faces._read();
                if (M_Root.Header.Version >= 12) {
                    _bbox = new Bbox(m_io, this, m_root);
                    _bbox._read();
                }
                if (M_Root.Header.Version >= 12) {
                    _bsphere = new List<float>();
                    for (var i = 0; i < 4; i++)
                    {
                        _bsphere.Add(m_io.ReadF4le());
                    }
                }
            }
            public partial class VertexFieldUnorm8 : KaitaiStruct
            {
                public VertexFieldUnorm8(int p_numValues, KaitaiStream p__io, Hgm.Mesh.Vertex p__parent = null, Hgm p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _numValues = p_numValues;
                }
                public void _read()
                {
                    _values = new List<byte>();
                    for (var i = 0; i < NumValues; i++)
                    {
                        _values.Add(m_io.ReadU1());
                    }
                }
                private List<byte> _values;
                private int _numValues;
                private Hgm m_root;
                private Hgm.Mesh.Vertex m_parent;
                public List<byte> Values { get { return _values; } }
                public int NumValues { get { return _numValues; } }
                public Hgm M_Root { get { return m_root; } }
                public Hgm.Mesh.Vertex M_Parent { get { return m_parent; } }
            }
            public partial class Vertex : KaitaiStruct
            {
                public static Vertex FromFile(string fileName)
                {
                    return new Vertex(new KaitaiStream(fileName));
                }

                public Vertex(KaitaiStream p__io, Hgm.Mesh p__parent = null, Hgm p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                }
                public void _read()
                {
                    _fields = new List<KaitaiStruct>();
                    for (var i = 0; i < M_Parent.NumFields; i++)
                    {
                        switch (M_Parent.Fields[i].ElementValueStr) {
                        case "fmt_sint16_c3": {
                            VertexFieldSint16 _t_fields = new VertexFieldSint16(3, m_io, this, m_root);
                            _fields.Add(_t_fields);
                            ((VertexFieldSint16) (_t_fields))._read();
                            break;
                        }
                        case "fmt_unorm8_c3": {
                            VertexFieldUnorm8 _t_fields = new VertexFieldUnorm8(3, m_io, this, m_root);
                            _fields.Add(_t_fields);
                            ((VertexFieldUnorm8) (_t_fields))._read();
                            break;
                        }
                        case "fmt_unorm8_c4": {
                            VertexFieldUnorm8 _t_fields = new VertexFieldUnorm8(4, m_io, this, m_root);
                            _fields.Add(_t_fields);
                            ((VertexFieldUnorm8) (_t_fields))._read();
                            break;
                        }
                        case "fmt_unorm8_c2": {
                            VertexFieldUnorm8 _t_fields = new VertexFieldUnorm8(2, m_io, this, m_root);
                            _fields.Add(_t_fields);
                            ((VertexFieldUnorm8) (_t_fields))._read();
                            break;
                        }
                        case "fmt_uint8_c4": {
                            VertexFieldUint8 _t_fields = new VertexFieldUint8(4, m_io, this, m_root);
                            _fields.Add(_t_fields);
                            ((VertexFieldUint8) (_t_fields))._read();
                            break;
                        }
                        case "fmt_sint16_c2": {
                            VertexFieldSint16 _t_fields = new VertexFieldSint16(2, m_io, this, m_root);
                            _fields.Add(_t_fields);
                            ((VertexFieldSint16) (_t_fields))._read();
                            break;
                        }
                        case "fmt_sint16_c1": {
                            VertexFieldSint16 _t_fields = new VertexFieldSint16(1, m_io, this, m_root);
                            _fields.Add(_t_fields);
                            ((VertexFieldSint16) (_t_fields))._read();
                            break;
                        }
                        case "fmt_unorm8_c1": {
                            VertexFieldUnorm8 _t_fields = new VertexFieldUnorm8(1, m_io, this, m_root);
                            _fields.Add(_t_fields);
                            ((VertexFieldUnorm8) (_t_fields))._read();
                            break;
                        }
                        default: {
                            VertexFieldUnknown _t_fields = new VertexFieldUnknown(i, m_io, this, m_root);
                            _fields.Add(_t_fields);
                            ((VertexFieldUnknown) (_t_fields))._read();
                            break;
                        }
                        }
                    }
                }
                private List<KaitaiStruct> _fields;
                private Hgm m_root;
                private Hgm.Mesh m_parent;
                public List<KaitaiStruct> Fields { get { return _fields; } }
                public Hgm M_Root { get { return m_root; } }
                public Hgm.Mesh M_Parent { get { return m_parent; } }
            }
            public partial class FaceStruct : KaitaiStruct
            {
                public static FaceStruct FromFile(string fileName)
                {
                    return new FaceStruct(new KaitaiStream(fileName));
                }

                public FaceStruct(KaitaiStream p__io, Hgm.Mesh.FacesStruct p__parent = null, Hgm p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                }
                public void _read()
                {
                    _f1 = m_io.ReadU2le();
                    _f2 = m_io.ReadU2le();
                    _f3 = m_io.ReadU2le();
                }
                private ushort _f1;
                private ushort _f2;
                private ushort _f3;
                private Hgm m_root;
                private Hgm.Mesh.FacesStruct m_parent;
                public ushort F1 { get { return _f1; } }
                public ushort F2 { get { return _f2; } }
                public ushort F3 { get { return _f3; } }
                public Hgm M_Root { get { return m_root; } }
                public Hgm.Mesh.FacesStruct M_Parent { get { return m_parent; } }
            }
            public partial class VertexFieldUnknown : KaitaiStruct
            {
                public VertexFieldUnknown(int p_fieldIndex, KaitaiStream p__io, Hgm.Mesh.Vertex p__parent = null, Hgm p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _fieldIndex = p_fieldIndex;
                    f_isLastField = false;
                    f_ofs = false;
                    f_ofsNext = false;
                    f_lenUnk1 = false;
                }
                public void _read()
                {
                    _unk1 = m_io.ReadBytes(LenUnk1);
                }
                private bool f_isLastField;
                private bool _isLastField;
                public bool IsLastField
                {
                    get
                    {
                        if (f_isLastField)
                            return _isLastField;
                        _isLastField = (bool) (FieldIndex == (M_Parent.M_Parent.Fields.Count - 1));
                        f_isLastField = true;
                        return _isLastField;
                    }
                }
                private bool f_ofs;
                private uint _ofs;
                public uint Ofs
                {
                    get
                    {
                        if (f_ofs)
                            return _ofs;
                        _ofs = (uint) (((Hgm.RecordElement) (M_Parent.M_Parent.Fields[FieldIndex].Data)).Offset);
                        f_ofs = true;
                        return _ofs;
                    }
                }
                private bool f_ofsNext;
                private uint? _ofsNext;
                public uint? OfsNext
                {
                    get
                    {
                        if (f_ofsNext)
                            return _ofsNext;
                        if (IsLastField == false) {
                            _ofsNext = (uint) (((Hgm.RecordElement) (M_Parent.M_Parent.Fields[(FieldIndex + 1)].Data)).Offset);
                        }
                        f_ofsNext = true;
                        return _ofsNext;
                    }
                }
                private bool f_lenUnk1;
                private int _lenUnk1;
                public int LenUnk1
                {
                    get
                    {
                        if (f_lenUnk1)
                            return _lenUnk1;
                        _lenUnk1 = (int) ((IsLastField ? (((uint) (M_Parent.M_Io.Size)) - Ofs) : (OfsNext - Ofs)));
                        f_lenUnk1 = true;
                        return _lenUnk1;
                    }
                }
                private byte[] _unk1;
                private int _fieldIndex;
                private Hgm m_root;
                private Hgm.Mesh.Vertex m_parent;
                public byte[] Unk1 { get { return _unk1; } }
                public int FieldIndex { get { return _fieldIndex; } }
                public Hgm M_Root { get { return m_root; } }
                public Hgm.Mesh.Vertex M_Parent { get { return m_parent; } }
            }
            public partial class VertexFieldSint16 : KaitaiStruct
            {
                public VertexFieldSint16(int p_numValues, KaitaiStream p__io, Hgm.Mesh.Vertex p__parent = null, Hgm p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _numValues = p_numValues;
                }
                public void _read()
                {
                    _values = new List<short>();
                    for (var i = 0; i < NumValues; i++)
                    {
                        _values.Add(m_io.ReadS2le());
                    }
                }
                private List<short> _values;
                private int _numValues;
                private Hgm m_root;
                private Hgm.Mesh.Vertex m_parent;
                public List<short> Values { get { return _values; } }
                public int NumValues { get { return _numValues; } }
                public Hgm M_Root { get { return m_root; } }
                public Hgm.Mesh.Vertex M_Parent { get { return m_parent; } }
            }
            public partial class VertexFieldUint8 : KaitaiStruct
            {
                public VertexFieldUint8(int p_numValues, KaitaiStream p__io, Hgm.Mesh.Vertex p__parent = null, Hgm p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _numValues = p_numValues;
                }
                public void _read()
                {
                    _values = new List<byte>();
                    for (var i = 0; i < NumValues; i++)
                    {
                        _values.Add(m_io.ReadU1());
                    }
                }
                private List<byte> _values;
                private int _numValues;
                private Hgm m_root;
                private Hgm.Mesh.Vertex m_parent;
                public List<byte> Values { get { return _values; } }
                public int NumValues { get { return _numValues; } }
                public Hgm M_Root { get { return m_root; } }
                public Hgm.Mesh.Vertex M_Parent { get { return m_parent; } }
            }
            public partial class FacesStruct : KaitaiStruct
            {
                public static FacesStruct FromFile(string fileName)
                {
                    return new FacesStruct(new KaitaiStream(fileName));
                }

                public FacesStruct(KaitaiStream p__io, Hgm.Mesh p__parent = null, Hgm p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                }
                public void _read()
                {
                    _unk1TypeMarker = ((Hgm.RecordType) m_io.ReadU1());
                    _face = new List<FaceStruct>();
                    for (var i = 0; i < (M_Parent.NumFaces / 3); i++)
                    {
                        FaceStruct _t_face = new FaceStruct(m_io, this, m_root);
                        _face.Add(_t_face);
                        _t_face._read();
                    }
                }
                private RecordType _unk1TypeMarker;
                private List<FaceStruct> _face;
                private Hgm m_root;
                private Hgm.Mesh m_parent;
                public RecordType Unk1TypeMarker { get { return _unk1TypeMarker; } }
                public List<FaceStruct> Face { get { return _face; } }
                public Hgm M_Root { get { return m_root; } }
                public Hgm.Mesh M_Parent { get { return m_parent; } }
            }
            private bool f_fields;
            private List<Record> _fields;
            public List<Record> Fields
            {
                get
                {
                    if (f_fields)
                        return _fields;
                    _fields = (List<Record>) (FieldsArr.Elements);
                    f_fields = true;
                    return _fields;
                }
            }
            private bool f_numFields;
            private uint _numFields;
            public uint NumFields
            {
                get
                {
                    if (f_numFields)
                        return _numFields;
                    _numFields = (uint) (FieldsArr.NumElements);
                    f_numFields = true;
                    return _numFields;
                }
            }
            private uint _materialIndex;
            private byte[] __unnamed1;
            private uint _numVertices;
            private uint _numFaces;
            private VertexTypeMarker _haveVertexType;
            private LenStr _vertexType;
            private RecordArray _fieldsArr;
            private byte[] __unnamed7;
            private uint _sizeVertex;
            private byte[] __unnamed9;
            private List<Vertex> _vertices;
            private FacesStruct _faces;
            private Bbox _bbox;
            private List<float> _bsphere;
            private Hgm m_root;
            private Hgm m_parent;
            private List<byte[]> __raw_vertices;
            public uint MaterialIndex { get { return _materialIndex; } }
            public byte[] Unnamed_1 { get { return __unnamed1; } }
            public uint NumVertices { get { return _numVertices; } }
            public uint NumFaces { get { return _numFaces; } }
            public VertexTypeMarker HaveVertexType { get { return _haveVertexType; } }
            public LenStr VertexType { get { return _vertexType; } }
            public RecordArray FieldsArr { get { return _fieldsArr; } }
            public byte[] Unnamed_7 { get { return __unnamed7; } }
            public uint SizeVertex { get { return _sizeVertex; } }
            public byte[] Unnamed_9 { get { return __unnamed9; } }
            public List<Vertex> Vertices { get { return _vertices; } }
            public FacesStruct Faces { get { return _faces; } }

            /// <summary>
            /// Exists only when Header.Version &gt;= 12
            /// </summary>
            public Bbox Bbox { get { return _bbox; } }

            /// <summary>
            /// Exists only when Header.Version &gt;= 12
            /// </summary>
            public List<float> Bsphere { get { return _bsphere; } }
            public Hgm M_Root { get { return m_root; } }
            public Hgm M_Parent { get { return m_parent; } }
            public List<byte[]> M_RawVertices { get { return __raw_vertices; } }
        }
        public partial class HgmHeader : KaitaiStruct
        {
            public static HgmHeader FromFile(string fileName)
            {
                return new HgmHeader(new KaitaiStream(fileName));
            }

            public HgmHeader(KaitaiStream p__io, Hgm p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
            }
            public void _read()
            {
                _magic = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Magic, new byte[] { 104, 115, 109, 104 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 104, 115, 109, 104 }, Magic, M_Io, "/types/hgm_header/seq/0");
                }
                _version = m_io.ReadU2le();
                __unnamed2 = m_io.ReadBytes(2);
                _bbox = new Bbox(m_io, this, m_root);
                _bbox._read();
            }
            private byte[] _magic;
            private ushort _version;
            private byte[] __unnamed2;
            private Bbox _bbox;
            private Hgm m_root;
            private Hgm m_parent;
            public byte[] Magic { get { return _magic; } }
            public ushort Version { get { return _version; } }
            public byte[] Unnamed_2 { get { return __unnamed2; } }
            public Bbox Bbox { get { return _bbox; } }
            public Hgm M_Root { get { return m_root; } }
            public Hgm M_Parent { get { return m_parent; } }
        }
        public partial class RecordCondensedElement : KaitaiStruct
        {
            public static RecordCondensedElement FromFile(string fileName)
            {
                return new RecordCondensedElement(new KaitaiStream(fileName));
            }

            public RecordCondensedElement(KaitaiStream p__io, Hgm.Record p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
            }
            public void _read()
            {
                _fieldIndex = m_io.ReadU4le();
                _valueId = m_io.ReadU4le();
                _value = new LenStr(m_io, this, m_root);
                _value._read();
                __unnamed3 = m_io.ReadBytes(4);
                _offset = m_io.ReadU4le();
                _type = ((Hgm.RecordElementType) m_io.ReadU1());
            }
            private uint _fieldIndex;
            private uint _valueId;
            private LenStr _value;
            private byte[] __unnamed3;
            private uint _offset;
            private RecordElementType _type;
            private Hgm m_root;
            private Hgm.Record m_parent;
            public uint FieldIndex { get { return _fieldIndex; } }
            public uint ValueId { get { return _valueId; } }
            public LenStr Value { get { return _value; } }
            public byte[] Unnamed_3 { get { return __unnamed3; } }
            public uint Offset { get { return _offset; } }
            public RecordElementType Type { get { return _type; } }
            public Hgm M_Root { get { return m_root; } }
            public Hgm.Record M_Parent { get { return m_parent; } }
        }
        public partial class HgmArmature : KaitaiStruct
        {
            public static HgmArmature FromFile(string fileName)
            {
                return new HgmArmature(new KaitaiStream(fileName));
            }

            public HgmArmature(KaitaiStream p__io, Hgm p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
            }
            public void _read()
            {
                _name = new LenStr(m_io, this, m_root);
                _name._read();
                _numBones = m_io.ReadU4le();
                __unnamed2 = m_io.ReadBytes(6);
                if (M_Root.Header.Version >= 12) {
                    __unnamed3 = m_io.ReadBytes(4);
                }
                _bones = new List<Bip>();
                for (var i = 0; i < NumBones; i++)
                {
                    Bip _t_bones = new Bip(m_io, this, m_root);
                    _bones.Add(_t_bones);
                    _t_bones._read();
                }
                __raw_joints = new List<byte[]>();
                _joints = new List<Temp1>();
                for (var i = 0; i < NumBones; i++)
                {
                    __raw_joints.Add(m_io.ReadBytes(64));
                    var io___raw_joints = new KaitaiStream(__raw_joints[__raw_joints.Count - 1]);
                    Temp1 _t_joints = new Temp1(io___raw_joints, this, m_root);
                    _joints.Add(_t_joints);
                    _t_joints._read();
                }
            }
            private LenStr _name;
            private uint _numBones;
            private byte[] __unnamed2;
            private byte[] __unnamed3;
            private List<Bip> _bones;
            private List<Temp1> _joints;
            private Hgm m_root;
            private Hgm m_parent;
            private List<byte[]> __raw_joints;
            public LenStr Name { get { return _name; } }
            public uint NumBones { get { return _numBones; } }
            public byte[] Unnamed_2 { get { return __unnamed2; } }
            public byte[] Unnamed_3 { get { return __unnamed3; } }
            public List<Bip> Bones { get { return _bones; } }
            public List<Temp1> Joints { get { return _joints; } }
            public Hgm M_Root { get { return m_root; } }
            public Hgm M_Parent { get { return m_parent; } }
            public List<byte[]> M_RawJoints { get { return __raw_joints; } }
        }
        public partial class Record : KaitaiStruct
        {
            public static Record FromFile(string fileName)
            {
                return new Record(new KaitaiStream(fileName));
            }

            public Record(KaitaiStream p__io, Hgm.RecordArray p__parent = null, Hgm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_elementValueStr = false;
            }
            public void _read()
            {
                _type1 = ((Hgm.RecordType) m_io.ReadU1());
                switch (Type1) {
                case Hgm.RecordType.Element: {
                    _data = new RecordElement(m_io, this, m_root);
                    ((RecordElement) (_data))._read();
                    break;
                }
                case Hgm.RecordType.CondensedElement: {
                    _data = new RecordCondensedElement(m_io, this, m_root);
                    ((RecordCondensedElement) (_data))._read();
                    break;
                }
                }
            }
            private bool f_elementValueStr;
            private string _elementValueStr;
            public string ElementValueStr
            {
                get
                {
                    if (f_elementValueStr)
                        return _elementValueStr;
                    _elementValueStr = (string) ((Type1 == Hgm.RecordType.Element ? ((Hgm.RecordElement) (Data)).Value.Str : (Type1 == Hgm.RecordType.CondensedElement ? ((Hgm.RecordCondensedElement) (Data)).Value.Str : "")));
                    f_elementValueStr = true;
                    return _elementValueStr;
                }
            }
            private RecordType _type1;
            private KaitaiStruct _data;
            private Hgm m_root;
            private Hgm.RecordArray m_parent;
            public RecordType Type1 { get { return _type1; } }
            public KaitaiStruct Data { get { return _data; } }
            public Hgm M_Root { get { return m_root; } }
            public Hgm.RecordArray M_Parent { get { return m_parent; } }
        }
        private HgmHeader _header;
        private uint _numMeshes;
        private List<Mesh> _meshes;
        private byte _armatureElementType;
        private HgmArmature _armature;
        private Hgm m_root;
        private KaitaiStruct m_parent;
        private byte[] __raw_header;
        public HgmHeader Header { get { return _header; } }
        public uint NumMeshes { get { return _numMeshes; } }
        public List<Mesh> Meshes { get { return _meshes; } }
        public byte ArmatureElementType { get { return _armatureElementType; } }
        public HgmArmature Armature { get { return _armature; } }
        public Hgm M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
        public byte[] M_RawHeader { get { return __raw_header; } }
    }
}

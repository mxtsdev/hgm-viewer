// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using Kaitai;

using System.Collections.Generic;

namespace HgmViewer.Formats
{
    public partial class Sltm : KaitaiStruct
    {
        public static Sltm FromFile(string fileName)
        {
            return new Sltm(new KaitaiStream(fileName));
        }

        public Sltm(KaitaiStream p__io, KaitaiStruct p__parent = null, Sltm p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _header = new HeaderStruct(m_io, this, m_root);
            _mtlName = new RecStruct(m_io, this, m_root);
            switch (Header.Version)
            {
                case 0:
                    {
                        _mtl = new Version0Struct(m_io, this, m_root);
                        break;
                    }
                case 1:
                    {
                        _mtl = new Version1Struct(m_io, this, m_root);
                        break;
                    }
            }
        }
        public partial class LenStr : KaitaiStruct
        {
            public static LenStr FromFile(string fileName)
            {
                return new LenStr(new KaitaiStream(fileName));
            }

            public LenStr(KaitaiStream p__io, KaitaiStruct p__parent = null, Sltm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _len = m_io.ReadU4le();
                _str = System.Text.Encoding.GetEncoding("ascii").GetString(m_io.ReadBytes(Len));
            }
            private uint _len;
            private string _str;
            private Sltm m_root;
            private KaitaiStruct m_parent;
            public uint Len { get { return _len; } }
            public string Str { get { return _str; } }
            public Sltm M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class Version0Struct : KaitaiStruct
        {
            public static Version0Struct FromFile(string fileName)
            {
                return new Version0Struct(new KaitaiStream(fileName));
            }

            public Version0Struct(KaitaiStream p__io, Sltm p__parent = null, Sltm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk1 = m_io.ReadBytes(17);
                _textures = new Textures0Struct(m_io, this, m_root);
                __raw_data = m_io.ReadBytes(96);
                var io___raw_data = new KaitaiStream(__raw_data);
                _data = new MaterialDataStruct(io___raw_data, this, m_root);
                if (!(M_Io.IsEof))
                {
                    _submtl = new List<Rec0Struct>();
                    {
                        var i = 0;
                        while (!m_io.IsEof)
                        {
                            _submtl.Add(new Rec0Struct(m_io, this, m_root));
                            i++;
                        }
                    }
                }
            }
            private byte[] _unk1;
            private Textures0Struct _textures;
            private MaterialDataStruct _data;
            private List<Rec0Struct> _submtl;
            private Sltm m_root;
            private Sltm m_parent;
            private byte[] __raw_data;
            public byte[] Unk1 { get { return _unk1; } }
            public Textures0Struct Textures { get { return _textures; } }
            public MaterialDataStruct Data { get { return _data; } }
            public List<Rec0Struct> Submtl { get { return _submtl; } }
            public Sltm M_Root { get { return m_root; } }
            public Sltm M_Parent { get { return m_parent; } }
            public byte[] M_RawData { get { return __raw_data; } }
        }
        public partial class MaterialDataStruct : KaitaiStruct
        {
            public static MaterialDataStruct FromFile(string fileName)
            {
                return new MaterialDataStruct(new KaitaiStream(fileName));
            }

            public MaterialDataStruct(KaitaiStream p__io, KaitaiStruct p__parent = null, Sltm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk0 = m_io.ReadBytes(4);
                _opacity = m_io.ReadU4le();
                _unk1 = m_io.ReadBytes(28);
                _colors = m_io.ReadU4le();
                _unk2 = m_io.ReadBytes(16);
                _uSize = m_io.ReadF4le();
                _vSize = m_io.ReadF4le();
                _unk3 = m_io.ReadBytes(8);
                _stretchFactor1 = m_io.ReadF4le();
                _stretchFactor2 = m_io.ReadF4le();
                _unk4 = m_io.ReadBytes(16);
            }
            private byte[] _unk0;
            private uint _opacity;
            private byte[] _unk1;
            private uint _colors;
            private byte[] _unk2;
            private float _uSize;
            private float _vSize;
            private byte[] _unk3;
            private float _stretchFactor1;
            private float _stretchFactor2;
            private byte[] _unk4;
            private Sltm m_root;
            private KaitaiStruct m_parent;
            public byte[] Unk0 { get { return _unk0; } }
            public uint Opacity { get { return _opacity; } }
            public byte[] Unk1 { get { return _unk1; } }
            public uint Colors { get { return _colors; } }
            public byte[] Unk2 { get { return _unk2; } }
            public float USize { get { return _uSize; } }
            public float VSize { get { return _vSize; } }
            public byte[] Unk3 { get { return _unk3; } }
            public float StretchFactor1 { get { return _stretchFactor1; } }
            public float StretchFactor2 { get { return _stretchFactor2; } }
            public byte[] Unk4 { get { return _unk4; } }
            public Sltm M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class Version1Struct : KaitaiStruct
        {
            public static Version1Struct FromFile(string fileName)
            {
                return new Version1Struct(new KaitaiStream(fileName));
            }

            public Version1Struct(KaitaiStream p__io, Sltm p__parent = null, Sltm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk1 = m_io.ReadBytes(17);
                _textures = new TexturesStruct(m_io, this, m_root);
                __raw_data = m_io.ReadBytes(96);
                var io___raw_data = new KaitaiStream(__raw_data);
                _data = new MaterialDataStruct(io___raw_data, this, m_root);
                if (!(M_Io.IsEof))
                {
                    _submtl = new List<RecStruct>();
                    {
                        var i = 0;
                        while (!m_io.IsEof)
                        {
                            _submtl.Add(new RecStruct(m_io, this, m_root));
                            i++;
                        }
                    }
                }
            }
            private byte[] _unk1;
            private TexturesStruct _textures;
            private MaterialDataStruct _data;
            private List<RecStruct> _submtl;
            private Sltm m_root;
            private Sltm m_parent;
            private byte[] __raw_data;
            public byte[] Unk1 { get { return _unk1; } }
            public TexturesStruct Textures { get { return _textures; } }
            public MaterialDataStruct Data { get { return _data; } }
            public List<RecStruct> Submtl { get { return _submtl; } }
            public Sltm M_Root { get { return m_root; } }
            public Sltm M_Parent { get { return m_parent; } }
            public byte[] M_RawData { get { return __raw_data; } }
        }
        public partial class TexturesStruct : KaitaiStruct
        {
            public static TexturesStruct FromFile(string fileName)
            {
                return new TexturesStruct(new KaitaiStream(fileName));
            }

            public TexturesStruct(KaitaiStream p__io, Sltm.Version1Struct p__parent = null, Sltm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _baseMap = new RecStruct(m_io, this, m_root);
                _rmMap = new RecStruct(m_io, this, m_root);
                _normalMap = new RecStruct(m_io, this, m_root);
                _aoMap = new RecStruct(m_io, this, m_root);
                _siMap = new RecStruct(m_io, this, m_root);
                _colorizationMap = new RecStruct(m_io, this, m_root);
                _specialMap = new RecStruct(m_io, this, m_root);
            }
            private RecStruct _baseMap;
            private RecStruct _rmMap;
            private RecStruct _normalMap;
            private RecStruct _aoMap;
            private RecStruct _siMap;
            private RecStruct _colorizationMap;
            private RecStruct _specialMap;
            private Sltm m_root;
            private Sltm.Version1Struct m_parent;
            public RecStruct BaseMap { get { return _baseMap; } }
            public RecStruct RmMap { get { return _rmMap; } }
            public RecStruct NormalMap { get { return _normalMap; } }
            public RecStruct AoMap { get { return _aoMap; } }
            public RecStruct SiMap { get { return _siMap; } }
            public RecStruct ColorizationMap { get { return _colorizationMap; } }
            public RecStruct SpecialMap { get { return _specialMap; } }
            public Sltm M_Root { get { return m_root; } }
            public Sltm.Version1Struct M_Parent { get { return m_parent; } }
        }
        public partial class Textures0Struct : KaitaiStruct
        {
            public static Textures0Struct FromFile(string fileName)
            {
                return new Textures0Struct(new KaitaiStream(fileName));
            }

            public Textures0Struct(KaitaiStream p__io, Sltm.Version0Struct p__parent = null, Sltm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _baseMap = new Rec0Struct(m_io, this, m_root);
                _rmMap = new Rec0Struct(m_io, this, m_root);
                _normalMap = new Rec0Struct(m_io, this, m_root);
                _aoMap = new Rec0Struct(m_io, this, m_root);
                _siMap = new Rec0Struct(m_io, this, m_root);
                _colorizationMap = new Rec0Struct(m_io, this, m_root);
                _specialMap = new Rec0Struct(m_io, this, m_root);
            }
            private Rec0Struct _baseMap;
            private Rec0Struct _rmMap;
            private Rec0Struct _normalMap;
            private Rec0Struct _aoMap;
            private Rec0Struct _siMap;
            private Rec0Struct _colorizationMap;
            private Rec0Struct _specialMap;
            private Sltm m_root;
            private Sltm.Version0Struct m_parent;
            public Rec0Struct BaseMap { get { return _baseMap; } }
            public Rec0Struct RmMap { get { return _rmMap; } }
            public Rec0Struct NormalMap { get { return _normalMap; } }
            public Rec0Struct AoMap { get { return _aoMap; } }
            public Rec0Struct SiMap { get { return _siMap; } }
            public Rec0Struct ColorizationMap { get { return _colorizationMap; } }
            public Rec0Struct SpecialMap { get { return _specialMap; } }
            public Sltm M_Root { get { return m_root; } }
            public Sltm.Version0Struct M_Parent { get { return m_parent; } }
        }
        public partial class Rec0Struct : KaitaiStruct
        {
            public static Rec0Struct FromFile(string fileName)
            {
                return new Rec0Struct(new KaitaiStream(fileName));
            }

            public Rec0Struct(KaitaiStream p__io, KaitaiStruct p__parent = null, Sltm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _data = new LenStr(m_io, this, m_root);
            }
            private LenStr _data;
            private Sltm m_root;
            private KaitaiStruct m_parent;
            public LenStr Data { get { return _data; } }
            public Sltm M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class HeaderStruct : KaitaiStruct
        {
            public static HeaderStruct FromFile(string fileName)
            {
                return new HeaderStruct(new KaitaiStream(fileName));
            }

            public HeaderStruct(KaitaiStream p__io, Sltm p__parent = null, Sltm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _magic = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Magic, new byte[] { 83, 76, 84, 77 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 83, 76, 84, 77 }, Magic, M_Io, "/types/header_struct/seq/0");
                }
                _version = m_io.ReadU2le();
                _unk1 = m_io.ReadU2le();
                _magic2 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Magic2, new byte[] { 76, 84, 77, 66 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 76, 84, 77, 66 }, Magic2, M_Io, "/types/header_struct/seq/3");
                }
            }
            private byte[] _magic;
            private ushort _version;
            private ushort _unk1;
            private byte[] _magic2;
            private Sltm m_root;
            private Sltm m_parent;
            public byte[] Magic { get { return _magic; } }
            public ushort Version { get { return _version; } }
            public ushort Unk1 { get { return _unk1; } }
            public byte[] Magic2 { get { return _magic2; } }
            public Sltm M_Root { get { return m_root; } }
            public Sltm M_Parent { get { return m_parent; } }
        }
        public partial class RecStruct : KaitaiStruct
        {
            public static RecStruct FromFile(string fileName)
            {
                return new RecStruct(new KaitaiStream(fileName));
            }

            public RecStruct(KaitaiStream p__io, KaitaiStruct p__parent = null, Sltm p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _recType = m_io.ReadU1();
                switch (RecType)
                {
                    case 2:
                        {
                            _data = new LenStr(m_io, this, m_root);
                            break;
                        }
                }
            }
            private byte _recType;
            private LenStr _data;
            private Sltm m_root;
            private KaitaiStruct m_parent;
            public byte RecType { get { return _recType; } }
            public LenStr Data { get { return _data; } }
            public Sltm M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        private HeaderStruct _header;
        private RecStruct _mtlName;
        private KaitaiStruct _mtl;
        private Sltm m_root;
        private KaitaiStruct m_parent;
        public HeaderStruct Header { get { return _header; } }
        public RecStruct MtlName { get { return _mtlName; } }
        public KaitaiStruct Mtl { get { return _mtl; } }
        public Sltm M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}

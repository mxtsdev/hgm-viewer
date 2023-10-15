using System;
using System.Collections.Generic;
using System.Linq;
using static HgmViewer.Formats.Hgm;

namespace HgmViewer.Formats
{
    public class IndexField
    {
        public int Index { get; set; }
        public RecordElement Field { get; set; }
    }

    public partial class Hgm
    {
        public partial class Mesh
        {
            private List<IndexField> _indexedFields = null;

            private void InitIndexedFields()
            {
                var fields = m_root.Meshes.SelectMany(x => x.Fields.Select(y => y.Data)).OfType<RecordElement>().ToArray();

                _indexedFields = new List<IndexField>();
                for (var i = 0; i < Fields.Count; i++)
                {
                    var f = Fields[i];
                    var re = f.Data as RecordElement;

                    if (re == null && f.Data is RecordCondensedElement rce)
                        re = fields[(int)rce.FieldIndex];

                    if (re == null)
                        throw new ApplicationException($"[{nameof(Hgm)}.{nameof(GetFieldByName)}] Unhandled field type: {f.Data.GetType().FullName}");

                    _indexedFields.Add(new IndexField { Field = re, Index = i });
                }
            }

            public IndexField GetFieldByName(string fieldName)
            {
                if (_indexedFields == null)
                    InitIndexedFields();

                var found = _indexedFields.SingleOrDefault(x => x.Field.Key.Str == fieldName);

                return found;

                //var meshes = new[] { mesh, Meshes[0] }; // look in current mesh, then first mesh
                //foreach (var m in meshes)
                //{
                //    var found = m.Fields.Select((x, i) => new IndexField { Field = x.Data as RecordElement, Index = i }).SingleOrDefault(x => x.Field?.Key.Str == fieldName);

                //    if (found != null)
                //        return found;
                //}

                //return null;
            }
        }
    }
}
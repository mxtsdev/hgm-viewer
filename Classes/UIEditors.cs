using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;

namespace HgmViewer.Classes
{
    //[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    //public class FolderNameEditorExConfigAttribute : Attribute
    //{
    //    public string Title { get; set; }
    //}

    public class FolderNameEditorEx : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            //var property = context.PropertyDescriptor;
            //var config = property.Attributes[typeof(FolderNameEditorExConfigAttribute)] as FolderNameEditorExConfigAttribute;
            var path = value as string;
            var isRelativePath = Path.IsPathFullyQualified(path);
            var modifiedPath = isRelativePath ? path : Path.GetFullPath(path);

            var dlg = new FolderBrowserDialog { SelectedPath = modifiedPath };
            InitializeDialog(dlg);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var currentDirPath = Directory.GetCurrentDirectory();
                var (isSubPath, relativePath) = Helpers.GetRelativePathOf(currentDirPath, dlg.SelectedPath);
                return isSubPath ? relativePath : dlg.SelectedPath;
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.Modal;

        protected virtual void InitializeDialog(FolderBrowserDialog folderBrowserDialog)
        {
        }
    }
}

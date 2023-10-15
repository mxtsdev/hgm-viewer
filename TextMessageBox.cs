using System;
using System.Windows.Forms;

namespace HgmViewer
{
    /// <summary>
    /// Message box dialog with room for more text
    /// </summary>
    public partial class TextMessageBox : Form
    {
        public string Title { get => Text; set => Text = value; }
        public string ShortMessage
        {
            get => tbShortMessage.Text;
            set { tbShortMessage.Text = value; OnShortMessageSet(); }
        }
        public string Message { get => tbMessage.Text; set => tbMessage.Text = value; }

        public TextMessageBox()
        {
            InitializeComponent();
        }

        private void OnShortMessageSet()
        {
            using var g = tbShortMessage.CreateGraphics();
            var strHeight = (int)Math.Round(g.MeasureString(tbShortMessage.Text, tbShortMessage.Font).Height);
            var strHeight2 = (int)Math.Round(g.MeasureString("ABC", tbShortMessage.Font).Height);
            var strHeight3 = tbShortMessage.PreferredHeight;

            tbShortMessage.Height = strHeight3 - strHeight2 + strHeight;

            var newTop = tbShortMessage.Bottom + 5;
            var diff = tbMessage.Top - newTop;
            tbMessage.Top = newTop;
            tbMessage.Height += diff;
        }
    }
}
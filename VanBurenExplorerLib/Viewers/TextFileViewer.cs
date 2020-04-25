using System.IO;
using System.Windows.Forms;
using VanBurenExplorerLib.Files;

namespace VanBurenExplorerLib.Viewers
{
    public class TextFileViewer : IFileViewer
    {
        private readonly VanBurenFile _file;

        public TextFileViewer(VanBurenFile file)
        {
            _file = file;
        }

        public Control GetControl()
        {
            return new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Text = File.ReadAllText(_file.FullPath)
            };
        }
    }
}
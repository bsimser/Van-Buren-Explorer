using System.IO;
using System.Windows.Forms;
using Be.Windows.Forms;
using VanBurenExplorerLib.Models;

namespace VanBurenExplorerLib.Views
{
    /// <summary>
    /// Used the old(ish) hex editor control from the hex editor project
    /// https://www.nuget.org/packages/Be.Windows.Forms.HexBox/
    /// https://sourceforge.net/projects/hexbox/
    /// </summary>
    public class GenericFileViewer : IFileViewer
    {
        private readonly VanBurenFile _file;

        public GenericFileViewer(VanBurenFile file)
        {
            _file = file;
        }

        public Control GetControl()
        {
            var provider = new DynamicByteProvider(File.ReadAllBytes(_file.FullPath));
            return new HexBox
            {
                ColumnInfoVisible = true,
                LineInfoVisible = true,
                StringViewVisible = true,
                UseFixedBytesPerLine = true,
                VScrollBarVisible = true,
                ByteProvider = provider,
                Dock = DockStyle.Fill
            };
        }
    }
}
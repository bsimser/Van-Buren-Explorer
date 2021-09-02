using System.Windows.Forms;
using VanBurenExplorerLib.Models;

namespace VanBurenExplorerLib.Views
{
    public class TgaFileViewer : IFileViewer
    {
        private readonly VanBurenFile _file;

        public TgaFileViewer(VanBurenFile file)
        {
            _file = file;
        }

        public Control GetControl()
        {
            var control = new PictureBox();
            control.Dock = DockStyle.Fill;
            control.SizeMode = PictureBoxSizeMode.CenterImage;
            // TODO read in file and convert to bitmap
            // control.Image = result
            return control;
        }
    }
}
using System.Windows.Forms;
using VanBurenExplorerLib.Files;

namespace VanBurenExplorerLib.Viewers
{
    public class GenericFileViewer : IFileViewer
    {
        private readonly VanBurenFile _file;

        public GenericFileViewer(VanBurenFile file)
        {
            _file = file;
        }

        public Control GetControl()
        {
            return new Label
            {
                Text = _file.FullPath, 
                AutoSize = true
            };
        }
    }
}
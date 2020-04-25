using System.Windows.Forms;

namespace VanBurenExplorerLib
{
    public interface IView
    {
        void SetControl(Control control);
        void SetStatusText(string text);
    }
}
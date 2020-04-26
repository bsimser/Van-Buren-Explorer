using System.Windows.Forms;

namespace VanBurenExplorerLib.Views
{
    /// <summary>
    /// The main view interface. The view is passive and routes
    /// user-initiated events such as mouse click commands to
    /// the presenter to act upon that data.
    /// </summary>
    public interface IMainView
    {
        void SetControl(Control control);
        void SetStatusText(string text);
        void ChangeDirectory(string path);
    }
}
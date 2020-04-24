using System.IO;
using VanBurenExplorerLib;

namespace VanBurenExplorer
{
    /// <summary>
    /// Poor man's MVP pattern for WinForms
    /// </summary>
    public class MainViewPresenter
    {
        // the view is attached to the main form and has access to it's controls
        private readonly IView _view;

        public MainViewPresenter(IView view)
        {
            _view = view;
        }

        public void Init()
        {
        }

        public void LoadFile(FileInfo file)
        {
            // update the views status with the file we just processed
            _view.SetStatusText(file.Name);
        }
    }
}
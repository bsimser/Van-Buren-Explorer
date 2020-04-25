using System.IO;
using VanBurenExplorerLib;
using VanBurenExplorerLib.Files;
using VanBurenExplorerLib.Viewers;

namespace VanBurenExplorer
{
    /// <summary>
    /// Poor man's MVP pattern for WinForms
    /// </summary>
    public class MainViewPresenter
    {
        // the view is attached to the main form and has access to it's controls
        private readonly IView _view;
        // the interface to our custom viewer
        private IFileViewer _fileViewer;

        public MainViewPresenter(IView view)
        {
            _view = view;
        }

        public void Init()
        {
        }

        public void LoadFile(FileInfo file)
        {
            // create a concrete file using our file factory
            var vbFile = FileFactory.CreateUsing(new FileProperties {FullPath = file.FullName});
            // create a concrete viewer based on the file type
            _fileViewer = ViewerFactory.CreateUsing(new ViewerProperties {File = vbFile});
            // TODO display the file in our view
            _view.SetControl(_fileViewer.GetControl());
            // update the views status with the file we just processed
            _view.SetStatusText(file.Name);
        }
    }
}
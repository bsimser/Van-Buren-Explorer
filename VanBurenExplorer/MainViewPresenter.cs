using System.Collections.Generic;
using System.IO;
using System.Linq;
using VanBurenExplorerLib.Models;
using VanBurenExplorerLib.Views;

namespace VanBurenExplorer
{
    /// <summary>
    /// Poor man's MVP pattern for WinForms. The presenter's job is to
    /// instruct the view what to display. The view is also allowed to
    /// communicate back to the presenter (by raising events).
    /// </summary>
    public class MainViewPresenter
    {
        // the view is attached to the main form and has access to it's controls
        private readonly IMainView _mainView;
        
        // the interface to our custom viewer
        private IFileViewer _fileViewer;

        // our model to hold any data the view needs
        private MainViewModel _model;

        public MainViewPresenter(IMainView mainView)
        {
            _mainView = mainView;
            _model = new MainViewModel();
        }

        public void Init()
        {
            // TODO figure out what to do here or get rid of this
        }

        /// <summary>
        /// Main entry to load a single file from a directory, parse it, and deliver a viewer for it
        /// called whenever someone selects a file from the main list view control in the form
        /// </summary>
        /// <param name="file"></param>
        public void LoadFile(FileInfo file)
        {
            // create a concrete file using our file factory
            var vbFile = FileFactory.CreateUsing(new FileProperties {FullPath = file.FullName});

            // create a concrete viewer based on the file type and pass it the resource "catalog"
            _fileViewer = ViewerFactory.CreateUsing(new ViewerProperties {File = vbFile, Catalog = _model.ResourceFile});

            // TODO display the file in our view
            _mainView.SetControl(_fileViewer.GetControl());
            
            // update the views status with the file we just processed
            _mainView.SetStatusText(file.Name);
        }

        /// <summary>
        /// After selecting a directory to load try to find the main resource file that contains our catalog of resources
        /// </summary>
        /// <param name="path"></param>
        public void LoadCatalog(string path)
        {
            var catalogFile = Path.Combine(path, "resource.rht");
            if (!File.Exists(catalogFile)) return;

            _model = new MainViewModel
            {
                CurrentDirectory = path
            };
            var catalog = new RHTFile();
            using (var reader = new BinaryReader(File.OpenRead(catalogFile)))
            {
                // read the header of the main file
                var header = new F3RHTHeader
                {
                    Major = reader.ReadInt32(), 
                    Minor = reader.ReadInt32(), 
                    NumEntries = reader.ReadInt32(),
                    OffsetToGroupNames = reader.ReadInt32(),
                    OffsetToResourceNames = reader.ReadInt32()
                };
                
                // allocate the entries
                var entries = new List<Entry>(header.NumEntries);
                
                // read in the entries
                for (var i = 0; i < header.NumEntries; i++)
                {
                    entries.Add(new Entry
                    {
                        Version = reader.ReadInt32(),
                        Index = reader.ReadInt32(),
                        type = (LumpType) reader.ReadInt32(),
                        RawOffsetToName = reader.ReadInt32(),
                        RawOffsetToGroupName = reader.ReadInt32()
                    });
                }

                // read and convert the .grp file names into a list
                var names = GetNullTerminatedStringsFrom(reader.ReadBytes(header.OffsetToResourceNames - header.OffsetToGroupNames));

                // read and convert the entry names into a list
                var strings = GetNullTerminatedStringsFrom(reader.ReadBytes((int) (reader.BaseStream.Length - header.OffsetToResourceNames)));

                // now loop through the entries again assigning the grp file name and entry name
                // TODO if we move the file position we can read the names and strings before the entries and do the loop once
                for (var i = 0; i < header.NumEntries; i++)
                {
                    var entry = entries[i];
                    entry.GroupName = GetStringFromOffset(names, entry.RawOffsetToGroupName);
                    entry.Name = GetStringFromOffset(strings, entry.RawOffsetToName);
                }

                catalog.Header = header;
                catalog.Entries = entries;
            }
            _model.ResourceFile = catalog;
        }

        private string GetStringFromOffset(IEnumerable<StringWithOffset> strings, int offset)
        {
            return strings.Single(x => x.Offset == offset).Name;
        }

        /// <summary>
        /// Converts a null terminated group of bytes into a list of strings storing the offset as we go
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static IList<StringWithOffset> GetNullTerminatedStringsFrom(byte[] data)
        {
            var result = new List<StringWithOffset>();
            var index = 0;
            for (var i = 0; i < data.Length; i++)
            {
                if (data[i] != 0) continue;
                result.Add(new StringWithOffset{
                    Offset = index,
                    Name = System.Text.Encoding.UTF8.GetString(data, index, i - index)
                });
                index = i + 1;
            }
            return result;
        }
    }
}
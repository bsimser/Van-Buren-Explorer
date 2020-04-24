using System;
using System.IO;
using System.Windows.Forms;
using VanBurenExplorerLib;

namespace VanBurenExplorer
{
    public partial class MainForm : Form, IView
    {
        private readonly MainViewPresenter _presenter;

        public MainForm()
        {
            InitializeComponent();
            _presenter = new MainViewPresenter(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutVanBurenExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void DirectoryClick(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                PopulateTreeView(folderBrowserDialog1.SelectedPath);
            }
        }

        private void PopulateTreeView(string path)
        {
            var info = new DirectoryInfo(path);
            if (!info.Exists) return;
            var rootNode = new TreeNode(info.Name) {Tag = info};
            GetDirectories(info.GetDirectories(), rootNode);
            treeView.Nodes.Add(rootNode);
            rootNode.Expand();
        }

        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            foreach (var subDir in subDirs)
            {
                try
                {
                    var aNode = new TreeNode(subDir.Name, 0, 0) { Tag = subDir, ImageKey = "folder" };
                    var subSubDirs = subDir.GetDirectories();
                    if (subSubDirs.Length != 0)
                    {
                        GetDirectories(subSubDirs, aNode);
                    }
                    nodeToAddTo.Nodes.Add(aNode);
                }
                catch (UnauthorizedAccessException)
                {
                    // ok, so we are not allowed to dig into that directory. Move on...
                }
            }
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            using (new WaitCursor())
            {
                if (!e.Node.IsSelected) return;
                var newSelected = e.Node;
                listView.Items.Clear();
                var nodeDirInfo = (DirectoryInfo) newSelected.Tag;
                ListViewItem.ListViewSubItem[] subItems;
                foreach (var dir in nodeDirInfo.GetDirectories())
                {
                    var item = new ListViewItem(dir.Name, 0);
                    subItems = new[] {
                        new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToString("g")),
                        new ListViewItem.ListViewSubItem(item, "File folder")
                    };
                    item.SubItems.AddRange(subItems);
                    listView.Items.Add(item);
                }
                foreach (var file in nodeDirInfo.GetFiles())
                {
                    var item = new ListViewItem(file.Name, 1);
                    item.Tag = file;
                    subItems = new[] { 
                        // TODO rather than call this "File" come up with a better name based on it's type
                        new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToString("g")),
                        new ListViewItem.ListViewSubItem(item, "File"),
                        new ListViewItem.ListViewSubItem(item, GetFormattedSize(file.Length))
                    };
                    item.SubItems.AddRange(subItems);
                    listView.Items.Add(item);
                }

                directoryTextBox.Text = nodeDirInfo.FullName;
                listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        /// <summary>
        /// Get human readable size
        /// https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string GetFormattedSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            var order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return $"{len:0} {sizes[order]}";
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                // TODO get currently selected file
                var item = listView.FocusedItem;
                var info = (FileInfo) item?.Tag;
                if (info == null) return;
                // TODO process it and assign it to the plugin for viewing
                // TODO _presenter.LoadFile();
                toolStripStatusLabel1.Text = info.Name;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                _presenter.Init();
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // let the user know we're about to do this
            toolStripStatusLabel1.Text = "Loading...";
            // give the UI a kick before we start loading folders
            Application.DoEvents();
            // now we can start our load and keep the user informed
            using (new WaitCursor())
            {
                // might as well start here
                PopulateTreeView(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            }
            // once we're ready let the user know
            toolStripStatusLabel1.Text = "Ready";
        }
    }
}

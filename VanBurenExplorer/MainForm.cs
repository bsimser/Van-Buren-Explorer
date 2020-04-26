using System;
using System.IO;
using System.Windows.Forms;
using VanBurenExplorer.Properties;
using VanBurenExplorerLib;
using VanBurenExplorerLib.Views;

namespace VanBurenExplorer
{
    public partial class MainForm : Form, IMainView
    {
        private readonly MainViewPresenter _presenter;

        public MainForm()
        {
            InitializeComponent();
            _presenter = new MainViewPresenter(this);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void About_Click(object sender, EventArgs e)
        {
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void Directory_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (!result.Equals(DialogResult.OK)) return;
            ClearControls();
            ChangeDirectory(folderBrowserDialog1.SelectedPath);
        }

        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            foreach (var subDir in subDirs)
            {
                try
                {
                    var aNode = new TreeNode(subDir.Name, 0, 0) { Tag = subDir, ImageKey = Resources.TreeNodeImageKey };
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
                        new ListViewItem.ListViewSubItem(item, Resources.DirectoryListViewType)
                    };
                    item.SubItems.AddRange(subItems);
                    listView.Items.Add(item);
                }
                foreach (var file in nodeDirInfo.GetFiles())
                {
                    var item = new ListViewItem(file.Name, 1);
                    item.Tag = file;
                    subItems = new[] { 
                        new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToString("g")),
                        new ListViewItem.ListViewSubItem(item, Resources.FileListViewType),
                        // TODO rather than call this "File" come up with a better name based on it's type
                        new ListViewItem.ListViewSubItem(item, FileHelper.GetFormattedSize(file.Length))
                    };
                    item.SubItems.AddRange(subItems);
                    listView.Items.Add(item);
                }

                directoryTextBox.Text = nodeDirInfo.FullName;
                listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                // get currently selected file
                var item = listView.FocusedItem;
                // check if we have the full details for the file (folders won't have this)
                var file = (FileInfo) item?.Tag;
                // if this a folder just return
                if (file == null) return;
                // process it for viewing
                _presenter.LoadFile(file);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                _presenter.Init();
            }
        }

        /// <summary>
        /// The tree view load is done here after the form is displayed so the user
        /// is at least looking at something in case we have a big folder to process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            // let the user know we're about to do this
            toolStripStatusLabel1.Text = Resources.StatusLoadingText;
            // start the program off by asking where to start looking for files
            Directory_Click(this, e);
            // once we're ready let the user know
            toolStripStatusLabel1.Text = Resources.StatusReadyText;
        }

        public void SetControl(Control control)
        {
            splitContainer1.Panel1.Controls.Clear();
            splitContainer1.Panel1.Controls.Add(control);
        }

        public void SetStatusText(string text)
        {
            toolStripStatusLabel1.Text = text;
        }

        public void ChangeDirectory(string path)
        {
            using (new WaitCursor())
            {
                var info = new DirectoryInfo(path);
                if (!info.Exists) return;
                var rootNode = new TreeNode(info.Name) { Tag = info };
                GetDirectories(info.GetDirectories(), rootNode);
                treeView.Nodes.Add(rootNode);
                rootNode.Expand();
                treeView.SelectedNode = rootNode;
                _presenter.LoadCatalog(path);
            }
        }

        private void ClearControls()
        {
            treeView.Nodes.Clear();
            listView.Items.Clear();
            splitContainer1.Panel1.Controls.Clear();
        }
    }
}

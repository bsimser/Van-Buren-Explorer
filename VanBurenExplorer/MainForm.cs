using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace VanBurenExplorer
{
    public partial class MainForm : Form
    {
        private string _rootPath;

        public MainForm()
        {
            InitializeComponent();
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

        private void directoryClick(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                _rootPath = folderBrowserDialog1.SelectedPath;
                InitTreeview(_rootPath);
            }
        }

        private void InitTreeview(string path)
        {
            if (!path.EndsWith(@"\")) path += @"\";
            directoryTextBox.Text = path.ToUpper();
            var root = new TreeNode(path) { Tag = path };
            root.Nodes.Add(new TreeNode());
            treeView.Nodes.Clear();
            treeView.Nodes.Add(root);
            root.Expand();
            treeView.SelectedNode = root;
        }

        /// <summary>
        /// Before expanding a folder populate the subdirectories
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                LoadDirectory(e.Node, _rootPath);
            }
        }

        private void LoadDirectory(TreeNode node, string path)
        {
            node.Nodes.Clear();
            var dir = new DirectoryInfo(path);
            var folders = dir.GetDirectories();
            foreach (var treeNode in folders.Select(subdir => new TreeNode(subdir.Name) { Tag = subdir}))
            {
                treeNode.Nodes.Add(new TreeNode());
                node.Nodes.Add(treeNode);
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            using (new WaitCursor())
            {
                if (e.Node.IsSelected)
                {
                    // TODO load file directory
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                // TODO get currently selected file
                // TODO process it and assign it to the plugin for viewing
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}

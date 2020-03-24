using CSharpIDE.Models;
using CSharpIDE.Views.Interfaces;
using FastColoredTextBoxNS;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpIDE.Views
{
    public partial class MainWindow : Form, IMainWindow
    {
        private Project project;
        public CompilerResults Results { get; set; }
        private ImageList imageList;
        public Project Project { get => project; set => project = value; }
        public string OFDFileName {get;set;}
        public string OFDPath { get; set; }
        public string SelectedFileName { get; set; }
        public ProjectFile ProjectFile { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Project = new Project();
            imageList = new ImageList();
            AddImagesToList();
        }

        #region private functions

        private void AddImagesToList()
        {
            imageList.Images.Add("Parent", Image.FromFile(@"..\..\..\Images\Parent.png"));
            imageList.Images.Add("Child", Image.FromFile(@"..\..\..\Images\Child.png"));
            ProjectTreeView.ImageList = imageList;
            ProjectTreeView.ImageIndex = 1;
            ProjectTreeView.SelectedImageIndex = 1;
        }
        private void RefreshTreeView()
        {
            if (Project.Name != null)
            {
                ProjectTreeView.Nodes.Clear();
                TreeNode rootnode = new TreeNode(Project.Name);

                ProjectTreeView.Nodes.Add(rootnode);
                rootnode.ImageIndex = 1;
                rootnode.SelectedImageIndex = 1;

                foreach (var item in Project.Files)
                {
                    rootnode.Nodes.Add(item.Name);
                    rootnode.Nodes[rootnode.Nodes.Count - 1].ContextMenuStrip = TreeViewContextMenu;
                    rootnode.ImageIndex = 0;
                    rootnode.SelectedImageIndex = 0;
                }
                ProjectTreeView.ExpandAll();
            }
        }
        private void AddNewTab(ProjectFile file)
        {
            TabPage tabPage = new TabPage();
            FastColoredTextBox fastColoredTextBox = new FastColoredTextBox();
            fastColoredTextBox.Dock = DockStyle.Fill;
            fastColoredTextBox.Language = Language.CSharp;
            fastColoredTextBox.Text = file.Data;
            fastColoredTextBox.TextChanged += Ts_TextChanged;
            fastColoredTextBox.ContextMenuStrip = TabContextMenu;
            tabPage.Text = file.Name;
            tabPage.BorderStyle = BorderStyle.FixedSingle;
            tabPage.Controls.Add(fastColoredTextBox);
            MainTabControl.TabPages.Add(tabPage);
            MainTabControl.SelectedTab = MainTabControl.TabPages[MainTabControl.TabPages.Count - 1];
        }
        private void Ts_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MainTabControl.SelectedTab.Text[MainTabControl.SelectedTab.Text.Length - 1] != '*')
                MainTabControl.SelectedTab.Text += "*";
        }

        #endregion

        #region events

        public event EventHandler NewProjectEvent;
        public event EventHandler OpenProjectEvent;
        public event EventHandler NewFileEvent;
        public event EventHandler SaveFocusedFile;
        public event EventHandler SaveAllFiles;
        public event EventHandler BuildProject;
        public event EventHandler RunProject;
        public event EventHandler OpenFileEvent;
        public event EventHandler RemoveFile;
        public event EventHandler ExcludeFile;

        #endregion

        #region menustrip

        private void createProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProjectEvent.Invoke(sender, e);
            RefreshTreeView();
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fd = new OpenFileDialog())
            {
                fd.Filter = "mysln file|*.mysln";
                DialogResult result = fd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fd.FileName) && fd.FileName.Contains(".mysln"))
                {
                    OFDFileName = Path.GetFileNameWithoutExtension(fd.FileName);
                    OFDPath = Path.GetDirectoryName(fd.FileName);
                    OpenProjectEvent.Invoke(sender, e);
                    RefreshTreeView();
                }
            }
            //MessageBox.Show(OFDPath + "\\" + OFDFileName);
            
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSharp file|*.cs";

                DialogResult result = sfd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ProjectFile = new ProjectFile() { Name = Path.GetFileNameWithoutExtension(sfd.FileName) + ".cs" };
                    NewFileEvent.Invoke(sender, e);
                    RefreshTreeView();
                }
            }
            
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fd = new OpenFileDialog())
            {
                fd.Filter = "cs file|*.cs";
                DialogResult result = fd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fd.FileName) && fd.FileName.Contains(".cs"))
                {
                    OFDFileName = Path.GetFileNameWithoutExtension(fd.FileName);
                    OFDPath = Path.GetDirectoryName(fd.FileName);
                    OpenFileEvent.Invoke(sender, e);
                    RefreshTreeView();
                }
            }
            
        }

        private void saveFocusedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainTabControl.SelectedTab.Text[MainTabControl.SelectedTab.Text.Length - 1] == '*')
            {
                MainTabControl.SelectedTab.Text = MainTabControl.SelectedTab.Text.Remove(MainTabControl.SelectedTab.Text.Length - 1, 1);
                ProjectFile = new ProjectFile() { Name = MainTabControl.SelectedTab.Text, Data = (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Text };
                //MessageBox.Show(ProjectFile.Name);
                //MessageBox.Show(ProjectFile.Data);
                SaveFocusedFile.Invoke(sender, e);
                RefreshTreeView();
            }
            
        }

        private void saveAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (TabPage item in MainTabControl.TabPages)
            {
                if (item.Text[item.Text.Length - 1] == '*')
                {
                    item.Text = item.Text.Remove(item.Text.Length - 1, 1);
                    foreach (ProjectFile _file in project.Files)
                    {
                        if (_file.Name == item.Text) _file.Data = (item.Controls[0] as FastColoredTextBox).Text;
                    }
                }
            }
            SaveAllFiles.Invoke(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Project.Name!=null)
            {
                var confirmResult = MessageBox.Show("Do you want to save changes?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    saveAllFilesToolStripMenuItem_Click(sender, e);
                }
            }
            Environment.Exit(0);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Paste();
        }

        private void addNewFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFileToolStripMenuItem_Click(sender, e);
        }

        private void buildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuildProject.Invoke(sender, e);
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunProject.Invoke(sender, e);
            LoadResults();
        }

        private void LoadResults()
        {
            if (Results.Errors.Count == 0 && Results != null)
            {
                #region Errors
                TableLayoutPanel panel = new TableLayoutPanel();
                panel.Dock = DockStyle.Fill;
                panel.AutoScroll = true;
                panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                panel.ColumnCount = 4;
                panel.RowCount = 1;
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

                panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
                panel.Controls.Add(new Label() { Text = "Error Code" }, 0, 0);
                panel.Controls.Add(new Label() { Text = "Message" }, 1, 0);
                panel.Controls.Add(new Label() { Text = "Line" }, 2, 0);
                panel.Controls.Add(new Label() { Text = "File" }, 3, 0);
                panel.RowCount = panel.RowCount + 1;
                panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));

                TabControlBottom.TabPages[0].Controls.Clear();
                TabControlBottom.TabPages[0].Controls.Add(panel);

                #endregion

                #region Warnings

                TableLayoutPanel warningpanel = new TableLayoutPanel();
                warningpanel.Dock = DockStyle.Fill;
                warningpanel.AutoScroll = true;
                warningpanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                warningpanel.ColumnCount = 4;
                warningpanel.RowCount = 1;
                warningpanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                warningpanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
                warningpanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                warningpanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

                warningpanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
                warningpanel.Controls.Add(new Label() { Text = "Warning Code" }, 0, 0);
                warningpanel.Controls.Add(new Label() { Text = "Message" }, 1, 0);
                warningpanel.Controls.Add(new Label() { Text = "Line" }, 2, 0);
                warningpanel.Controls.Add(new Label() { Text = "File" }, 3, 0);
                warningpanel.RowCount = warningpanel.RowCount + 1;
                warningpanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));

                TabControlBottom.TabPages[1].Controls.Clear();
                TabControlBottom.TabPages[1].Controls.Add(warningpanel);

                #endregion

                #region Output
                string tmp = string.Empty;
                TabControlBottom.SelectedTab = TabControlBottom.TabPages[2];
                TabControlBottom.TabPages[2].Controls[0].Text = $"Build started: {Project.Name}\nPath: {Project.Path}\nBuild: 1 succeeded, 0 failed, 0 up-to-date, 0 skipped";
                #endregion
            }
            else
            {
                #region Warnings
                TableLayoutPanel warningpanel = new TableLayoutPanel();
                warningpanel.Dock = DockStyle.Fill;
                warningpanel.AutoScroll = true;
                warningpanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                warningpanel.ColumnCount = 4;
                warningpanel.RowCount = 1;
                warningpanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                warningpanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
                warningpanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                warningpanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

                warningpanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
                warningpanel.Controls.Add(new Label() { Text = "Warning Code" }, 0, 0);
                warningpanel.Controls.Add(new Label() { Text = "Message" }, 1, 0);
                warningpanel.Controls.Add(new Label() { Text = "Line" }, 2, 0);
                warningpanel.Controls.Add(new Label() { Text = "File" }, 3, 0);
                warningpanel.RowCount = warningpanel.RowCount + 1;
                warningpanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));

                TabControlBottom.TabPages[1].Controls.Clear();
                TabControlBottom.TabPages[1].Controls.Add(warningpanel);

                #endregion

                #region Errors

                TableLayoutPanel panel = new TableLayoutPanel();
                panel.Dock = DockStyle.Fill;
                panel.AutoScroll = true;
                panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                panel.ColumnCount = 4;
                panel.RowCount = 1;
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

                panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
                panel.Controls.Add(new Label() { Text = "Error Code" }, 0, 0);
                panel.Controls.Add(new Label() { Text = "Message" }, 1, 0);
                panel.Controls.Add(new Label() { Text = "Line" }, 2, 0);
                panel.Controls.Add(new Label() { Text = "File" }, 3, 0);


                foreach (CompilerError err in Results.Errors)
                {
                    panel.RowCount = panel.RowCount + 1;
                    panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
                    panel.Controls.Add(new Label() { Text = err.ErrorNumber }, 0, panel.RowCount - 1);
                    panel.Controls.Add(new Label() { Text = err.ErrorText }, 1, panel.RowCount - 1);
                    panel.Controls.Add(new Label() { Text = err.Line.ToString() }, 2, panel.RowCount - 1);
                    panel.Controls.Add(new Label() { Text = err.FileName }, 3, panel.RowCount - 1);
                }
                panel.RowCount = panel.RowCount + 1;
                panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));

                TabControlBottom.TabPages[0].Controls.Clear();
                TabControlBottom.TabPages[0].Controls.Add(panel);
                TabControlBottom.SelectedTab = TabControlBottom.TabPages[0];

                #endregion
            }
        }

        private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Project.Name != null)
            {
                var confirmResult = MessageBox.Show("Do you want to save changes?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    saveAllFilesToolStripMenuItem_Click(sender, e);
                }
            }
            Project = null;
            ProjectTreeView.Nodes.Clear();
            MainTabControl.TabPages.Clear();
        }

        private void projectTreeCollapsedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (projectTreeCollapsedToolStripMenuItem.Checked)
            {
                projectTreeCollapsedToolStripMenuItem.Checked = false;
                splitContainer2.Panel1Collapsed = false;
            }
            else 
            { 
                projectTreeCollapsedToolStripMenuItem.Checked = true;
                splitContainer2.Panel1Collapsed = true;
            }
        }

        private void errorsTreeCollapsedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(errorsTreeCollapsedToolStripMenuItem.Checked)
            {
                errorsTreeCollapsedToolStripMenuItem.Checked = false;
                splitContainer1.Panel2Collapsed = false;
            }
            else
            {
                errorsTreeCollapsedToolStripMenuItem.Checked = true;
                splitContainer1.Panel2Collapsed = true;
            }
        }

        #endregion

        #region toolstrip

        private void NewProjectTS_Click(object sender, EventArgs e)
        {
            createProjectToolStripMenuItem_Click(sender, e);
        }

        private void OpenProjectTS_Click(object sender, EventArgs e)
        {
            openProjectToolStripMenuItem_Click(sender, e);
            
        }

        private void NewFileTS_Click(object sender, EventArgs e)
        {
            newFileToolStripMenuItem_Click(sender, e);
        }

        private void OpenFileTS_Click(object sender, EventArgs e)
        {
            openFileToolStripMenuItem_Click(sender, e);
        }

        private void SaveFileTS_Click(object sender, EventArgs e)
        {
            saveFocusedFileToolStripMenuItem_Click(sender, e);
        }

        private void SaveAllFilesTS_Click(object sender, EventArgs e)
        {
            saveAllFilesToolStripMenuItem_Click(sender, e);
        }

        private void CutTS_Click(object sender, EventArgs e)
        {
            cutToolStripMenuItem_Click(sender, e);
        }

        private void CopyTS_Click(object sender, EventArgs e)
        {
            copyToolStripMenuItem_Click(sender, e);
        }

        private void PasteTS_Click(object sender, EventArgs e)
        {
            pasteToolStripMenuItem_Click(sender, e);
        }

        private void BuildTS_Click(object sender, EventArgs e)
        {
            buildToolStripMenuItem_Click(sender, e);
        }

        private void RunTS_Click(object sender, EventArgs e)
        {
            runToolStripMenuItem_Click(sender, e);
        }

        private void CommentTS_Click(object sender, EventArgs e)
        {
            (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).CommentSelected();
        }

        #endregion

        #region projecttree

        private void ProjectTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            bool chk = false;
            foreach (var item in Project.Files)
            {
                if(item.Name == e.Node.Text)
                {
                    foreach (var itemT in MainTabControl.TabPages)
                    {
                        if((itemT as TabPage).Text == item.Name) {
                            MainTabControl.SelectedTab = (itemT as TabPage);
                            chk = true;
                        }
                    }
                    if (!chk) AddNewTab(item);
                }
            }
        }

        #endregion

        #region contextMenuTab
        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            cutToolStripMenuItem_Click(sender, e);
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            copyToolStripMenuItem_Click(sender, e);
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pasteToolStripMenuItem_Click(sender, e);
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFocusedFileToolStripMenuItem_Click(sender, e);
        }

        private void lineUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int line = (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Selection.Start.iLine;
            if(line>0)
            {
                string linetext = (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Lines[line];
                string linetext2 = (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Lines[line-1];
                (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Selection = new Range((MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox), line);
                (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).SelectedText = linetext2;
                (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Selection = new Range((MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox), line-1);
                (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).SelectedText = linetext;
            }
        }

        private void lineDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int line = (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Selection.Start.iLine;
            if (line < (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).LinesCount-1)
            {
                string linetext = (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Lines[line];
                string linetext2 = (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Lines[line + 1];
                (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Selection = new Range((MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox), line);
                (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).SelectedText = linetext2;
                (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).Selection = new Range((MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox), line + 1);
                (MainTabControl.SelectedTab.Controls[0] as FastColoredTextBox).SelectedText = linetext;
            }
        }

        private void closePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainTabControl.SelectedTab.Text[MainTabControl.SelectedTab.Text.Length-1]=='*')
            {
                var confirmResult = MessageBox.Show("Do you want to save changes?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    saveFileToolStripMenuItem_Click(sender, e);
                }
            }
            MainTabControl.TabPages.Remove(MainTabControl.SelectedTab);
        }

        #endregion

        #region treeviewcontextmenu
        private void excludeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedFileName = ProjectTreeView.SelectedNode.Text;
            for (int i = 0; i < MainTabControl.TabPages.Count; i++)
            {
                if (MainTabControl.TabPages[i].Text == SelectedFileName)
                {
                    MainTabControl.TabPages.RemoveAt(i);
                    break;
                }
            }
            ExcludeFile.Invoke(sender, e);
            RefreshTreeView();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedFileName = ProjectTreeView.SelectedNode.Text;
            for (int i = 0; i < MainTabControl.TabPages.Count; i++)
            {
                if (MainTabControl.TabPages[i].Text == SelectedFileName)
                {
                    MainTabControl.TabPages.RemoveAt(i);
                    break;
                }
            }
            RemoveFile.Invoke(sender, e);
            RefreshTreeView();
        }

        private void ProjectTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            TreeNode node_here = ProjectTreeView.GetNodeAt(e.X, e.Y);
            ProjectTreeView.SelectedNode = node_here;

            if (node_here == null) return;
        }

        #endregion

        #region projectcontextmenu

        private void addNewFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            newFileToolStripMenuItem_Click(sender, e);
        }

        private void buildToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            buildToolStripMenuItem_Click(sender, e);
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            runToolStripMenuItem_Click(sender, e);
        }

        private void closeProjectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            closeProjectToolStripMenuItem_Click(sender, e);
        }

        #endregion

        #region window
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Project.Name != null)
            {
                var confirmResult = MessageBox.Show("Do you want to save changes?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    saveAllFilesToolStripMenuItem_Click(sender, e);
                }
            }
        }
        #endregion

        bool IShow.ShowDialog()
        {
            return this.ShowDialog() == DialogResult.OK;
        }
    }
}

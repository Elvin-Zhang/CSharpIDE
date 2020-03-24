using CSharpIDE.Models;
using CSharpIDE.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpIDE.Views
{
    public partial class NewProjectWindow : Form, INewProjectWindow
    {
        public string ProjectName { get => ProjectNameTxtBox.Text; }
        public string ProjectPath { get => ProjectPathTxtBox.Text; set => ProjectPathTxtBox.Text = value; }

        public NewProjectWindow()
        {
            InitializeComponent();
            OKButton.Enabled = false;
        }

        public event EventHandler ChooseFolder;

        bool IShow.ShowDialog()
        {
            return this.ShowDialog() == DialogResult.OK;
        }

        private void ProjectNameTxtBox_TextChanged(object sender, EventArgs e)
        {
            if(ProjectNameTxtBox.Text.Length>0 && ProjectPathTxtBox.Text.Length>0)
            {
                OKButton.Enabled = true;
            }
            else
            {
                OKButton.Enabled = false;
            }
        }

        private void ChooseFolderButton_Click(object sender, EventArgs e)
        {
            ChooseFolder.Invoke(ProjectPathTxtBox, e);
        }

        private void ProjectPathTxtBox_TextChanged(object sender, EventArgs e)
        {
            ProjectNameTxtBox_TextChanged(sender, e);
        }
    }
}

using CSharpIDE.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpIDE.Presenters
{
    public class NewProjectPresenter
    {
        public INewProjectWindow NewProjectWindow { get; set; }

        public NewProjectPresenter(INewProjectWindow new_Project)
        {
            NewProjectWindow = new_Project;
            subscribe();
        }

        private void subscribe()
        {
            NewProjectWindow.ChooseFolder += ChooseFolderPR;
        }

        public void ChooseFolderPR(object txtFolder, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    NewProjectWindow.ProjectPath = fbd.SelectedPath;
                    (txtFolder as TextBox).Text = NewProjectWindow.ProjectPath;
                }
            }
        }
    }
}

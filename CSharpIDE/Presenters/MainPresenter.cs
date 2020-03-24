using CSharpIDE.Services;
using CSharpIDE.Services.Interfaces;
using CSharpIDE.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpIDE.Presenters
{
    public class MainPresenter
    {
        public IMainWindow MainWindow { get; set; }
        public IMainServices MainServices { get; set; }
        public MainPresenter(IMainWindow mainWindow, IMainServices mainServices)
        {
            MainWindow = mainWindow;
            MainServices = mainServices;
            subscribe();
        }

        private void subscribe()
        {
            MainWindow.NewProjectEvent += MainWindow_NewProjectEvent;
            MainWindow.OpenProjectEvent += MainWindow_OpenProjectEvent;
            MainWindow.NewFileEvent += MainWindow_NewFileEvent;
            MainWindow.OpenFileEvent += MainWindow_OpenFileEvent;
            MainWindow.SaveFocusedFile += MainWindow_SaveFocusedFile;
            MainWindow.SaveAllFiles += MainWindow_SaveAllFiles;
            MainWindow.ExcludeFile += MainWindow_ExcludeFile;
            MainWindow.RemoveFile += MainWindow_RemoveFile;
            MainWindow.BuildProject += MainWindow_BuildProject;
            MainWindow.RunProject += MainWindow_RunProject;
        }

        private void MainWindow_RunProject(object sender, EventArgs e)
        {
            MainServices.RunProject();
            MainWindow.Results = MainServices.Results;

        }

        private void MainWindow_BuildProject(object sender, EventArgs e)
        {
            MainServices.BuildProject();
        }

        private void MainWindow_RemoveFile(object sender, EventArgs e)
        {
            MainServices.RemoveFile(MainWindow.SelectedFileName);
            MainWindow.Project = MainServices.Project;
        }

        private void MainWindow_ExcludeFile(object sender, EventArgs e)
        {
            MainServices.ExcludeFile(MainWindow.SelectedFileName);
            MainWindow.Project = MainServices.Project;
        }

        private void MainWindow_SaveAllFiles(object sender, EventArgs e)
        {
            MainServices.Project = MainWindow.Project;
            MainServices.WriteFiles();
        }

        private void MainWindow_OpenFileEvent(object sender, EventArgs e)
        {
            MainServices.OpenFile(MainWindow.OFDFileName, MainWindow.OFDPath);
            MainWindow.Project = MainServices.Project;
        }

        private void MainWindow_SaveFocusedFile(object sender, EventArgs e)
        {
            MainServices.SaveFile(MainWindow.ProjectFile);
            MainWindow.Project = MainServices.Project;
        }

        private void MainWindow_NewProjectEvent(object sender, EventArgs e)
        {
            var newProjectPresenter = IOC.Reference.Resolve<NewProjectPresenter>();

            if (newProjectPresenter.NewProjectWindow.ShowDialog())
            {
                MainServices.Project.Name = newProjectPresenter.NewProjectWindow.ProjectName;
                MainServices.Project.Path = newProjectPresenter.NewProjectWindow.ProjectPath;
                MainWindow.Project = MainServices.Project;
                MainServices.CreateNewProject();
            }
        }

        private void MainWindow_OpenProjectEvent(object sender, EventArgs e)
        {
            MainServices.LoadProject(MainWindow.OFDFileName, MainWindow.OFDPath);
            MainWindow.Project = MainServices.Project;
        }

        private void MainWindow_NewFileEvent(object sender, EventArgs e)
        {
            MainServices.AddFileToProject(MainWindow.ProjectFile);
            MainWindow.Project = MainServices.Project;
        }
    }
}

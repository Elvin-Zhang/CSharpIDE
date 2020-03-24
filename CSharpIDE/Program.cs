using CSharpIDE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharpIDE.Models;
using CSharpIDE.Presenters;
using CSharpIDE.Services;
using CSharpIDE.Services.Interfaces;
using CSharpIDE.Views.Interfaces;


namespace CSharpIDE
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IOC.Reference.Register<MainWindow, IMainWindow>()
                .Register<NewProjectWindow, INewProjectWindow>()
                .Register<MainServices, IMainServices>()
                .Register<NewProjectPresenter>()
                .Register<MainPresenter>()
                .Register<Project>()
                .Register<ProjectFile>()
                .Build();

            MainPresenter mainPresenter = IOC.Reference.Resolve<MainPresenter>();

            Application.Run(mainPresenter.MainWindow as MainWindow);
        }
    }
}

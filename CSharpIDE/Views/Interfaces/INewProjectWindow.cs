using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpIDE.Views.Interfaces
{
    public interface INewProjectWindow : IShow
    {
        event EventHandler ChooseFolder;
        string ProjectName { get; }
        string ProjectPath { get; set; }
    }
}

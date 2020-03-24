using CSharpIDE.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpIDE.Services.Interfaces
{
    public interface IMainServices
    {
        Project Project { get; set; }
        CompilerResults Results { get; set; }

        void AddFileToProject(ProjectFile file);
        void BuildProject();
        void LoadProject(string ofdFileName, string ofdPath);
        void RunProject();
        void SaveFile(ProjectFile file);
        void WriteFile(ProjectFile file);
        void CreateNewProject();
        void OpenFile(string ofdFileName, string ofdPath);
        void WriteFiles();
        void ExcludeFile(string file);
        void RemoveFile(string file);
        CompilerResults CompilerResults(string[] sources, string output, params string[] references);
    }
}

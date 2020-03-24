using CSharpIDE.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpIDE.Views.Interfaces
{
    public interface IMainWindow : IShow
    {
        event EventHandler NewProjectEvent;
        event EventHandler OpenProjectEvent;
        event EventHandler NewFileEvent;
        event EventHandler OpenFileEvent;
        event EventHandler SaveFocusedFile;
        event EventHandler SaveAllFiles;
        event EventHandler BuildProject;
        event EventHandler RunProject;
        event EventHandler RemoveFile;
        event EventHandler ExcludeFile;

        CompilerResults Results { get; set; }
        Project Project { get; set; }
        string OFDFileName { get; set; }
        string OFDPath { get; set; }
        string SelectedFileName { get; set; }
        ProjectFile ProjectFile { get; set; }
    }
}

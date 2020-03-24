using CSharpIDE.Models;
using CSharpIDE.Services.Interfaces;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CSharpIDE.Services
{
    public class MainServices : IMainServices
    {
        public Project Project { get; set; }
        public CompilerResults Results { get; set; }

        public MainServices()
        {
            Project = new Project();
        }
        public void AddFileToProject(ProjectFile file)
        {
            WriteFile(file);
            Project.Files.Add(file);
            RefreshMysln();
        }

        public void BuildProject()
        {
            if (Project.Name != null && Project.Name != string.Empty)
            {
                string[] sources = new string[Project.Files.Count];
                for (int i = 0; i < Project.Files.Count; i++)
                {
                    sources[i] = Project.Files[i].Data;
                }
                Results = CompilerResults(sources, "compile.exe");
            }
        }

        public CompilerResults CompilerResults(string[] sources, string output, params string[] references)
        {
            var parameters = new CompilerParameters(references, output);
            parameters.GenerateExecutable = true;
            using (var provider = new CSharpCodeProvider())
                return provider.CompileAssemblyFromSource(parameters, sources);
        }

        public void LoadProject(string ofdFileName, string ofdPath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Project));
            using (TextReader textReader = new StreamReader($@"{ofdPath}\\{ofdFileName}.mysln"))
                Project = (xmlSerializer.Deserialize(textReader) as Project);
            Project.Path = ofdPath;
            GetData();
        }

        private void GetData()
        {
            if (Project.Path != null)
            {
                foreach (var item in Project.Files)
                {
                    using (TextReader textReader = new StreamReader($@"{Project.Path}\\{item.Name}"))
                        item.Data = textReader.ReadToEnd();
                }
            }
        }

        public void RunProject()
        {
            if (Project.Name != null && Project.Name != string.Empty)
            {
                BuildProject();
                if (Results.Errors.Count != 0)
                    MessageBox.Show("Please Check Code!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    Process.Start("compile.exe");
            }
        }

        public void SaveFile(ProjectFile file)
        {
            WriteFile(file);
            foreach (var item in Project.Files)
            {
                if(item.Name == file.Name)
                {
                    item.Data = file.Data;
                }
            }
            RefreshMysln();
        }

        public void WriteFile(ProjectFile file)
        {
            using (TextWriter textWriter = new StreamWriter($@"{Project.Path}\\{file.Name}"))
            {
                textWriter.WriteLine(file.Data);
            }
                
        }

        public void CreateNewProject()
        {
            Project.Files.Clear();
            Directory.CreateDirectory($@"{Project.Path}\\{Project.Name}");
            Project.Path += "\\" + Project.Name;
            string textcode;
            using (TextReader textReader = new StreamReader($@"..\..\..\FilesForProject\MainFile.cs"))
                textcode = textReader.ReadToEnd();
            AddFileToProject(new ProjectFile() { Name = "Program.cs", Data = textcode });
        }

        private void RefreshMysln()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Project));
            using (TextWriter textWriter = new StreamWriter($@"{Project.Path}\\{Project.Name}.mysln"))
                xmlSerializer.Serialize(textWriter, Project);
        }

        public void OpenFile(string ofdFileName, string ofdPath)
        {
            string textcode;
            using (TextReader textReader = new StreamReader($@"{ofdPath}\\{ofdFileName}.cs"))
                textcode = textReader.ReadToEnd();
            ProjectFile projectFile = new ProjectFile() { Name = ofdFileName+".cs", Data = textcode };
            AddFileToProject(projectFile);
        }

        public void WriteFiles()
        {
            foreach(ProjectFile item in Project.Files)
            {
                WriteFile(item);
            }
        }

        public void ExcludeFile(string file)
        {
            for (int i = 0; i < Project.Files.Count; i++)
            {
                if(Project.Files[i].Name == file)
                {
                    Project.Files.RemoveAt(i);
                }
            }
            RefreshMysln();
        }

        public void RemoveFile(string file)
        {
            ExcludeFile(file);
            if (File.Exists(Path.Combine(Project.Path, file)))
            {   
                File.Delete(Path.Combine(Project.Path, file));
            }
        }
    }
}

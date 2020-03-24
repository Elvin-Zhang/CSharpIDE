using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpIDE.Models
{
    public class Project
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<ProjectFile> Files { get; set; }

        public Project()
        {
            Files = new List<ProjectFile>();
        }
    }
}

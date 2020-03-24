using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSharpIDE.Models
{
    public class ProjectFile
    {
        public string Name { get; set; }

        [XmlIgnore]
        public string Data { get; set; }
    }
}

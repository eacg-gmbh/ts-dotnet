using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_NetFramework_Scanner.Common
{
    public class Target
    {
        public string project { get; set; }

        public string module { get; set; }

        public string moduleId { get; set; }

        public string release { get; set; }

        public string branch { get; set; }

        public string tag { get; set; }

        public List<Dependency> dependencies { get; set; } = new List<Dependency>();
    }

    public class Dependency
    {
        public string name { get; set; }

        public string key { get; set; }

        public List<string> versions { get; set; } = new List<string>();

        //public string description { get; set; }

        //public bool Private {get; set;}

        //public List<licence> licences { get; set; } = new List<licence>();

        //public string homepageUrl { get; set; }

        //public string repoUrl { get; set; }

        //public string checksum { get; set; }

        public List<Dependency> dependencies { get; set; } = new List<Dependency>();
    }

    public class license
    {
        public string name { get; set; }

        public string url { get; set; }
    }
}

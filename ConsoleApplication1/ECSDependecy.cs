using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class ECSDependecy
    {
        public String name { get; set; }
        public String key { get; set; }
        public List<String> versions { get; set; }
        public List<ECSDependecy> dependencies { get; set; }
        public String description { get; set; }
        public String homepageUrl { get; set; }
        public String repoUrl { get; set; }
        public List<License> licenses { get; set; }
        public String checksum { get; set; }

    }

    public class ECSArtifact
    {
        public String project { get; set; }
        public String module { get; set; }
        public String moduleId { get; set; }
        public List<ECSDependecy> dependencies { get; set; }
    }

    public class License
    {
        public String name { get; set; }
        public String url { get; set; }
    }
}

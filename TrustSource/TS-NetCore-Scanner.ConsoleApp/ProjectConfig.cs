using System;
using System.Collections.Generic;
using System.Text;

namespace TS_NetCore_Scanner.ConsoleApp
{
    internal class ProjectConfig
    {
        public ProjectConfig() { trustSourceAPI = new TrustSourceAPI(); }

        public string ProjectPath { get; set; }

        public TrustSourceAPI trustSourceAPI { get; set; }
    }

    internal class TrustSourceAPI
    {
        public string ApiUrl { get; set; }

        public string Username { get; set; }

        public string ApiKey { get; set; }
    }
}

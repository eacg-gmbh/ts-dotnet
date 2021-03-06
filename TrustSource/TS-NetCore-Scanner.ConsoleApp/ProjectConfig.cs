﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TS_NetCore_Scanner.ConsoleApp
{
    internal class ProjectConfig
    {
        public ProjectConfig() { trustSourceAPI = new TrustSourceAPI(); }

        public string Branch { get; set; }

        public string Tag { get; set; }

        public TrustSourceAPI trustSourceAPI { get; set; }
    }

    internal class TrustSourceAPI
    {
        public string ApiUrl { get; set; }

        public string ApiKey { get; set; }
    }
}

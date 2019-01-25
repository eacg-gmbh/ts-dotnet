using System;
using System.Collections.Generic;
using System.Text;

namespace TS_NetCore_Scanner.Engine
{
    public class MetaPackagesSkipper
    {
        public static List<string> MetaPackages { get; set; } = new List<string>()
        {
            "Microsoft.NETCore.App",
            "Microsoft.AspNetCore.App",
            "Microsoft.AspNetCore.All",
            "Microsoft.NETCore.Portable.Compatibility "
        };
    }
}

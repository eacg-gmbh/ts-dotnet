using System;
using System.Collections.Generic;
using System.Text;

namespace TS_Net_Scanner.Engine
{
    public class MetaPackagesSkipper
    {
        public static List<string> MetaPackages { get; set; } = new List<string>()
        {
            "Microsoft.NETCore.App",
            "NETStandard.Library",
            "Microsoft.NETCore.Platforms",
            "Microsoft.AspNetCore.App",
            "Microsoft.AspNetCore.All",
            "Microsoft.NETCore.Portable.Compatibility",
            "System.Collections",
            "System.Collections.NonGeneric",
            "System.Collections.Specialized",
            "System.Collections.Concurrent",
            "System.Reflection",
            "System.Resources.ResourceManager",
            "System.Runtime",
            "System.Runtime.Handles",            
            "System.Runtime.Extensions",
            "System.Globalization",
            "System.Globalization.Calendars",
            "System.IO",
            "System.IO.FileSystem",
            "System.Linq",
            "System.Linq.Queryable",
            "System.ObjectModel",
            "System.Reflection.Emit",
            "System.Runtime.Serialization.Formatters",
            "System.Linq.Expressions",
            "System.Reflection.Emit.ILGeneration",
            "System.Reflection.Emit.Lightweight",
            "System.Reflection.Extensions",
            "System.Reflection.Primitives",
            "System.Reflection.TypeExtensions",
            "System.Threading",
            "System.Threading.Tasks",
            "System.Runtime.Serialization.Xml",
            "System.Net.NameResolution",
            "System.Data.SqlClient",
            "System.Data.Common",
            "System.ComponentModel.TypeConverter",
            "System.Configuration.ConfigurationManager",
            "System.IdentityModel.Tokens.Jwt",
            "System.Runtime.InteropServices",
            "Microsoft.IdentityModel.Logging",
            "Microsoft.IdentityModel.Tokens",
            "System.IdentityModel.Tokens.Jwt",
            "System.Diagnostics.Tracing",
            "System.Diagnostics.Debug",
            "System.Security.Cryptography.X509Certificates",
            "System.Xml.XmlSerializer",
            "System.Security.Cryptography.Algorithms",
            "System.Security.Cryptography.Encoding",
            "System.Security.Cryptography.Cng",
            "System.Security.Cryptography.Csp",
            "System.Security.Cryptography.OpenSsl",
            "System.Security.Cryptography.Primitives"
        };
    }
}

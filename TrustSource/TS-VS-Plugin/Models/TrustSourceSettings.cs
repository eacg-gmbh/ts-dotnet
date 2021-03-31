using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustSource;
using Microsoft.VisualStudio.Shell;

namespace TrustSource.Models
{
    public class TrustSourceSettings
    {
        public string ApiKey { get; set; }

        public bool AskOptional { get; set; }
    }
}

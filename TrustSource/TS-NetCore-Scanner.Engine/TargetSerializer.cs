using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TS_NetCore_Scanner.Engine
{
    internal class TargetSerializer
    {
        public static string ConvertToJson(Target target)
        {
            try
            {
                string output = JsonConvert.SerializeObject(target);
                return output;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using TS_Net_Scanner.Common;

namespace TS_Net_Scanner.Engine
{
    public class TargetSerializer
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

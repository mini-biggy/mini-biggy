using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MiniBiggy.Serializers
{
    public class PrettyJsonSerializer : JsonSerializer
    {
        public override byte[] Serialize<T>(List<T> list)
        {
            var json = JsonConvert.SerializeObject(list,
                Formatting.Indented, Settings);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
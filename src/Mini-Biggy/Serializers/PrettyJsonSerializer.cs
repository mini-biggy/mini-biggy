using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MiniBiggy.Serializers {
    public class PrettyJsonSerializer : JsonSerializer {

        public override byte[] Serialize<T>(List<T> list) {
            var json = JsonConvert.SerializeObject(list, 
                Formatting.Indented, new JsonSerializerSettings {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
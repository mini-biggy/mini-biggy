using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MiniBiggy.Serializers {
    public class JsonSerializer : ISerializer {
        public List<T> Deserialize<T>(byte[] bytes) {
            var list = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return JsonConvert.DeserializeObject<List<T>>(list);
        }

        public virtual byte[] Serialize<T>(List<T> list) where T : new() {
            var json = JsonConvert.SerializeObject(list);
            return Encoding.UTF8.GetBytes(json);
        }
    }

}
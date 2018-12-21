﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MiniBiggy.Serializers
{
    public class JsonSerializer : ISerializer
    {
        internal JsonSerializerSettings Settings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                };
            }
        }

        public List<T> Deserialize<T>(byte[] bytes)
        {
            var list = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return JsonConvert.DeserializeObject<List<T>>(list, Settings);
        }

        public virtual byte[] Serialize<T>(List<T> list) where T : new()
        {
            var json = JsonConvert.SerializeObject(list, Settings);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
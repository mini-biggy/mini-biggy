using System.Collections.Generic;

namespace MiniBiggy.Serializers {
    public interface ISerializer {
        byte[] Serialize<T>(List<T> list) where T : new();
        List<T> Deserialize<T>(byte[] list);
    }
}
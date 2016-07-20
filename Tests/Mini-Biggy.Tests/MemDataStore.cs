using System;
using System.Threading.Tasks;

namespace MiniBiggy.Tests {
    public class MemDataStore : IDataStore {

        public byte[] Json { get; set; }

        public MemDataStore() {
            Json = new byte[0];
        }


        public async Task<byte[]> ReadAllAsync() {
            return await Task.FromResult(Json);
        }

        public async Task WriteAllAsync(byte[] json) {
            Json = json;
        }
    }
}
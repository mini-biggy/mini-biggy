using System.Threading.Tasks;

namespace MiniBiggy.Tests {
    public class MemDataStore : IDataStore {

        public string Json { get; set; }

        public MemDataStore() {
            Json = "";
        }

        public Task<string> ReadAllTextAsync() {
            return Task.FromResult(Json);
        }

        public async Task WriteAllTextAsync(string json) {
            Json = json;
        }
    }
}
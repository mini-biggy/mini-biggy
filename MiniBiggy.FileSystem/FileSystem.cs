using System.IO;
using System.Threading.Tasks;

namespace MiniBiggy.FileSystem {
    public class FileSystem : IDataStore {
        public async Task<string> ReadAllTextAsync(string listName) {
            return await Task.Run(() => {
                if (!File.Exists(listName)) {
                    return "";
                }
                return File.ReadAllText(listName);
            });
        }

        public Task WriteAllTextAsync(string listName, string json) {
            return Task.Run(() => {
                File.WriteAllText(listName, json);
            });
        }
    }
}
using System.IO;
using System.Threading.Tasks;

namespace MiniBiggy.FileSystem {
    public class FileSystem : IDataStore {

        protected virtual string GetListFullPath(string listName) {
            return listName + ".js";
        }

        public async Task<string> ReadAllTextAsync(string listName) {
            return await Task.Run(() => {
                var listPath = GetListFullPath(listName);
                if (!File.Exists(listPath)) {
                    return "";
                }
                return File.ReadAllText(listPath);
            });
        }

        public Task WriteAllTextAsync(string listName, string json) {
            return Task.Run(() => {
                File.WriteAllText(GetListFullPath(listName), json);
            });
        }
    }
}
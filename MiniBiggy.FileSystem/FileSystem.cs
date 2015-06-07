using System.IO;
using System.Threading.Tasks;

namespace MiniBiggy.FileSystem {
    public class FileSystem : IDataStore {

        public string BasePath

        public FileSystem(string path) {
            
        }

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

        public async Task WriteAllTextAsync(string listName, string json) {
            await Task.Run(() => {
                File.WriteAllText(GetListFullPath(listName), json);
            });
        }
    }
}
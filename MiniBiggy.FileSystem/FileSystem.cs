using System.IO;
using System.Threading.Tasks;

namespace MiniBiggy.FileSystem {
    public class FileSystem : IDataStore {
        private readonly string _basePath = "";
        private const string FileExtension = ".jss";

        public FileSystem() {
            
        }

        public FileSystem(string basePath) {
            _basePath = basePath;
        }

        protected virtual string GetListFullPath(string listName) {
            return Path.Combine(_basePath, listName + FileExtension);
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
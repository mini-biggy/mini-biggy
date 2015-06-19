using System.IO;
using System.Text;
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
            var listPath = GetListFullPath(listName);
            if (!File.Exists(listPath)) {
                return "";
            }
            return await Task.Run(() => File.ReadAllText(listPath));
        }

        public async Task WriteAllTextAsync(string listName, string json) {
            var bytes = Encoding.UTF8.GetBytes(json);
            var path = GetListFullPath(listName);
            await Try.ThreeTimes(async () => {
                File.Delete(path);
                using (var fs = new FileStream(GetListFullPath(listName), FileMode.OpenOrCreate, FileAccess.Write)) {
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                }
            });
        }
    }
}
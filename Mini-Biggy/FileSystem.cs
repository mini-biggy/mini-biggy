using MiniBiggy.Util;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiggy.FileSystem {
    public class FileSystem : IDataStore {
        private readonly string _fullPath = "";
        public FileSystem(string fullPath) {
            _fullPath = fullPath;
        }
        
        public async Task<string> ReadAllTextAsync() {
            if (!File.Exists(_fullPath)) {
                return "";
            }
            return await Task.Run(() => File.ReadAllText(_fullPath));
        }

        public async Task WriteAllTextAsync(string json) {
            var bytes = Encoding.UTF8.GetBytes(json);
            var directory = Path.GetDirectoryName(_fullPath);
            if (directory != "") {
                Directory.CreateDirectory(directory);
            }
            await Try.ThreeTimes(async () => {
                File.Delete(_fullPath);
                using (var fs = new FileStream(_fullPath, FileMode.OpenOrCreate, FileAccess.Write)) {
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                }
            });
        }
    }
}
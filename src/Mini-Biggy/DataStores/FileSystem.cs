using MiniBiggy.Util;
using System.IO;
using System.Threading.Tasks;

namespace MiniBiggy.DataStores {
    public class FileSystem : IDataStore {
        private readonly string _fullPath = "";
        public FileSystem(string fullPath) {
            _fullPath = fullPath;
        }
        
        public async Task<byte[]> ReadAllAsync() {
            if (!File.Exists(_fullPath)) {
                return new byte[0];
            }
            return await Task.Run(() => File.ReadAllBytes(_fullPath));
        }

        public async Task WriteAllAsync(byte[] bytes) {
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
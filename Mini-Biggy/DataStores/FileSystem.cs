using System;
using MiniBiggy.Util;
using System.IO;
using System.Threading.Tasks;

namespace MiniBiggy.DataStores {
    public class FileSystem : IDataStore {
        public string FullPath { get; }
        public FileSystem(string fullPath) {
            FullPath = fullPath;
        }
        
        public async Task<byte[]> ReadAllAsync() {
            if (!File.Exists(FullPath)) {
                return new byte[0];
            }
            return await Try.ThreeTimesAsync(() => File.ReadAllBytes(FullPath));
        }

        public async Task WriteAllAsync(byte[] bytes) {
            await WriteAllAsync(bytes, FullPath);
        }

        public async Task WriteAllAsync(byte[] bytes, string path) {
            var directory = Path.GetDirectoryName(path);
            if (directory != "") {
                Directory.CreateDirectory(directory);
            }
            await Try.ThreeTimesAsync(async () => {
                File.Delete(path + ".old");
                if (File.Exists(path)) {
                    File.Move(path, path + ".old");
                }
                using (var fs = new FileStream(FullPath, FileMode.OpenOrCreate, FileAccess.Write)) {
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                }
            }, 300);
        }
    }
}
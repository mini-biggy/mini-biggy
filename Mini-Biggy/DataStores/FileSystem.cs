using MiniBiggy.Util;
using System.IO;
using System.Threading.Tasks;

namespace MiniBiggy.DataStores
{
    public class FileSystem : IDataStore
    {
        private static readonly object SyncRoot = new object();

        public string FullPath { get; }

        public FileSystem(string fullPath)
        {
            FullPath = fullPath;
        }

        public virtual async Task<byte[]> ReadAllAsync()
        {
            if (!File.Exists(FullPath))
            {
                return new byte[0];
            }
            return await Try.ThreeTimesAsync(() => File.ReadAllBytes(FullPath));
        }

        public virtual async Task WriteAllAsync(byte[] bytes)
        {
            await WriteAllAsync(bytes, FullPath);
        }

        public virtual async Task WriteAllAsync(byte[] bytes, string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (directory != "")
            {
                Directory.CreateDirectory(directory);
            }
            await Try.ThreeTimesAsync(async () =>
            {
                lock (SyncRoot)
                {
                    if (File.Exists(path))
                    {
                        var old = $"{path}.old";
                        File.Delete(old);
                        File.Move(path, old);
                    }
                    using (var fs = new FileStream(FullPath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        fs.WriteAsync(bytes, 0, bytes.Length);
                    }
                }
                await Task.Delay(0);
            }, 300);
        }
    }
}
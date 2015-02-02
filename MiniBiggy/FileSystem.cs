using System.IO;

namespace MiniBiggy {
    public class FileSystem : IFilesystem {
        public bool Exists(string filename) {
            return File.Exists(filename);
        }

        public string ReadAllText(string filename) {
            return File.ReadAllText(filename);
        }

        public void WriteAllText(string filename, string json) {
            File.WriteAllText(filename, json);
        }
    }
}
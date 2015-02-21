using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MiniBiggy.UniversalApps {
    public class Storage : IDataStore {
        public async Task<string> ReadAllTextAsync(string listName) {
            try {
                var local = ApplicationData.Current.LocalFolder;
                Stream stream = await local.OpenStreamForReadAsync(listName);
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8)) {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (FileNotFoundException) {
                return "";
            }
        }

        public async Task WriteAllTextAsync(string listName, string json) {
            byte[] fileBytes = Encoding.UTF8.GetBytes(json.ToCharArray());
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(listName, CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync()) {
                stream.Write(fileBytes, 0, fileBytes.Length);
            }
        }
    }
}
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using System.Diagnostics;
using MiniBiggy.DataStores;
using System.Text;

namespace MiniBiggy.Tests {
    public class PersistentListIntegration {
        
        [Test]
        public async Task Shoud_save_on_desired_path() {
            var file = Path.Combine(Path.GetTempPath(), "foo.jss");
            File.Delete(file);
            Directory.CreateDirectory(Path.GetDirectoryName(file));

            var fs = new FileSystem(file);
            await fs.WriteAllAsync(Encoding.UTF8.GetBytes("json"));
            Assert.IsTrue(File.Exists(file));

            File.Delete(file);
            Directory.CreateDirectory(Path.GetDirectoryName(file));
        }
    }
}

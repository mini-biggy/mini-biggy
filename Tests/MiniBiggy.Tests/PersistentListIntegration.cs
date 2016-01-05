using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace MiniBiggy.Tests {
    public class PersistentListIntegration {

        [Test]
        public async Task Should_save_same_path() {
            File.Delete("foo.jss");
            var fs = new FileSystem.FileSystem();
            await fs.WriteAllTextAsync("foo", "json");
            Assert.IsTrue(File.Exists("foo.jss"));
        }

        [Test]
        public async Task Shoud_save_on_desired_path() {
            string file = "c:\\temp\\foo.jss";
            File.Delete(file);
            Directory.CreateDirectory(Path.GetDirectoryName(file));

            var fs = new FileSystem.FileSystem("c:\\temp");
            await fs.WriteAllTextAsync("foo", "json");
            Assert.IsTrue(File.Exists(file));

            File.Delete(file);
            Directory.CreateDirectory(Path.GetDirectoryName(file));
        }
    }
}

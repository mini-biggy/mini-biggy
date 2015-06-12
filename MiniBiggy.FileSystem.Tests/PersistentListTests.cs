using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MiniBiggy.FileSystem.Tests {
    public class PersistentListTests {
        [Test]
        public async Task Should_save_same_path() {
            File.Delete("foo.jss");
            var fs = new FileSystem();
            await fs.WriteAllTextAsync("foo", "json");
            Assert.IsTrue(File.Exists("foo.jss"));
        }

        [Test]
        public async Task Shoud_save_on_desired_path() {
            string file = "c:\\temp\\foo.jss";
            File.Delete(file);
            Directory.CreateDirectory(Path.GetDirectoryName(file));

            var fs = new FileSystem("c:\\temp");
            await fs.WriteAllTextAsync("foo", "json");
            Assert.IsTrue(File.Exists(file));

            File.Delete(file);
            Directory.CreateDirectory(Path.GetDirectoryName(file));
        }
    }
}

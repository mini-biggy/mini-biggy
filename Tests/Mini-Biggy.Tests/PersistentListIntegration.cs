using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using System.Diagnostics;
using MiniBiggy.DataStores;
using System.Text;
using MiniBiggy.SaveStrategies;
using MiniBiggy.Serializers;

namespace MiniBiggy.Tests {
    public class PersistentListIntegration {

        private PersistentList<Tweet> _list;
        private FileSystem _store;
        private string _file;

        [SetUp]
        public void SetUp() {
            _file = Path.Combine(Path.GetTempPath(), "foo.jss");
            File.Delete(_file);
            Directory.CreateDirectory(Path.GetDirectoryName(_file));
            _list = new PersistentList<Tweet>(
                _store = new FileSystem(_file), 
                new PrettyJsonSerializer(),
                new SaveOnlyWhenRequested());
        }

        [Test]
        public async Task Shoud_save_on_desired_path() {
            var fs = new FileSystem(_file);
            await fs.WriteAllAsync(Encoding.UTF8.GetBytes("json"));
            Assert.IsTrue(File.Exists(_file));
        }


        [Test]
        public void Should_throw_when_saving_is_not_possible() {
            var tweet = new Tweet { Username = "Foo" };
            _list.Add(tweet);
            _list.Save();
            FileStream fs = null;
            try {
                fs = File.Open(_file, FileMode.Open, FileAccess.Read, FileShare.None);
                _list.Add(tweet);
                _list.Save();
                Assert.Fail("Should throw exception!");
            }
            catch (Exception) {}
            finally {
                fs?.Dispose();
            }
        }

        [TearDown]
        public void TearDown() {
            File.Delete(_file);
            Directory.CreateDirectory(Path.GetDirectoryName(_file));
        }
    }
}

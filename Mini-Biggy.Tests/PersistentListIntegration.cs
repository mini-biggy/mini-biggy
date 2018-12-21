using MiniBiggy.DataStores;
using MiniBiggy.SaveStrategies;
using MiniBiggy.Serializers;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MiniBiggy.Tests
{
    public class PersistentListIntegration : IDisposable
    {
        private PersistentList<Tweet> _list;
        private FileSystem _store;
        private string _file;

        public PersistentListIntegration()
        {
            _file = Path.Combine(Path.GetTempPath(), "foo.jss");
            File.Delete(_file);
            Directory.CreateDirectory(Path.GetDirectoryName(_file));
            _list = new PersistentList<Tweet>(
                _store = new FileSystem(_file),
                new PrettyJsonSerializer(),
                new SaveOnlyWhenRequested());
        }

        public void Dispose()
        {
            File.Delete(_file);
            Directory.CreateDirectory(Path.GetDirectoryName(_file));
        }

        [Fact]
        public async Task Shoud_save_on_desired_path()
        {
            var fs = new FileSystem(_file);
            await fs.WriteAllAsync(Encoding.UTF8.GetBytes("json"));
            Assert.True(File.Exists(_file));
        }

        [Fact]
        public void Should_throw_when_saving_is_not_possible()
        {
            var tweet = new Tweet { Username = "Foo" };
            _list.Add(tweet);
            _list.Save();
            FileStream fs = null;
            try
            {
                fs = File.Open(_file, FileMode.Open, FileAccess.Read, FileShare.None);
                _list.Add(tweet);
                _list.Save();
                Assert.True(false, "Should throw exception!");
            }
            catch (Exception) { }
            finally
            {
                fs?.Dispose();
            }
        }
    }
}
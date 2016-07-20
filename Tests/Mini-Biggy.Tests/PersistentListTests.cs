using MiniBiggy.SaveStrategies;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Text;
using System.Linq;

namespace MiniBiggy.Tests {
    public class PersistentListTests {
        private PersistentList<Tweet> _list;
        private MemDataStore _store;

        [SetUp]
        public void SetUp() {
            _list = new PersistentList<Tweet>(_store = new MemDataStore(), null, null);
        }

        [Test]
        public async Task Should_save_async() {
            _list.Add(new Tweet());
            await _list.SaveAsync();
            Assert.AreEqual("[{\"$id\":\"1\",\"Username\":null,\"Message\":null,\"DateTime\":\"0001-01-01T00:00:00\"}]", Encoding.UTF8.GetString(_store.Json));
        }

        [Test]
        public void Should_save_sync() {
            _list.Add(new Tweet());
            _list.Save();
            Assert.AreEqual("[{\"$id\":\"1\",\"Username\":null,\"Message\":null,\"DateTime\":\"0001-01-01T00:00:00\"}]", Encoding.UTF8.GetString(_store.Json));
        }

        [Test]
        public void Objects_references_are_preserved() {
            var tweet = new Tweet { Username = "Foo" };
            _list.Add(tweet);
            _list.Add(tweet);
            _list.Save();

            _list = new PersistentList<Tweet>(_store, null, null);

            var t1 = _list.First();
            var t2 = _list.Last();

            Assert.AreSame(t1, t2);
        }
    }
}
using MiniBiggy.SaveStrategies;
using NUnit.Framework;

namespace MiniBiggy.Tests {
    public class PersistentListTests {
        private PersistentList<Tweet> _list;
        private MemDataStore _store;

        [SetUp]
        public void SetUp() {
            _list = new PersistentList<Tweet>(_store = new MemDataStore(), null);
        }

        [Test]
        public void Default_strategy_is_SaveOnlyWhenRequested() {
            Assert.IsInstanceOf<SaveOnlyWhenRequested>(_list.SaveStrategy);
        }

        [Test]
        public async void Should_save_async() {
            _list.Add(new Tweet());
            await _list.SaveAsync();
            Assert.AreEqual("[{\"Username\":null,\"Message\":null,\"DateTime\":\"0001-01-01T00:00:00\"}]", _store.Json);
        }

        [Test]
        public void Sould_save_sync() {
            _list.Add(new Tweet());
            _list.Save();
            Assert.AreEqual("[{\"Username\":null,\"Message\":null,\"DateTime\":\"0001-01-01T00:00:00\"}]", _store.Json);
        }
    }
}
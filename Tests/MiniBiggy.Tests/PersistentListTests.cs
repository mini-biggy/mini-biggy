using NUnit.Framework;

namespace MiniBiggy.Tests {
    public class PersistentListTests {
        private PersistentList<Tweet> _list;
        private MemDataStore _store;

        [SetUp]
        public void SetUp() {
            _list = new PersistentList<Tweet>(_store = new MemDataStore());
        }

        [Test]
        public async void Adding_item_saves_the_list() {
            _list.Add(new Tweet());
            await _list.SaveAsync();
            Assert.AreEqual("[{\"Username\":null,\"Message\":null,\"DateTime\":\"0001-01-01T00:00:00\"}]", _store.Json);
        }

        [Test]
        public void When_autosave_true__saves_automatically() {
            _list.AutoSave = true;
            _list.Add(new Tweet());
            Assert.AreEqual("[{\"Username\":null,\"Message\":null,\"DateTime\":\"0001-01-01T00:00:00\"}]", _store.Json);
        }
    }
}
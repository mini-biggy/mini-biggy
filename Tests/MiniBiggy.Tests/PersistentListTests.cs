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
        public void Adding_item_saves_the_list() {
            _list.Add(new Tweet());
            Assert.AreEqual("[{\"Username\":null,\"Message\":null,\"DateTime\":\"0001-01-01T00:00:00\"}]", _store.Json);
        }
    }
}
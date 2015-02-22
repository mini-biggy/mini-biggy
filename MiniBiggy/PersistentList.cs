using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MiniBiggy {

    public class PersistentList<T> : ICollection<T> where T : new() {

        private readonly List<T> _items;
        private readonly IDataStore _dataStore;

        public event EventHandler<PersistedEventArgs<T>> ItemsRemoved;
        public event EventHandler<PersistedEventArgs<T>> ItemsAdded;
        public event EventHandler<PersistedEventArgs<T>> ItemsUpdated;
        public event EventHandler<PersistedEventArgs<T>> ItemsChanged;
        public event EventHandler Loaded;
        public event EventHandler Saved;

        public bool AutoSave { get; set; }

        public PersistentList(IDataStore dataStore) {
            _dataStore = dataStore;
            _items = new List<T>();
            Load();
        }

        public PersistentList(IDataStore dataStore, IEnumerable<T> items)
            : this(dataStore) {
            Add(items);
        }

        private void Load() {
            var json = _dataStore.ReadAllTextAsync(Name).Result;
            if (String.IsNullOrEmpty(json)) {
                return;
            }
            _items.AddRange(JsonConvert.DeserializeObject<List<T>>(json));
        }
        
        public virtual string Name {
            get {
                return typeof(T).Name;
            }
        }

        public void Save() {
            SaveAsync().Wait();
        }

        public async Task SaveAsync() {
            var json = JsonConvert.SerializeObject(_items);
            await _dataStore.WriteAllTextAsync(Name, json);
        }

        public async virtual Task<int> UpdateAsync(T item) {
            var index = _items.IndexOf(item);
            if (index > -1) {
                _items.RemoveAt(index);
                _items.Insert(index, item);
            }
            if (AutoSave) {
                await SaveAsync();
            }
            OnItemsUpdated(new List<T> { item });
            OnItemsChanged(new List<T> { item });
            return 1;
        }

        public async virtual Task<int> UpdateAsync(IEnumerable<T> items) {
            var itemsToUpdate = items.ToList();
            foreach (var item in itemsToUpdate) {
                var index = _items.IndexOf(item);
                if (index > -1) {
                    _items.RemoveAt(index);
                    _items.Insert(index, item);
                }
            }
            if (AutoSave) {
                await SaveAsync();
            }
            OnItemsUpdated(itemsToUpdate);
            OnItemsChanged(itemsToUpdate);
            return itemsToUpdate.Count();
        }

        public virtual void Add(T item) {
            _items.Add(item);
            if (AutoSave) {
                SaveAsync().Wait();
            }
            OnItemsAdded(new List<T> { item });
            OnItemsChanged(new List<T> { item });
        }

        public void Add(IEnumerable<T> items) {
            var list = items.ToList();
            _items.AddRange(list);
            if (AutoSave) {
                SaveAsync().Wait();
            }
            OnItemsAdded(list);
            OnItemsChanged(list);
        }

        public virtual void Clear() {
            _items.Clear();
            if (AutoSave) {
                SaveAsync().Wait();
            }
            OnItemsChanged(new List<T>());
        }

        public virtual bool Contains(T item) {
            return _items.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex) {
            _items.CopyTo(array, arrayIndex);
        }

        public virtual int Count {
            get { return _items.Count; }
        }

        public virtual bool IsReadOnly {
            get { return false; }
        }

        public virtual bool Remove(T item) {
            var removed = _items.Remove(item);
            if (AutoSave) {
                SaveAsync().Wait();
            }
            OnItemsRemoved(new List<T> { item });
            OnItemsChanged(new List<T> { item });
            return removed;
        }

        public virtual int Remove(IEnumerable<T> items) {
            var itemsToRemove = items.ToList();
            var removedItems = new List<T>();
            foreach (var item in itemsToRemove) {
                if (_items.Remove(item)) {
                    removedItems.Add(item);
                }
            }
            if (AutoSave) {
                SaveAsync().Wait();
            }
            OnItemsRemoved(removedItems);
            OnItemsChanged(removedItems);
            return removedItems.Count();
        }

        public IEnumerator<T> GetEnumerator() {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _items.GetEnumerator();
        }

        protected virtual void OnItemsRemoved(List<T> items) {
            var handler = ItemsRemoved;
            if (handler != null) handler(this, new PersistedEventArgs<T>(items));
        }

        protected virtual void OnItemsUpdated(List<T> items) {
            var handler = ItemsUpdated;
            if (handler != null) handler(this, new PersistedEventArgs<T>(items));
        }

        protected virtual void OnItemsChanged(List<T> items) {
            var handler = ItemsChanged;
            if (handler != null) handler(this, new PersistedEventArgs<T>(items));
        }

        protected virtual void OnItemsAdded(List<T> items) {
            var handler = ItemsAdded;
            if (handler != null) handler(this, new PersistedEventArgs<T>(items));
        }

        protected virtual void OnLoaded() {
            var handler = Loaded;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnSaved() {
            var handler = Saved;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
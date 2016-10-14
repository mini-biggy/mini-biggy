using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniBiggy.SaveStrategies;
using MiniBiggy.Serializers;

namespace MiniBiggy {
    public class PersistentList<T> : ICollection<T> where T : new() {

        private readonly List<T> _items;
        private readonly IDataStore _dataStore;
        private readonly ISerializer _serializer;
        private readonly ISaveStrategy _saveStrategy;

        private static readonly object SyncRoot = new object();

        public event EventHandler<PersistedEventArgs<T>> ItemsRemoved;
        public event EventHandler<PersistedEventArgs<T>> ItemsAdded;
        public event EventHandler<PersistedEventArgs<T>> ItemsUpdated;
        public event EventHandler<PersistedEventArgs<T>> ItemsChanged;
        public event EventHandler Loaded;
        public event EventHandler<SavedEventArgs> Saved;

        public bool IsNew { get; set; }
        
        public PersistentList(IDataStore dataStore, ISerializer serializer, ISaveStrategy saveStrategy) {
            _dataStore = dataStore;
            _serializer = serializer;
            _items = new List<T>();
            _serializer = serializer ?? new JsonSerializer();
            _saveStrategy = saveStrategy ?? new SaveOnlyWhenRequested();
            _saveStrategy.NotifyUnsolicitedSave += (sender, args) => Save();
            Load();
        }

        private void Load() {
            var bytes = _dataStore.ReadAllAsync().Result;
            if (bytes.Length == 0) {
                IsNew = true;
                return;
            }
            _items.AddRange(_serializer.Deserialize<T>(bytes));
        }
        
        public virtual string Name => typeof(T).Name;

        public void Save() {
            SaveAsync().Wait();
        }

        public async Task SaveAsync() {
            try {
                byte[] bytes;
                lock (SyncRoot) {
                    bytes = _serializer.Serialize(_items);
                }
                await _dataStore.WriteAllAsync(bytes);
                OnSaved();
            }
            catch (Exception ex) {
                OnSaved(ex);
                throw;
            }
        }

        public virtual async Task<int> UpdateAsync(T item) {
            lock (SyncRoot) {
                var index = _items.IndexOf(item);
                if (index > -1) {
                    _items[index] = item;
                }
            }
            if (_saveStrategy.ShouldSaveNow()) {
                await SaveAsync();
            }
            OnItemsUpdated(new List<T> { item });
            OnItemsChanged(new List<T> { item });
            return 1;
        }

        public virtual async Task<int> UpdateAsync(IEnumerable<T> items) {
            var itemsToUpdate = items.ToList();
            lock (SyncRoot) {
                foreach (var item in itemsToUpdate) {
                    var index = _items.IndexOf(item);
                    if (index > -1) {
                        _items[index] = item;
                    }
                }
            }
            if (_saveStrategy.ShouldSaveNow()) {
                await SaveAsync();
            }
            OnItemsUpdated(itemsToUpdate);
            OnItemsChanged(itemsToUpdate);
            return itemsToUpdate.Count();
        }

        public virtual void Add(T item) {
            lock (SyncRoot) {
                _items.Add(item);
            }
            if (_saveStrategy.ShouldSaveNow()) {
                Save();
            }
            OnItemsAdded(new List<T> { item });
            OnItemsChanged(new List<T> { item });
        }

        public void Add(IEnumerable<T> items) {
            var list = items.ToList();
            lock (SyncRoot) {
                _items.AddRange(list);
            }
            if (_saveStrategy.ShouldSaveNow()) {
                Save();
            }
            OnItemsAdded(list);
            OnItemsChanged(list);
        }

        public virtual void Clear() {
            lock (SyncRoot) {
                _items.Clear();
            }
            if (_saveStrategy.ShouldSaveNow()) {
                Save();
            }
            OnItemsChanged(new List<T>());
        }

        public virtual bool Contains(T item) {
            lock (SyncRoot) {
                return _items.Contains(item);
            }
        }

        public virtual void CopyTo(T[] array, int arrayIndex) {
            lock (SyncRoot) {
                _items.CopyTo(array, arrayIndex);
            }
        }

        public virtual int Count => _items.Count;

        public virtual bool IsReadOnly => false;

        public virtual bool Remove(T item) {
            bool removed;
            lock (SyncRoot) {
                removed = _items.Remove(item);
            }
            if (_saveStrategy.ShouldSaveNow()) {
                Save();
            }
            OnItemsRemoved(new List<T> { item });
            OnItemsChanged(new List<T> { item });
            return removed;
        }

        public virtual int Remove(IEnumerable<T> items) {
            var itemsToRemove = items.ToList();
            var removedItems = new List<T>();
            lock (SyncRoot) {
                foreach (var item in itemsToRemove) {
                    if (_items.Remove(item)) {
                        removedItems.Add(item);
                    }
                }
            }
            if (_saveStrategy.ShouldSaveNow()) {
                Save();
            }
            OnItemsRemoved(removedItems);
            OnItemsChanged(removedItems);
            return removedItems.Count();
        }

        public IEnumerator<T> GetEnumerator() {
            lock (SyncRoot) {
                return _items.GetEnumerator();
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            lock (SyncRoot) {
                return _items.GetEnumerator();
            }
        }

        protected virtual void OnItemsRemoved(List<T> items) {
            ItemsRemoved?.Invoke(this, new PersistedEventArgs<T>(items));
        }

        protected virtual void OnItemsUpdated(List<T> items) {
            ItemsUpdated?.Invoke(this, new PersistedEventArgs<T>(items));
        }

        protected virtual void OnItemsChanged(List<T> items) {
            ItemsChanged?.Invoke(this, new PersistedEventArgs<T>(items));
        }

        protected virtual void OnItemsAdded(List<T> items) {
            ItemsAdded?.Invoke(this, new PersistedEventArgs<T>(items));
        }

        protected virtual void OnLoaded() {
            Loaded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSaved(Exception exception = null) {
            Saved?.Invoke(this, new SavedEventArgs(exception));
        }
    }
}
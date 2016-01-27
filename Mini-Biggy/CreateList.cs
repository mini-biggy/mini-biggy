using MiniBiggy.SaveStrategies;
using System;

namespace MiniBiggy {
    public static class CreateList<T> where T : new() {
        public static PersistentListBuilder UsingPath(string fullpath) {
            return new PersistentListBuilder(fullpath);
        }

        public class PersistentListBuilder {
            private string _fullPath;
            private ISaveStrategy _saveStrategy;

            public PersistentListBuilder(string fullPath) {
                _fullPath = fullPath;
            }

            private PersistentList<T> Create() {
                return new PersistentList<T>(new FileSystem.FileSystem(_fullPath), _saveStrategy);
            }

            public PersistentList<T> BackgroundSavingEvery(TimeSpan timeSpan) {
                _saveStrategy = new BackgroundSave(timeSpan);
                return Create();
            }

            public PersistentList<T> BackgroundSavingEverySecond() {
                _saveStrategy = new BackgroundSave(TimeSpan.FromSeconds(1));
                return Create();
            }

            public PersistentList<T> BackgroundSavingEveryTwoSeconds() {
                _saveStrategy = new BackgroundSave(TimeSpan.FromSeconds(2));
                return Create();
            }

            public PersistentList<T> BackgroundSavingEveryFiveSeconds() {
                _saveStrategy = new BackgroundSave(TimeSpan.FromSeconds(5));
                return Create();
            }

            public PersistentList<T> BackgroundSavingEveryMinute() {
                _saveStrategy = new BackgroundSave(TimeSpan.FromMinutes(1));
                return Create();
            }

            public PersistentList<T> SavingWhenCollectionChanges() {
                _saveStrategy = new SaveOnEveryChange();
                return Create();
            }

            public PersistentList<T> SavingWhenRequested() {
                _saveStrategy = new SaveOnlyWhenRequested();
                return Create();
            }
        }
    }
}
using MiniBiggy.SaveStrategies;

namespace MiniBiggy {
    public static class PersistentList {
        public static PersistentList<T> Create<T>() where T : new() {
            return Create<T>("");
        }

        public static PersistentList<T> Create<T>(string path) where T : new() {
            return Create<T>(path, new SaveOnlyWhenRequested());
        }
        public static PersistentList<T> Create<T>(ISaveStrategy saveStrategy) where T : new() {
            return Create<T>("", saveStrategy);
        }
        public static PersistentList<T> Create<T>(string path, ISaveStrategy saveStrategy) where T : new() {
            return new PersistentList<T>(new FileSystem.FileSystem(path), saveStrategy);
        }
    }
}

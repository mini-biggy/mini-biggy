namespace MiniBiggy {
    public static class PersistentList {
        public static PersistentList<T> Create<T>() where T : new() {
            return new PersistentList<T>(new FileSystem.FileSystem());
        }
        public static PersistentList<T> Create<T>(string path) where T : new() {
            return new PersistentList<T>(new FileSystem.FileSystem(path));
        }
    }
}

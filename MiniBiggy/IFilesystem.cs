namespace MiniBiggy {
    public interface IFilesystem {
        bool Exists(string filename);
        string ReadAllText(string filename);
        void WriteAllText(string filename, string json);
    }
}
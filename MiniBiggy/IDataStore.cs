using System.Threading.Tasks;

namespace MiniBiggy {
    public interface IDataStore {
        Task<string> ReadAllTextAsync(string listName);
        Task WriteAllTextAsync(string listName, string json);
    }
}
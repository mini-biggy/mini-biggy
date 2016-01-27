using System.Threading.Tasks;

namespace MiniBiggy {
    public interface IDataStore {
        Task<string> ReadAllTextAsync();
        Task WriteAllTextAsync(string json);
    }
}
using System.Threading.Tasks;

namespace MiniBiggy
{
    public interface IDataStore
    {
        Task<byte[]> ReadAllAsync();

        Task WriteAllAsync(byte[] list);
    }
}
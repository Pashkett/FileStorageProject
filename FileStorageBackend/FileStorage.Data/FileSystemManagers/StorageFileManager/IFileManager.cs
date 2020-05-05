using System.IO;
using System.Threading.Tasks;

namespace FileStorage.Data.FileSystemManagers.StorageFileManager
{
    public interface IFileManager
    {
        bool IsFileExists(string path);
        Task CreateFileAsync(string path, byte[] streamedFileContent);
        Task<MemoryStream> ReadFileAsync(string path);
        void DeleteFile(string path);
    }
}

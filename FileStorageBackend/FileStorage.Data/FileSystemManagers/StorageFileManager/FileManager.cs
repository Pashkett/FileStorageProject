using System;
using System.IO;
using System.Threading.Tasks;

namespace FileStorage.Data.FileSystemManagers.StorageFileManager
{
    /// <summary>
    /// Class for basic operations with Files in File System level
    /// </summary>
    public class FileManager : IFileManager
    {
        public bool IsFileExists(string path) =>
            File.Exists(path);

        public async Task CreateFileAsync(string path, byte[] streamedFileContent)
        {
            if (IsFileExists(path))
                throw new ArgumentException("File has been already exists.");
            else
                using (var targetStream = File.Create(path))
                {
                    await targetStream.WriteAsync(streamedFileContent);
                }
        }

        public async Task<MemoryStream> ReadFileAsync(string path)
        {
            if (!IsFileExists(path))
                throw new ArgumentException("Current file does not exists");
            else
            {
                var memoryStream = new MemoryStream();

                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memoryStream);
                }

                return memoryStream;
            }
        }

        public void DeleteFile(string path)
        {
            if (!IsFileExists(path))
                throw new ArgumentException("Current file does not exists.");
            else
                File.Delete(path);
        }
    }
}

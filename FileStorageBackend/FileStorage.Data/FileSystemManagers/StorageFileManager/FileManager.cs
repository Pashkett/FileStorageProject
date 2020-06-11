using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileStorage.Data.FileSystemManagers.StorageFileManager
{
    /// <summary>
    /// Class for basic operations with Files in File System level
    /// </summary>
    public class FileManager : IFileManager
    {
        private readonly IFileSystem fileSystem;

        public FileManager(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public async Task CreateFileAsync(string path, byte[] streamedFileContent)
        {
            if (path == null)
                throw new ArgumentNullException("Path must not be null");

            if (fileSystem.File.Exists(path))
                throw new ArgumentException("File has been already exists.");
            else
                using (var targetStream = fileSystem.File.Create(path))
                {
                    await targetStream.WriteAsync(streamedFileContent);
                }
        }

        public async Task<MemoryStream> ReadFileAsync(string path)
        {
            if (path == null)
                throw new ArgumentNullException("Path must not be null");

            if (!fileSystem.File.Exists(path))
                throw new ArgumentException("Current file does not exists");
            else
            {
                var memoryStream = new MemoryStream();

                using (var stream = fileSystem.FileStream.Create(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memoryStream);
                }

                return memoryStream;
            }
        }

        public void DeleteFile(string path)
        {
            if (path == null)
                throw new ArgumentNullException("Path must not be null");

            if (!fileSystem.File.Exists(path))
                throw new ArgumentException("Current file does not exists.");
            else
                fileSystem.File.Delete(path);
        }
    }
}

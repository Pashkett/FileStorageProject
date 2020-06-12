using System;
using System.IO.Abstractions;

namespace FileStorage.Data.FileSystemManagers.StorageFolderManager
{
    /// <summary>
    /// Class for basic operations with Folders in File System level
    /// </summary>
    public class FolderManager : IFolderManager
    {
        private readonly IFileSystem fileSystem;

        public FolderManager(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public bool IsFolderExists(string path)
        {
            if (path == null)
                throw new ArgumentNullException("Path must not be null");

            return fileSystem.Directory.Exists(path);
        }

        public void CreateFolder(string path)
        {
            if (path == null)
                throw new ArgumentNullException("Path must not be null");

            if (IsFolderExists(path))
                throw new ArgumentException("Folder has been already exists.");
            else
                fileSystem.Directory.CreateDirectory(path);
        }

        public void DeleteFolder(string path)
        {
            if (path == null)
                throw new ArgumentNullException("Path must not be null");

            if (!IsFolderExists(path))
                throw new ArgumentException("Folder does not exist.");
            else
                fileSystem.Directory.Delete(path, true);
        }
    }
}

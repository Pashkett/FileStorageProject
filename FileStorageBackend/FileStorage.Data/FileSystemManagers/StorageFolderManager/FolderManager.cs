using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileStorage.Data.FileSystemManagers.StorageFolderManager
{
    /// <summary>
    /// https://stackoverflow.com/questions/1300773/what-belongs-in-a-repository-and-what-doesnt
    /// </summary>
    public class FolderManager : IFolderManager
    {
        public bool IsFolderExists(string path) =>
            Directory.Exists(path);

        public bool CreateFolder(string path)
        {
            if (IsFolderExists(path))
                throw new ArgumentException("Folder has been already exists.");
            else
            {
                Directory.CreateDirectory(path);
                return true;
            }
        }

        public bool DeleteFolder(string path)
        {
            if (IsFolderExists(path))
                throw new ArgumentException("Folder has been already exists.");
            else
            {
                Directory.Delete(path, true);
                return true;
            }
        }
    }
}

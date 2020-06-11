namespace FileStorage.Data.FileSystemManagers.StorageFolderManager
{
    public interface IFolderManager
    {
        bool IsFolderExists(string path);
        void CreateFolder(string path);
        void DeleteFolder(string path);
    }
}
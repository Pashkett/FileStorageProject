namespace FileStorage.Data.FileSystemManagers.StorageFolderManager
{
    public interface IFolderManager
    {
        bool CreateFolder(string path);
        bool IsFolderExists(string path);
        bool DeleteFolder(string path);
    }
}
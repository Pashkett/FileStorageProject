using System;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using FileStorage.Data.FileSystemManagers.StorageFolderManager;

namespace FileStorage.Data.Tests
{
    [TestFixture]
    public class FolderManagerTests
    {
        [Test]
        public void Folder_Path_Is_Null_For_Check_Existence()
        {
            string path = null;
            var mockFileSystem = new MockFileSystem();
            var sut = new FolderManager(mockFileSystem);

            Assert.That(() => sut.IsFolderExists(path), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Folder_Exists_By_Path()
        {
            string path = @"C:\\TargetFolder\Target";
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddDirectory(path);
            var sut = new FolderManager(mockFileSystem);

            Assert.That(sut.IsFolderExists(path), Is.True);
        }

        [Test]
        public void Folder_Is_Null_For_Creation()
        {
            string path = null;
            var mockFileSystem = new MockFileSystem();
            var sut = new FolderManager(mockFileSystem);

            Assert.That(() => sut.CreateFolder(path), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Folder_Has_Been_Already_Exists_For_Creation()
        {
            var path = @"C:\SourceFiles\file.txt";
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddDirectory(path);
            var sut = new FolderManager(mockFileSystem);

            Assert.That(() => sut.CreateFolder(path), Throws.TypeOf<ArgumentException>());
        }
        
        [Test]
        public void Folder_Does_Not_Exists_For_Deletion()
        {
            var path = @"C:\SourceFiles\file.txt";
            var mockFileSystem = new MockFileSystem();
            var sut = new FolderManager(mockFileSystem);

            Assert.That(() => sut.DeleteFolder(path), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Successfully_Folder_Deletion()
        {
            var path = @"C:\SourceFiles\file.txt";
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddDirectory(path);
            var sut = new FolderManager(mockFileSystem);

            sut.DeleteFolder(path);

            Assert.That(mockFileSystem.Directory.Exists(path), Is.False);
        }

        [Test]
        public void Successfully_Folder_Creation()
        {
            var path = @"C:\SourceFiles\file.txt";
            var mockFileSystem = new MockFileSystem();
            var sut = new FolderManager(mockFileSystem);

            sut.CreateFolder(path);

            Assert.That(mockFileSystem.Directory.Exists(path), Is.True);
        }
    }
}

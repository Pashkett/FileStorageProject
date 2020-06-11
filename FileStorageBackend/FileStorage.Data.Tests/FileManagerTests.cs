using System;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using NUnit.Framework.Internal;
using FileStorage.Data.FileSystemManagers.StorageFileManager;

namespace FileStorage.Data.Tests
{
    [TestFixture]
    public class FileManagerTests
    {
        [Test]
        public void Successfuly_Create_File_From_Path()
        {
            var pathSorce = @"C:\SourceFiles\file.txt";
            var pathTarget = @"C:\TargetFiles\file.txt";
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(pathSorce, new MockFileData("TestFile"));
            mockFileSystem.AddDirectory(@"C:\TargetFiles");
            var bytes = mockFileSystem.File.ReadAllBytes(pathSorce);
            var sut = new FileManager(mockFileSystem);

            sut.CreateFileAsync(pathTarget, bytes).Wait();

            Assert.That(mockFileSystem.FileExists(pathTarget), Is.True);
        }

        [Test]
        public void File_Has_Been_Already_Exists_When_Created()
        {
            var path = @"C:\SourceFiles\file.txt";
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(path, new MockFileData("TestFile"));
            var bytes = mockFileSystem.File.ReadAllBytes(path);
            var sut = new FileManager(mockFileSystem);

            Assert.That(() => sut.CreateFileAsync(path, bytes), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void File_Does_Not_Exists_For_Reading()
        {
            var path = @"C:\files\toDelete.txt";
            var mockFileSystem = new MockFileSystem();
            var sut = new FileManager(mockFileSystem);

            Assert.That(() => sut.ReadFileAsync(path), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Successful_Delete_File_From_Path()
        {
            var path = @"C:\files\toDelete.txt"; 
            var mockFileSystem = new MockFileSystem();
            var mockFileToDelete = new MockFileData("TestFile");
            mockFileSystem.AddFile(path, mockFileToDelete);
            var sut = new FileManager(mockFileSystem);

            sut.DeleteFile(path);

            Assert.That(mockFileSystem.FileExists(path), Is.False);
        }

        [Test]
        public void File_Does_Not_Exists_For_Deletion()
        {
            var path = @"C:\files\toDelete.txt";
            var mockFileSystem = new MockFileSystem();
            var sut = new FileManager(mockFileSystem);

            Assert.That(() => sut.DeleteFile(path), Throws.TypeOf<ArgumentException>());
        }
    }
}
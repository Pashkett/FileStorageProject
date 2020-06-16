using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using FileStorage.Domain.Utilities;
using FileStorage.Domain.Exceptions;

namespace FileStorage.Domain.Tests
{
    [TestFixture]
    public class FormFileExtensionsTests
    {
        private Mock<IFormFile> sut;
        private string fileName;

        [SetUp]
        public void Setup()
        {
            fileName = "Test.txt";
            sut = new Mock<IFormFile>();
            sut.Setup(f => f.FileName).Returns(fileName);
        }

        [Test]
        public void Empty_File_Exception()
        {
            sut.Setup(file => file.Length).Returns(0);

            Assert.That(() => sut.Object.ProcessFormFileAsync(12000),
                Throws.TypeOf<StorageItemEmptyException>()
                    .With
                    .Matches<StorageItemEmptyException>(
                        ex => ex.Message == $"File: {fileName} is empty from FormFile."));
        }

        [Test]
        public void File_Exceed_Limit_Exception()
        {
            sut.Setup(file => file.Length).Returns(1000);

            Assert.That(() => sut.Object.ProcessFormFileAsync(999),
                Throws.TypeOf<StorageItemExceedLimitException>());
        }

        [Test]
        public void Empty_File_Exception_From_Memory_Stream()
        { 
            sut.Setup(file => file.Length).Returns(999);

            Assert.That(() => sut.Object.ProcessFormFileAsync(1000),
                Throws.TypeOf<StorageItemEmptyException>()
                    .With
                    .Matches<StorageItemEmptyException>(
                        ex => ex.Message == $"File: {fileName} is empty from MemoryStream."));
        }
    }
}
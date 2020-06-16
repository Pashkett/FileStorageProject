using FileStorage.Data.Models;
using FileStorage.Domain.Utilities;
using NUnit.Framework;
using System;

namespace FileStorage.Domain.Tests
{
    [TestFixture]
    public class StorageItemsHelpersTests
    {
        [Test]
        public void Exception_Relative_Folder_Path_For_File()
        {
            var file = new StorageItem()
            {
                DisplayName = "testDisplay",
                TrustedName = "testTrust",
                IsFolder = false
            };

            Assert.That(() => StorageItemsHelpers.GetFolderRelativePath(file),
                Throws.TypeOf<ArgumentException>()
                    .With.Matches<ArgumentException>(
                        ex => ex.Message == "Item should be folder."));
        }
        
        [Test]
        public void Exception_Relative_NonPrimary_Folder_Path_Without_Parent()
        {
            var folder = new StorageItem()
            {
                DisplayName = "testDisplay",
                TrustedName = "testTrust",
                IsFolder = true,
                IsPrimaryFolder = false,
                ParentFolder = null
            };

            Assert.That(() => StorageItemsHelpers.GetFolderRelativePath(folder),
                Throws.TypeOf<ArgumentException>()
                    .With.Matches<ArgumentException>(
                        ex => ex.Message == "Non-primary folder should have a parent folder"));
        }
    }
}

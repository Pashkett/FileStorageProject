using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using FileStorage.Data.Models;
using FileStorage.Data.Persistence.Extensions;
using NUnit.Framework.Internal;

namespace FileStorage.Data.Tests
{
    [TestFixture]
    public class RepositoryStorageItemExtensionsTests
    {
        private IQueryable<StorageItem> sut;

        [OneTimeSetUp]
        public void Setup()
        {
            var filesList = new List<StorageItem>(new StorageItem[]
            {
                new StorageItem { DisplayName = "First", Size = 1_000_000,Extension = ".txt" },
                new StorageItem { DisplayName = "Second", Size = 2_000_000, Extension = ".txt" },
                new StorageItem { DisplayName = "Third", Size = 3_000_000, Extension = ".txt" },
                new StorageItem { DisplayName = "Fourth", Size = 4_000_000, Extension = ".txt" },
                new StorageItem { DisplayName = "Fifth", Size = 5_000_000, Extension = ".txt" },
                new StorageItem { DisplayName = "Sixth", Size = 6_000_000, Extension = ".txt" },
                new StorageItem { DisplayName = "Seventh", Size = 7_000_000, Extension = ".abc" }
            });

            sut = filesList.AsQueryable<StorageItem>();
        }

        [Test]
        [TestCase(1, 2, ExpectedResult = 2)]
        [TestCase(4, 2, ExpectedResult = 1)]
        [TestCase(5, 2, ExpectedResult = 0)]
        [TestCase(1, 3, ExpectedResult = 3)]
        [TestCase(3, 3, ExpectedResult = 1)]
        [TestCase(5, 3, ExpectedResult = 0)]
        public int Pagination_Testing(int pageNumber, int pageSize)
        {
            return sut.PageStorageItems(pageNumber, pageSize).Count();
        }

        [Test]
        public void Null_Exception_For_Null_Input_For_Pagination()
        {
            var sut = (IQueryable<StorageItem>)null;

            Assert.That(() => sut.PageStorageItems(1, 1), 
                Throws.TypeOf<System.ArgumentNullException>());
        }

        [Test]
        [TestCase(1_000_000, 70_000_000, ExpectedResult = 7)]
        [TestCase(2_000_000, 70_000_000, ExpectedResult = 6)]
        [TestCase(3_000_000, 70_000_000, ExpectedResult = 5)]
        [TestCase(4_000_000, 70_000_000, ExpectedResult = 4)]
        [TestCase(5_000_000, 70_000_000, ExpectedResult = 3)]
        [TestCase(50_000_000, 710_000_000, ExpectedResult = 0)]
        public int Filter_Storage_By_Size_Testing(long minSize, long maxSize)
        {
            return sut.FilterStorageItemsBySize(minSize, maxSize).Count();
        }

        [Test]
        public void Null_Exception_For_Null_Input_For_Filter_By_Size()
        {
            var sut = (IQueryable<StorageItem>)null;

            Assert.That(() => sut.FilterStorageItemsBySize(10_000_000, 100_000_000),
                Throws.TypeOf<System.ArgumentNullException>());
        }

        [Test]
        [TestCase("irST", ExpectedResult = 1_000_000)]
        [TestCase("Seven", ExpectedResult = 7_000_000)]
        [TestCase("eventh", ExpectedResult = 7_000_000)]
        [TestCase("venth", ExpectedResult = 7_000_000)]
        [TestCase("enth", ExpectedResult = 7_000_000)]
        [TestCase("th", ExpectedResult = 7_000_000)]

        public long SearchBy_Testing(string searchTerm)
        {
            return sut.SearchBy(searchTerm).Last().Size;
        }

        [Test]
        public void Null_Input_String_As_SearchTerm()
        {
            long quantity = 7;
            string searchTerm = null;

            Assert.That(sut.SearchBy(searchTerm).Count(), Is.EqualTo(quantity));
        }

        [Test]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        [TestCase("SuperName")]
        public void Sort_Testign_With_Null_Sorting_Condition(string orderByString)
        {
            Assert.That(sut.Sort(orderByString), Is.Ordered.By("DisplayName").Ascending);
        }

        [Test]
        public void Sort_By_Extension_Descending()
        {
            var orderByString = "extension desc";

            Assert.That(sut.Sort(orderByString), Is.Ordered.By("Extension").Descending);
        }
    }
}

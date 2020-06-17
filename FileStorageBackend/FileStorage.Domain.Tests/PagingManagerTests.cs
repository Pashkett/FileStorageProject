using NUnit.Framework;
using FileStorage.Domain.RequestModels;
using FileStorage.Domain.PagingHelpers;

namespace FileStorage.Domain.Tests
{
    [TestFixture]
    public class PagingManagerTests
    {
        [Test]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(10, ExpectedResult = 2)]
        [TestCase(95, ExpectedResult = 19)]
        [TestCase(99, ExpectedResult = 20)]
        [TestCase(100, ExpectedResult = 20)]
        public int Calculate_Pages_Quantity_For_Default_Request_Parameters(int totalCount)
        {
            var parameters = new StorageItemsRequestParameters();

            var sut = PagingManager.PrepareHeader(totalCount, parameters);

            return sut.TotalPages;
        }

        [Test]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(10, ExpectedResult = 2)]
        [TestCase(95, ExpectedResult = 19)]
        [TestCase(99, ExpectedResult = 20)]
        [TestCase(100, ExpectedResult = 20)]
        public int Calculate_Pages_For_Null_Request_Parameters(int totalCount)
        {
            StorageItemsRequestParameters parameters = null;

            var sut = PagingManager.PrepareHeader(totalCount, parameters);

            return sut.TotalPages;
        }
    }
}

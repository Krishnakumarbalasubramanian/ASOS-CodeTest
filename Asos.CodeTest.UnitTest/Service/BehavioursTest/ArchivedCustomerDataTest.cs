namespace Asos.CodeTest.UnitTest.Service.BehavioursTest
{
    using Asos.CodeTest.DataAccess.Interfaces;
    using Asos.CodeTest.Service;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Threading.Tasks;

    [TestClass]
    public class ArchivedCustomerDataTest
    {
        private readonly ArchivedCustomerData _archivedCustomerData;

        private readonly Mock<IArchivedDataService> _archiveDataService;

        public ArchivedCustomerDataTest()
        {
            this._archiveDataService = new Mock<IArchivedDataService>();
            this._archivedCustomerData = new ArchivedCustomerData(this._archiveDataService.Object);
        }

        [TestMethod]
        public async Task ShouldReturnCustomerData_OnPassingCustomerId()
        {
            this._archiveDataService.Setup(x => x.GetArchivedCustomer(It.IsAny<int>())).Returns(new Models.Customer() { });
            var result = await this._archivedCustomerData.GetCustomerDataByCustomerId(1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Models.Customer));
        }
    }
}

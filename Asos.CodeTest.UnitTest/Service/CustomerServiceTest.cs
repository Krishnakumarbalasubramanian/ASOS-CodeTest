namespace Asos.CodeTest.UnitTest.Service
{
    using Asos.CodeTest.Service.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    [TestClass]
    public class CustomerServiceTest
    {
        private readonly Mock<IArchivedCustomerData> _archivedCustomerData;

        private readonly Mock<IFailoverCustomerDataService> _failoverCustomerDataService;

        private readonly Mock<ICustomerData> _customerData;

        private readonly CustomerService _customerService;

        private readonly Models.Customer _customer;
        public CustomerServiceTest()
        {
            this._archivedCustomerData = new Mock<IArchivedCustomerData>();
            this._failoverCustomerDataService = new Mock<IFailoverCustomerDataService>();
            this._customerData = new Mock<ICustomerData>();

            this._customerService = new CustomerService(
                this._archivedCustomerData.Object, 
                this._failoverCustomerDataService.Object,
                this._customerData.Object);

            this._customer = new Models.Customer
            {
                Id = 1,
                Name = "test name",
            };

            var customerService = new CustomerService();
        }

        [TestMethod]
        public async Task ShouldReturnCustomer_DataToBeFetchedForArchievedCustomerData()
        {
            this._archivedCustomerData.Setup(x => x.GetCustomerDataByCustomerId(It.IsAny<int>()))
                .ReturnsAsync(this._customer);

            var result = await this._customerService.GetCustomer(1, true);

            this.AssertTestResponse(result);
        }

        [TestMethod]
        public async Task ShouldReturnCustomer_DataToBeFetchedForFailoverCustomerData()
        {
            this._failoverCustomerDataService.Setup(x => x.GetCustomerDataByCustomerId(It.IsAny<Models.CustomerResponse>()))
                .ReturnsAsync(this._customer);

            var result = await this._customerService.GetCustomer(1, false);

            this.AssertTestResponse(result);
        }

        private void AssertTestResponse(Models.Customer result)
        {
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Models.Customer));

            var expectedObject = JsonConvert.SerializeObject(this._customer);
            var actualObject = JsonConvert.SerializeObject(result);
            Assert.AreEqual(expectedObject, actualObject);
        }
    }
}

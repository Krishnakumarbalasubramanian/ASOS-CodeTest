namespace Asos.CodeTest.UnitTest.Service.BehavioursTest
{
    using Asos.CodeTest.DataAccess.Interfaces;
    using Asos.CodeTest.Service;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Threading.Tasks;

    [TestClass]
    public class CustomerDataTest
    {
        private readonly CustomerData _customerData;

        private readonly Mock<ICustomerDataAccess> _customerDataAccess;

        public CustomerDataTest()
        {
            this._customerDataAccess = new Mock<ICustomerDataAccess>();
            this._customerData = new CustomerData(this._customerDataAccess.Object);
        }

        [TestMethod]
        public async Task GetCustomerResponseByCustomerId_Success_Test()
        {
            this._customerDataAccess.Setup(x => x.LoadCustomerAsync(It.IsAny<int>())).ReturnsAsync(new Models.CustomerResponse() { });
            var result = await this._customerData.GetCustomerResponseByCustomerId(1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Models.CustomerResponse));
        }
    }
}

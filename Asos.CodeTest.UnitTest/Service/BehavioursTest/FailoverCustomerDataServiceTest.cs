namespace Asos.CodeTest.UnitTest.Service.BehavioursTest
{
    using Asos.CodeTest.Repository.Interface;
    using Asos.CodeTest.Service;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using Asos.CodeTest.Service.Interfaces;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    [TestClass]
    public class FailoverCustomerDataServiceTest
    {
        private readonly FailoverCustomerDataService _failoverCustomerDataService;

        private readonly Mock<IFailoverRepository> _failoverRepository;

        private readonly Mock<ICustomerData> _customerData;

        private readonly Mock<IArchivedCustomerData> _archivedCustomerData;

        private Mock<IFailoverCustomerData> _failoverCustomerData;

        private Models.CustomerResponse _customerResponse;

        private List<FailoverEntry> _failoverEntries;

        public FailoverCustomerDataServiceTest()
        {
            this._failoverRepository = new Mock<IFailoverRepository>();
            this._customerData = new Mock<ICustomerData>();
            this._archivedCustomerData = new Mock<IArchivedCustomerData>();
            this._failoverCustomerData = new Mock<IFailoverCustomerData>();

            this._failoverCustomerDataService = new FailoverCustomerDataService(
                this._failoverRepository.Object,
                this._customerData.Object,
                this._failoverCustomerData.Object,
                this._archivedCustomerData.Object);

            this._customerResponse = new Models.CustomerResponse
            {
                IsArchived = true,
                Customer = new Models.Customer
                {
                    Id = 1,
                    Name = "test name",
                },
            };

            this._failoverEntries = new List<FailoverEntry>();
            for (int i = 1; i <= 103; i++)
            {
                this._failoverEntries.Add(new FailoverEntry { DateTime = DateTime.Now });
            }

            this._failoverRepository.Setup(x => x.GetFailOverEntries())
                .Returns(this._failoverEntries.Skip(0).Take(5).ToList());

            this._customerData.Setup(x => x.GetCustomerResponseByCustomerId(It.IsAny<int>()))
                .ReturnsAsync(this._customerResponse);

            this._archivedCustomerData.Setup(x => x.GetCustomerDataByCustomerId(It.IsAny<int>()))
                .ReturnsAsync(this._customerResponse.Customer);
        }

        [TestMethod]
        public async Task ShouldReturnCustomerData_FailoverButArchivedCustomerData()
        {
            var result = await this._failoverCustomerDataService.GetCustomerDataByCustomerId(this._customerResponse);

            this.AssertTestResponse(result);
        }

        [TestMethod]
        public async Task ShouldReturnCustomerData_FailoverCustomerData()
        {
            this._customerResponse.IsArchived = false;

            this._customerData.Setup(x => x.GetCustomerResponseByCustomerId(It.IsAny<int>()))
                .ReturnsAsync(this._customerResponse);

            var result = await this._failoverCustomerDataService.GetCustomerDataByCustomerId(this._customerResponse);

            this.AssertTestResponse(result);
        }


        [TestMethod]
        public async Task ShouldReturnCustomerData_FailoverCustomerData_FromFailoverCustomerData()
        {
            this._failoverRepository.Setup(x => x.GetFailOverEntries())
                .Returns(this._failoverEntries);

            this._customerResponse.IsArchived = false;

            this._failoverCustomerData.Setup(x => x.GetCustomerResponseByCustomerId(It.IsAny<int>())).ReturnsAsync(this._customerResponse);

            var result = await this._failoverCustomerDataService.GetCustomerDataByCustomerId(this._customerResponse);
            
            this.AssertTestResponse(result);
        }

        private void AssertTestResponse(Models.Customer result)
        {
            Assert.IsNotNull(result);
            var expectedObject = JsonConvert.SerializeObject(this._customerResponse.Customer);
            var actualObject = JsonConvert.SerializeObject(result);
            Assert.AreEqual(expectedObject, actualObject);
        }
    }
}

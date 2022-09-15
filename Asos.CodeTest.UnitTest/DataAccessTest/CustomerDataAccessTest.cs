namespace Asos.CodeTest.UnitTest.DataAccessTest
{
    using Asos.CodeTest.DataAccess;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Fakes;
    using System.Threading.Tasks;

    [TestClass]
    public class CustomerDataAccessTest
    {
        private readonly CustomerDataAccess _customerDataAccess;

        public CustomerDataAccessTest()
        {
            this._customerDataAccess = new CustomerDataAccess();
        }

        [TestMethod]
        public async Task ShouldReturnCustomerData_OnPassingCustomerID()
        {
            using (ShimsContext.Create())
            {
                var expectedResponse = new Models.CustomerResponse()
                {
                    IsArchived = false,
                    Customer = new Models.Customer
                    {
                        Id = 1,
                        Name = "test"
                    }
                };
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(
                    JsonConvert.SerializeObject(
                        expectedResponse, Formatting.Indented));

                ShimHttpClient.AllInstances.SendAsyncHttpRequestMessageHttpCompletionOptionCancellationToken = (r, a, message, s) => Task.FromResult(response);

                var result = await this._customerDataAccess.LoadCustomerAsync(1);
                Assert.IsNotNull(result.Customer);
                Assert.IsInstanceOfType(result, typeof(Models.CustomerResponse));

                var expectedObject = JsonConvert.SerializeObject(expectedResponse);
                var actualObject = JsonConvert.SerializeObject(result);
                Assert.AreEqual(expectedObject, actualObject);
            }
        }

        [TestMethod]
        public async Task Should_NOT_ReturnCustomerData_OnPassingCustomerID()
        {
            using (ShimsContext.Create())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(
                    JsonConvert.SerializeObject(
                        new Models.Customer()
                        {
                                Id = 1,
                                Name = "test"   
                        }, Formatting.Indented));

                ShimHttpClient.AllInstances.SendAsyncHttpRequestMessageHttpCompletionOptionCancellationToken = (r, a, message, s) => Task.FromResult(response);

                var result = await this._customerDataAccess.LoadCustomerAsync(1);
                Assert.IsNull(result.Customer);
                Assert.IsNotInstanceOfType(result.Customer, typeof(Models.Customer));
            }
        }
    }
}

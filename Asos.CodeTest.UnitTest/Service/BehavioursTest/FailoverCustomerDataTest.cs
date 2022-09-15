namespace Asos.CodeTest.UnitTest.Service.BehavioursTest
{
    using Asos.CodeTest.Service;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.QualityTools.Testing.Fakes;
    using Newtonsoft.Json;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Fakes;
    using System.Threading.Tasks;

    [TestClass]
    public class FailoverCustomerDataTest
    {
        private readonly FailoverCustomerData _failoverCustomerData;

        public FailoverCustomerDataTest()
        {
            this._failoverCustomerData = new FailoverCustomerData();
        }

        [TestMethod]
        public async Task ShouldReturnCustomerResponseBasedonCustomerId()
        {
            using (ShimsContext.Create())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(
                    JsonConvert.SerializeObject(
                        new Models.CustomerResponse()
                        {
                            IsArchived = false,
                            Customer = new Models.Customer
                            {
                                Id = 1,
                                Name = "test"
                            }
                        }, Formatting.Indented));

                ShimHttpClient.AllInstances.SendAsyncHttpRequestMessageHttpCompletionOptionCancellationToken = (r, a, message, s) => Task.FromResult(response);

                var result = await this._failoverCustomerData.GetCustomerResponseByCustomerId(1);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Models.CustomerResponse));
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

                var result = await this._failoverCustomerData.GetCustomerResponseByCustomerId(1);
                Assert.IsNull(result.Customer);
                Assert.IsNotInstanceOfType(result.Customer, typeof(Models.Customer));
            }
        }
    }
}

namespace Asos.CodeTest.UnitTest.DataAccessTest
{
    using Asos.CodeTest.DataAccess;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using System.Data;
    using System.Data.Common.Fakes;
    using System.Data.SqlClient.Fakes;

    [TestClass]
    public class ArchivedDataServiceTest
    {
        private readonly ArchivedDataService _archivedDataService;

        public ArchivedDataServiceTest()
        {
            this._archivedDataService = new ArchivedDataService();
        }

        [TestMethod]
        public void ShouldReturnCustomerData_BasedOnCustomerId()
        {
            using (ShimsContext.Create())
            {
                ShimSqlConnection.AllInstances.Open = connection => { };

                string commandText;
                ShimSqlCommand.AllInstances.ExecuteReader = command =>
                {
                    commandText = command.CommandText;
                    return new ShimSqlDataReader();
                };

                int readCount = 0;
                ShimSqlDataReader.AllInstances.Read = reader => readCount == 0;
                ShimSqlDataReader.AllInstances.GetInt32Int32 = (reader, i) =>
                {
                    readCount++;
                    return 1;
                };

                ShimSqlDataReader.AllInstances.GetStringInt32 = (reader, i) =>
                {
                    readCount++;
                    return "test name";
                };

                var result = this._archivedDataService.GetArchivedCustomer(1);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Models.Customer));

                var expectedResponse = new Models.Customer { Id = 1, Name = "test name" };
                var expectedObject = JsonConvert.SerializeObject(expectedResponse);
                var actualObject = JsonConvert.SerializeObject(result);
                Assert.AreEqual(expectedObject, actualObject);
            }
        }

        [TestMethod]
        public void Should_NOT_ReturnCustomerData_BasedOnCustomerId()
        {
            using (ShimsContext.Create())
            {
                ShimSqlConnection.AllInstances.Open = connection => { };

                string commandText;
                ShimSqlCommand.AllInstances.ExecuteReader = command =>
                {
                    commandText = command.CommandText;
                    return new ShimSqlDataReader();
                };

                var result = this._archivedDataService.GetArchivedCustomer(1);
                Assert.IsNull(result);
                Assert.IsNotInstanceOfType(result, typeof(Models.Customer));
            }
        }
    }
}

namespace Asos.CodeTest.UnitTest.RepositoryTest
{
    using Asos.CodeTest.Repository;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Data.SqlClient.Fakes;
    using System.Linq;

    [TestClass]
    public class FailoverRepositoryTest
    {
        private readonly FailoverRepository _failoverRepository;

        public FailoverRepositoryTest()
        {
            this._failoverRepository = new FailoverRepository();
        }

        [TestMethod]
        public void ShouldReturnFailoverEntries_OnPassingCustomerId()
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
                ShimSqlDataReader.AllInstances.GetDateTimeInt32 = (reader, i) =>
                {
                    if (readCount <= 50)
                    {
                        readCount++;
                    }
                    return System.DateTime.Now;
                };

                var result = this._failoverRepository.GetFailOverEntries();
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(System.Collections.Generic.List<FailoverEntry>));
                Assert.AreEqual(System.DateTime.Now.Date, result.FirstOrDefault().DateTime.Date);
            }
        }

        [TestMethod]
        public void Should_NOT_ReturnFailoverEntries_OnPassingCustomerId()
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

                var result = this._failoverRepository.GetFailOverEntries();
                Assert.AreEqual(0, result.Count);
            }
        }
    }
}

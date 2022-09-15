namespace Asos.CodeTest.Service
{
    using Asos.CodeTest.DataAccess;
    using Asos.CodeTest.Models;
    using Asos.CodeTest.Service.Interfaces;
    using System.Threading.Tasks;
    public class FailoverCustomerData : IFailoverCustomerData
    { 
        public async Task<CustomerResponse> GetCustomerResponseByCustomerId(int customerId)
        {
            return await FailoverCustomerDataAccess.GetCustomerById(customerId);
        }
    }
}

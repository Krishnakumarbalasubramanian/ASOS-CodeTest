namespace Asos.CodeTest.Service
{
    using Asos.CodeTest.DataAccess.Interfaces;
    using Asos.CodeTest.Models;
    using Asos.CodeTest.Service.Interfaces;
    using System.Threading.Tasks;

    public class CustomerData : ICustomerData
    {
        private readonly ICustomerDataAccess _customerDataAccess;

        public CustomerData(ICustomerDataAccess customerDataAccess)
        {
            this._customerDataAccess = customerDataAccess;
        }

        public async Task<CustomerResponse> GetCustomerResponseByCustomerId(int customerId)
        {
            return await this._customerDataAccess.LoadCustomerAsync(customerId);
        }
    }
}

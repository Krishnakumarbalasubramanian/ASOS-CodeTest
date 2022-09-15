namespace Asos.CodeTest
{
    using Asos.CodeTest.DataAccess;
    using Asos.CodeTest.Models;
    using Asos.CodeTest.Repository;
    using Asos.CodeTest.Service;
    using Asos.CodeTest.Service.Interfaces;
    using System.Threading.Tasks;

    public class CustomerService
    {
        private readonly IArchivedCustomerData _archivedCustomerData;

        private readonly ICustomerData _customerData;

        private readonly IFailoverCustomerData _failoverCustomerData;

        private readonly IFailoverCustomerDataService _failoverCustomerDataService;
        public CustomerService()
        {
            this._archivedCustomerData = new ArchivedCustomerData(new ArchivedDataService());
            this._customerData = new CustomerData(new CustomerDataAccess());
            this._failoverCustomerData = new FailoverCustomerData();
            this._failoverCustomerDataService = new FailoverCustomerDataService(
                new FailoverRepository(),
                this._customerData,
                this._failoverCustomerData,
                this._archivedCustomerData);
        }

        public CustomerService(
            IArchivedCustomerData archivedCustomerData,            
            IFailoverCustomerDataService failoverCustomerDataService)
        {
            this._archivedCustomerData = archivedCustomerData;
            this._failoverCustomerDataService = failoverCustomerDataService;
        }
        public async Task<Customer> GetCustomer(int customerId, bool isCustomerArchived)
        {
            if (isCustomerArchived)
            {
                return await this._archivedCustomerData.GetCustomerDataByCustomerId(customerId);
            }

            return await this._failoverCustomerDataService.GetCustomerDataByCustomerId(customerId);
        }
    }
}

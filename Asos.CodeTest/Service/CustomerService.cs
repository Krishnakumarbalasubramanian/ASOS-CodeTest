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
            IFailoverCustomerDataService failoverCustomerDataService,
            ICustomerData customerData)
        {
            this._archivedCustomerData = archivedCustomerData;
            this._failoverCustomerDataService = failoverCustomerDataService;
            this._customerData = customerData;
        }

        public async Task<Customer> GetCustomer(int customerId, bool isCustomerArchived = default)
        {

            var customerData = await this._customerData.GetCustomerResponseByCustomerId(customerId);

            if (customerData.IsArchived)
            {
                return await this._archivedCustomerData.GetCustomerDataByCustomerId(customerId);
            }

            return await this._failoverCustomerDataService.GetCustomerDataByCustomerId(customerData);
        }
    }
}

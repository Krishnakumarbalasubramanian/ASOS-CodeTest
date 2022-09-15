namespace Asos.CodeTest.Service
{
    using Asos.CodeTest.Models;
    using Asos.CodeTest.Repository.Interface;
    using Asos.CodeTest.Service.Interfaces;
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    public class FailoverCustomerDataService : IFailoverCustomerDataService
    {
        private readonly IFailoverRepository _failoverRepository;

        private readonly IArchivedCustomerData _archivedCustomerData;

        private readonly ICustomerData _customerData;

        private readonly IFailoverCustomerData _failoverCustomerData;

        public FailoverCustomerDataService(
            IFailoverRepository failoverRepository,
            ICustomerData customerData,
            IFailoverCustomerData failoverCustomerData,
            IArchivedCustomerData archivedCustomerData)
        {
            this._failoverRepository = failoverRepository;
            this._customerData = customerData;
            this._failoverCustomerData = failoverCustomerData;
            this._archivedCustomerData = archivedCustomerData;
        }


        public async Task<Customer> GetCustomerDataByCustomerId(int customerId)
        {
            var failoverEntries = this._failoverRepository.GetFailOverEntries();

            var failedRequests = failoverEntries.Count(x => x.DateTime > DateTime.Now.AddMinutes(-10));

            var customerResponse = 
                (failedRequests > 100
                && ConfigurationManager.AppSettings["IsFailoverModeEnabled"].ToLower().Trim() == "true")
                ? await this._failoverCustomerData.GetCustomerResponseByCustomerId(customerId)
                : await this._customerData.GetCustomerResponseByCustomerId(customerId);

            var customer = 
                (customerResponse.IsArchived)
                ? await this._archivedCustomerData.GetCustomerDataByCustomerId(customerId)
                : customerResponse.Customer;

            return customer;
        }
    }
}

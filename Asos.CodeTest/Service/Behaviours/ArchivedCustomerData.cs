namespace Asos.CodeTest.Service
{
    using Asos.CodeTest.DataAccess.Interfaces;
    using Asos.CodeTest.Models;
    using Asos.CodeTest.Service.Interfaces;
    using System.Threading.Tasks;

    public class ArchivedCustomerData : IArchivedCustomerData
    {
        private readonly IArchivedDataService _archivedDataService;

        public ArchivedCustomerData(IArchivedDataService archivedDataService)
        {
            this._archivedDataService = archivedDataService;
        }

        public async Task<Customer> GetCustomerDataByCustomerId(int customerId)
        {
            return await Task.Run(() => this._archivedDataService.GetArchivedCustomer(customerId));
        }
    }
}

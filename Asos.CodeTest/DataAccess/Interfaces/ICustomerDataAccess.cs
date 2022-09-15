namespace Asos.CodeTest.DataAccess.Interfaces
{
    using System.Threading.Tasks;
    using Asos.CodeTest.Models;

    public interface ICustomerDataAccess
    {
        Task<CustomerResponse> LoadCustomerAsync(int customerId);
    }
}

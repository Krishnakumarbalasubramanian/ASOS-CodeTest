using System.Threading.Tasks;
using Asos.CodeTest.Models;

namespace Asos.CodeTest.Service.Interfaces
{
    public interface IFailoverCustomerDataService
    {
        Task<Customer> GetCustomerDataByCustomerId(CustomerResponse customer);
    }
}

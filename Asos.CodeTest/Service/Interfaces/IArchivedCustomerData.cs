using Asos.CodeTest.Models;
using System.Threading.Tasks;

namespace Asos.CodeTest.Service.Interfaces
{
    public interface IArchivedCustomerData
    {
        Task<Customer> GetCustomerDataByCustomerId(int customerId);
    }
}

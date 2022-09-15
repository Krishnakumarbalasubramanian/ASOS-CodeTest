using System.Threading.Tasks;
using Asos.CodeTest.Models;

namespace Asos.CodeTest.Service.Interfaces
{
    public interface IFailoverCustomerData
    {
        Task<CustomerResponse> GetCustomerResponseByCustomerId(int customerId);
    }
}

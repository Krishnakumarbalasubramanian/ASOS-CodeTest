using Asos.CodeTest.Models;
using System.Threading.Tasks;

namespace Asos.CodeTest.Service.Interfaces
{
    public  interface ICustomerData
    {
        Task<CustomerResponse> GetCustomerResponseByCustomerId(int customerId);
    }
}

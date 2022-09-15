namespace Asos.CodeTest.DataAccess.Interfaces
{
    using Asos.CodeTest.Models;

    public interface IArchivedDataService
    {
        Customer GetArchivedCustomer(int customerId);
    }
}

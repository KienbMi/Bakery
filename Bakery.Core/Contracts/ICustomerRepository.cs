using Bakery.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakery.Core.Contracts
{
    public interface ICustomerRepository
    {
        Task<int> GetCountAsync();
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int customerId);
    }
}
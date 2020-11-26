using Bakery.Core.Contracts;
using Bakery.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
            => await _dbContext.Customers
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToArrayAsync();

        public async Task<Customer> GetByIdAsync(int customerId)
            => await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId);

        public async Task<int> GetCountAsync()
        {
            return await _dbContext.Customers.CountAsync();
        }
    }
}

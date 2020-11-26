using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Persistence
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<OrderDto>> GetAllDtosAsync()
            => await _dbContext.Orders
                    .Include(o => o.Customer)
                    .OrderBy(o => o.OrderNr)
                    .Select(o => new OrderDto(o))
                    .ToArrayAsync();
        public async Task<IEnumerable<OrderDto>> GetDtosByLastnameAsync(string filterLastName)
            => await _dbContext.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.Customer.LastName.Contains(filterLastName))
                    .OrderBy(o => o.OrderNr)
                    .Select(o => new OrderDto(o))
                    .ToArrayAsync();

        public async Task<int> GetCountAsync()
        {
            return await _dbContext.Orders.CountAsync();
        }
    }
}

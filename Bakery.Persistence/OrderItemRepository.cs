using Bakery.Core.Contracts;
using Bakery.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bakery.Persistence
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Remove(OrderItem orderItem)
        {
            _dbContext.Remove(orderItem);
        }

        public async Task<OrderItem> GetByIdAsync(int itemId)
            => await _dbContext.OrderItems
                    .SingleOrDefaultAsync(item => item.Id == itemId);

        public async Task<int> GetCountAsync()
        {
            return await _dbContext.OrderItems.CountAsync();
        }
    }
}

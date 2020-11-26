using Bakery.Core.Entities;
using System.Threading.Tasks;

namespace Bakery.Core.Contracts
{
    public interface IOrderItemRepository
    {
        Task<int> GetCountAsync();
        Task<OrderItem> GetByIdAsync(int itemId);
        void Remove(OrderItem orderItem);
        void Add(OrderItem orderItem);
    }
}
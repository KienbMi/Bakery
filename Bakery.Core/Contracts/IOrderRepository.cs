using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakery.Core.Contracts
{
    public interface IOrderRepository
    {
        Task<int> GetCountAsync();
        Task<IEnumerable<OrderDto>> GetAllDtosAsync();
        Task<IEnumerable<OrderDto>> GetDtosByLastnameAsync(string filterLastName);
        void Add(Order order);
        Task<OrderWithItemsDto> GetByIdWithItemsAsync(int orderId);
        Task<Order> GetByIdAsync(int orderId);
        void Remove(Order orderInDb);
        Task<IEnumerable<OrderWithItemsDto>> GetAllWithItemsAsync();
    }
}
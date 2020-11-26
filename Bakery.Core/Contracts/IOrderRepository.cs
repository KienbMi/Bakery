using Bakery.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakery.Core.Contracts
{
    public interface IOrderRepository
    {
        Task<int> GetCountAsync();
        Task<IEnumerable<OrderDto>> GetAllDtosAsync();
        Task<IEnumerable<OrderDto>> GetDtosByLastnameAsync(string filterLastName);
    }
}
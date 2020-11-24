using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakery.Core.Contracts
{
    public interface IProductRepository
    {
        Task<int> GetCountAsync();
        Task AddRangeAsync(IEnumerable<Product> products);
        Task<IEnumerable<ProductDto>> GetWithFilterAsync(double priceFrom, double priceTo);
        void Add(Product product);
        Task<Product> GetByIdAsync(int productId);
    }
}
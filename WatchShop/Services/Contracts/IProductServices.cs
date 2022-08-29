using ShopOnline.Models.Dtos;

namespace WatchShop.Services.Contracts
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductDto>> GetItems();
        Task<ProductDto> GetItem(int id);
    }
}

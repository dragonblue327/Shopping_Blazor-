using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;

namespace WatchShop.Pages
{
    public class DisplayProductsBase:ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }
    }
}

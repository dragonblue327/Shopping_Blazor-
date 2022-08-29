using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class ShopingCartRepository : IShopingCartRepository
    {
        private readonly ShopOnlineDbContext shoponlineDbContext;

        public ShopingCartRepository(ShopOnlineDbContext shoponlineDbContext)
        {
            this.shoponlineDbContext = shoponlineDbContext;
        }
        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await this.shoponlineDbContext.CartItems.AnyAsync(c => c.CartId == cartId &&
                                                                     c.ProductId == productId);
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in this.shoponlineDbContext.Product
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = cartItemToAddDto.ProductId,
                                      Qty = cartItemToAddDto.Qty
                                  }).SingleOrDefaultAsync();
                if (item != null)
                {
                    var result = await this.shoponlineDbContext.CartItems.AddAsync(item);
                    await this.shoponlineDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;

        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await this.shoponlineDbContext.CartItems.FindAsync(id);
            if(item != null)
            {
                this.shoponlineDbContext.CartItems.Remove(item);
                await this.shoponlineDbContext.SaveChangesAsync();
            }
            return item;
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in this.shoponlineDbContext.Carts
                          join cartItem in this.shoponlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in this.shoponlineDbContext.Carts
                          join cartItem in this.shoponlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId,

                          }).ToListAsync();
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemToUpdateDto)
        {
            var item = await this.shoponlineDbContext.CartItems.FindAsync(id);
            if (item != null)
            {
                item.Qty = cartItemToUpdateDto.Qty;
                await this.shoponlineDbContext.SaveChangesAsync();
                return item;
            }
            return null;
        }
    }
}

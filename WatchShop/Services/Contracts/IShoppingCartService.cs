﻿using ShopOnline.Models.Dtos;

namespace WatchShop.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<List<CartItemDto>> GetItems(int userId);
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItemDto> DeleteItem(int id);
        Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);
        event Action<int> OnShoppingCartChange;
        void RaiseEventOnShoppingCartChanged(int totalQty);
    }
}

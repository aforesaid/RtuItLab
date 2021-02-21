using System.Linq;
using Shops.API.Models.ContextModels;
using Shops.API.Models.ViewModel;

namespace Shops.API.Helpers
{
    public static class ShopDTOTransformers
    {
        public static Shop ToShopDto(this ShopContext model)
        {
            if (model is null)
                return null;
            return new Shop
            {
                Id          = model.Id,
                Address     = model.Address,
                PhoneNumber = model.PhoneNumber,
                Products    = model.Products.Select(ToProductDto).ToList()
            };
        }
        public static Product ToProductDto(this ProductContext model)
        {
            if (model is null)
                return null;
            return new Product
            {
                ProductId = model.ProductId,
                Count = model.Count,
                Name = model.Name,
                Category = model.Category
            };
        }
    }
}
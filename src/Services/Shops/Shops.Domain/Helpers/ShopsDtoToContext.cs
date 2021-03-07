using RtuItLab.Infrastructure.Models.Shops;
using Shops.DAL.ContextModels;

namespace Shops.Domain.Helpers
{
    public static class ProductDtoToContext
    {
        public static ProductByReceiptContext ToProductByReceiptContext(this Product model)
        {
            if (model is null)
                return null;
            return new ProductByReceiptContext
            {
               Count = model.Count,
               Cost = model.Cost,
               Name = model.Name,
               Category = model.Category
            };
        }
    }
}

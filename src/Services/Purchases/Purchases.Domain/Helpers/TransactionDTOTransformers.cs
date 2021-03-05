using System.Linq;
using Purchases.DAL.ContextModels;
using RtuItLab.Infrastructure.Models.Purchases;
using RtuItLab.Infrastructure.Models.Shops;

namespace Purchases.Domain.Helpers
{
    public static class TransactionDtoTransformers
    {
        public static Transaction ToTransactionDto (this TransactionContext model)
        {
            if (model is null)
                return null;
            return new Transaction
            {
                Id = model.Id,
                Products        = model.Products?.Select(ToProductDto).ToList(),
                Receipt         = ToReceiptDto(model.Receipt),
                Date            = model.Date,
                TransactionType = model.TransactionType,
                IsShopCreate    = model.IsShopCreate
            };
        }
        public static Product ToProductDto(this ProductContext model)
        {
            if (model is null)
                return null;
            return new Product
            {
                Name = model.Name,
                ProductId = model.ProductId,
                Cost = model.Cost,
                Count = model.Count,
                Category = model.Category
            };
        }

        public static Receipt ToReceiptDto(this ReceiptContext model)
        {
            if (model is null)
                return null;
            return new Receipt
            {
                ShopId = model.ShopId,
                Cost = model.Cost,
                Count = model.Count,
                Date = model.Date
            };
        }
    }
}

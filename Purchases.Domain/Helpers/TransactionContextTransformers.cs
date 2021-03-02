using System.Linq;
using Purchases.DAL.ContextModels;
using RtuItLab.Infrastructure.Models.Purchases;
using RtuItLab.Infrastructure.Models.Shops;

namespace Purchases.Domain.Helpers
{
    public static class TransactionContextTransformers
    {
        public static TransactionContext ToTransactionContext(this Transaction model)
        {
            if (model is null)
                return null;
            return new TransactionContext
            {
                Products        = model.Products.Select(ToProductContext).ToList(),
                Receipt         = model.Receipt.ToReceiptContext(),
                Date            = model.Date,
                TransactionType = model.TransactionType,
                IsShopCreate    = model.IsShopCreate
            };
        }

        public static ProductContext ToProductContext(this Product model)
        {
            if (model is null)
                return null;
            return new ProductContext
            {
                Name      = model.Name,
                ProductId = model.ProductId,
                Cost      = model.Cost,
                Count     = model.Count,
                Category  = model.Category
            };
        }

        public static ReceiptContext ToReceiptContext(this Receipt model)
        {
            if (model is null)
                return null;
            return new ReceiptContext
            {
                ShopId = model.ShopId,
                Cost   = model.Cost,
                Count  = model.Count,
                Date   = model.Date
            };
        }
    }
}

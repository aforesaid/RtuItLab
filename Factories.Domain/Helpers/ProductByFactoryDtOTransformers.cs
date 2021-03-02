using Factories.DAL.ContextModels;
using RtuItLab.Infrastructure.Models.Shops;

namespace Factories.Domain.Helpers
{
    public static class ProductByFactoryDtoTransformers
    {
        public static ProductByFactory ToProductByFactoryDto(this ProductByFactoryContext model)
        {
            if (model is null)
                return null;
            return new ProductByFactory
            {
                ProductId = model.ProductId,
                ShopId = model.ShopId,
                Count = (int) model.Count
            };
        }
    }
}

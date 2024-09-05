using ShoeCatalog.Domain.ViewModels.Common;

namespace ShoeCatalog.Domain.ViewModels.Order
{
    public class OrderDetail
    {
        public Guid? Id { get; set; }
        public string? ItemName { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Total { get; set; }
        public DateTime? OrderDate { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public string? Carrier { get; set; }

    }
}

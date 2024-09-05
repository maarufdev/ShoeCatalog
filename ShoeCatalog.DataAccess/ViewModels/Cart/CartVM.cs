using ShoeCatalog.Domain.ViewModels.Common;
using ShoeCatalog.Domain.ViewModels.Order;

namespace ShoeCatalog.Domain.ViewModels.Cart;
public class CartVM
{
    public CartListVM? CartItem { get; set; } = new CartListVM();
    public IEnumerable<CartListVM> CartList { get; set; } = new List<CartListVM>();
    public CartUpsertVM? CartUpsertVM { get; set; } = new CartUpsertVM();
    public AppUserVM? User { get; set; } = new AppUserVM();
    public IEnumerable<OrderDetail> OrderDetailsList { get; set; } = new List<OrderDetail>();
}
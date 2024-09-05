using Microsoft.AspNetCore.Mvc;
using ShoeCatalog.Domain.Models;
using ShoeCatalog.Domain.ViewModels.Cart;
using ShoeCatalog.Domain.ViewModels.Common;
using ShoeCatalog.Repositories.Interfaces;
using System.Security.Claims;

namespace ShoeCatalog.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly List<CartListVM> _carts = new List<CartListVM>();
        private readonly Claim _userClaim;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _carts.AddRange(GetCarts());
            //_userClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
        }

        private static List<CartListVM> GetCarts()
        {
            var carts = new List<CartListVM>
            {
                new CartListVM
                {
                    Id = Guid.NewGuid(),
                    ShoeId = Guid.NewGuid().ToString(),
                    Name = "Black Mamba",
                    ImageFileName = "42c89a25-020c-48e3-868a-75022c712aa5.webp",
                    Brand = "Air Jordan",
                    Quantity = 5,
                    Price = 100.00,
                    Total = 500.00
                },
                new CartListVM
                {
                    Id = Guid.NewGuid(),
                    ShoeId = Guid.NewGuid().ToString(),
                    Name = "Air Jordan",
                    ImageFileName = "42c89a25-020c-48e3-868a-75022c712aa5.webp",
                    Brand = "Air Jordan",
                    Quantity = 2,
                    Price = 100.00,
                    Total = 200.00
                }
            };

            return carts;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> ProductCart()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cartResults = await _unitOfWork.CartRepository.GetAllAsync(
                                                        x => x.IsActive && x.AppUserId == claim.Value,
                                                        includeProperties: "Shoe",
                                                        tracked: false);

            var carts = new List<CartListVM>();

            foreach(var cart in cartResults)
            {
                var shoe = await _unitOfWork.ShoeRepository.GetFirstOrDefaultAsync(
                    s => s.Id == cart.ShoeId,
                    includeProperties: "Brand",
                    tracked: false);

                carts.Add(new CartListVM
                {
                    Id = cart.Id,
                    ShoeId = cart.ShoeId,
                    Name = shoe.Name,
                    ImageFileName = shoe.ImageFileName,
                    Brand = shoe.Brand.Name,
                    Quantity = cart.Quantity,
                    Price = cart.Price,
                    Total = cart.Total,
                    IsActive = cart.IsActive
                });
            }


            return Ok(carts);
        }
        public IActionResult GetCartDetail(string id)
        {
            return View();
        }

        [HttpPost("Cart/SaveCart")]
        public async Task<IActionResult> SaveCart(CartUpsertVM cart)
        {
            if(cart != null) 
            {
                bool result = false;
                var claim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);


                if (!string.IsNullOrWhiteSpace(cart.Id))
                {
                    var existingCart = await _unitOfWork.CartRepository.GetFirstOrDefaultAsync(c => c.Id == Guid.Parse(cart.Id));
                    if(existingCart != null)
                    {
                        existingCart.ShoeId = cart.ShoeId;
                        existingCart.Quantity = cart.Quantity;
                        existingCart.Price = cart.Price;
                        existingCart.Total = cart.Total;
                        existingCart.UpdatedOn = DateTime.UtcNow;

                        _unitOfWork.CartRepository.Update(existingCart);
                    }
                    
                } else
                {
                    Cart newCart = new()
                    {
                        Id = Guid.NewGuid(),
                        ShoeId = cart.ShoeId,
                        Quantity = cart.Quantity,
                        Price = cart.Price,
                        Total = cart.Total,
                        AppUserId = claim.Value,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = null
                    };

                    await _unitOfWork.CartRepository.AddAsync(newCart);
                }

                result = await _unitOfWork.SaveAsync();

                if (result) return Ok();
                else return BadRequest();
            }

            return BadRequest("Something went wrong");
        }

        [HttpPut("Cart/UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantity(Guid cartId, int qty)
        {
            var result = _carts.FirstOrDefault(i => i.Id == cartId);
            return Ok(await Task.Run(() => result));
        }

        [HttpPut("Cart/RemoveCart/{cartId}")]
        public async Task<IActionResult> RemoveCartItem(string cartId)
        {
            if (!string.IsNullOrWhiteSpace(cartId))
            {
                var existingCart = await _unitOfWork.CartRepository.GetFirstOrDefaultAsync(c => c.Id == Guid.Parse(cartId), tracked: true);

                if (existingCart is null) return BadRequest();

                existingCart.IsActive = false;
                existingCart.UpdatedOn = DateTime.UtcNow;

                var result = await _unitOfWork.SaveAsync();

                if(result) return Ok();
            }

            return BadRequest();
        }


        [HttpGet("Cart/Checkout/{cartId}")]
        public async Task<IActionResult> Checkout(string cartId)
       {
            var cartVM = new CartVM();

            var cart = await _unitOfWork.CartRepository.GetFirstOrDefaultAsync(c => c.Id == Guid.Parse(cartId), includeProperties: "Shoe", tracked: false);
            var brand = await _unitOfWork.BrandRepository.GetFirstOrDefaultAsync(b => b.Id == cart.Shoe.BrandId, tracked: false);
            
            if (cart != null)
            {
                var cartUpsertVM = new CartUpsertVM
                {
                    Id = cart.Id.ToString(),
                    ShoeId = cart.ShoeId,
                    Name = cart.Shoe.Name,
                    Brand = brand.Name,
                    Price = cart.Price,
                    Quantity = cart.Quantity,
                    Total = cart.Total,
                    IsActive = cart.IsActive
                };
                cartVM.CartUpsertVM = cartUpsertVM;

                return View(cartVM);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartVM cartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cart = cartVM.CartUpsertVM;
            var user = cartVM.User;

            Order order = new()
            {
                Id = Guid.NewGuid(),
                ShoeId = cart.ShoeId,
                AppUserId = claim.Value,
                Price = (decimal)cart.Price,
                Quantity = cart.Quantity,
                Total = (decimal)cart.Total,
                TrackingNumber = Guid.NewGuid(),
                Carrier = string.Empty,
                OrderDate = DateTime.UtcNow,
                StreetAddress = user.DeliveryAddress.StreetAddress,
                Building = user.DeliveryAddress.Building,
                District = user.DeliveryAddress.District,
                PostalCode = user.DeliveryAddress.PostalCode,
                City = user.DeliveryAddress.City,
                State = user.DeliveryAddress.State,
                ContactNumber = user.ContactNumber,
                OrderStatus = OrderStatus.Pending,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = null,
                IsActive = true
            };

            await _unitOfWork.OrderRepository.AddAsync(order);


            var toBeRemovedCart = await _unitOfWork.CartRepository.GetFirstOrDefaultAsync(c => c.Id == Guid.Parse(cart.Id), tracked: true);
            toBeRemovedCart.IsActive = false;
            toBeRemovedCart.UpdatedOn = DateTime.UtcNow;

            await _unitOfWork.CartRepository.Update(toBeRemovedCart);

            var orderResult = await _unitOfWork.SaveAsync();

            if (!orderResult) return RedirectToAction("Cart/Checkout/" + cart.Id);


            var domain = "https://localhost:44368";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{domain}/Cart/OrderConfirmation?id={Guid.NewGuid().ToString()}",
                CancelUrl = $"{domain}/Cart/Checkout/" + cartVM.CartUpsertVM.Id,
            };

            var sessionLineItem = new Stripe.Checkout.SessionLineItemOptions
            {
                PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)cart.Total,
                    Currency = "usd",
                    ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                    {
                        Name = cart.Name
                    },
                },
                Quantity = cart.Quantity
            };
            options.LineItems.Add(sessionLineItem);

            var stripeService = new Stripe.Checkout.SessionService();
            var session = await stripeService.CreateAsync(options);

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

        [HttpGet("Cart/GetShoeDetails/{shoeId}")]
        public async Task<IActionResult> GetDetails(string shoeId)
        {
            var shoe = await _unitOfWork.ShoeRepository.GetFirstOrDefaultAsync(
                                                s => s.Id == shoeId, 
                                                includeProperties: "Brand");

            var shoeCartDetail = new CartShoeDetails
            {
                ShoeId = shoe.Id,
                Name = shoe.Name,
                Brand = shoe.Brand.Name,
                Price = (double)shoe.Price,
            };

            return Ok(shoeCartDetail);
        }

        [HttpGet]
        public IActionResult OrderConfirmation([FromQuery] string id)
        {
            return View("OrderConfirmation", id);
        }
    }
}

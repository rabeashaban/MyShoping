using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Models;
using myshop.Entities.Repositories;
using myshop.Entities.ViewModels;
using myshop.Utilities;
using System.Security.Claims;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace myshop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public HomeController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public IActionResult Index(int? page)
        {
            var PageNumber = page ?? 1;
            int PageSize = 8;

            // استرجاع كل المنتجات مع الفئات وتطبيق الـPagination
            var products = _unitofwork.Product.GetAll(Includeword: "Category").ToPagedList(PageNumber, PageSize);

            ViewBag.cat = _unitofwork.Category.GetAll();

            ViewBag.prod = _unitofwork.Product.GetAll()
                .OrderByDescending(p => p.Id) 
                .Take(8)
                .ToList();
            ViewBag.prodTopPrice = _unitofwork.Product.GetAll()
            .OrderByDescending(p => p.Price) // ترتيب حسب السعر من الأعلى إلى الأقل
              .Take(8)
              .ToList();


            return View(products);
        }

        public IActionResult Allproducts(int? page)
        {
            var PageNumber = page ?? 1;
            int PageSize = 8;
            var products = _unitofwork.Product.GetAll().ToPagedList(PageNumber, PageSize);
            return View(products);
        }
        public IActionResult GetAllProducts()
        {
            var products = _unitofwork.Product.GetAll(Includeword: "Category");
            return PartialView("_ProductList", products); // التأكد من استخدام Partial View
        }
        public IActionResult FilterByCategory(string categoryName)
        {
            var products = _unitofwork.Product.GetAll(p => p.Category.Name == categoryName, Includeword: "Category");
            return PartialView("_ProductList", products); // التأكد من استخدام Partial View
        }



        public IActionResult Details(int ProductId)
        {
            ShoppingCart obj = new ShoppingCart()
            {
                ProductId = ProductId,
                Product = _unitofwork.Product.GetFirstorDefault(v => v.Id == ProductId, Includeword: "Category"),
                Count = 1
            };
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart Cartobj = _unitofwork.ShoppingCart.GetFirstorDefault(
                u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);

            if (Cartobj == null)
            {
                _unitofwork.ShoppingCart.Add(shoppingCart);
                _unitofwork.Complete();
                HttpContext.Session.SetInt32(SD.SessionKey,
                    _unitofwork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value).ToList().Count()
                   );

            }
            else
            {
                _unitofwork.ShoppingCart.IncreaseCount(Cartobj, shoppingCart.Count);
                _unitofwork.Complete();
            }


            return RedirectToAction("Index");
        }


    }
}

using Lab_11.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab_11.Controllers
{
    public class ShopController : Controller
    {
        Lab11Context _context;

        public ShopController(Lab11Context context)
        {
            _context = context;
        }
        // GET: ShopController
        public ActionResult Index(int? id)
        {
            if(TempData["CartMessage"] != null)
                ViewBag.CartMessage = TempData["CartMessage"].ToString();
            ViewBag.Categories = _context.Category.ToList();
            if(id == null)
                return View(_context.Article.ToList());
            else
            {
                return View(_context.Article.Where((ar) => ar.CategoryId == id).ToList());
            }
        }

        public ActionResult Refresh()
        {
            return Redirect("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int? id)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(7);
            var cookie = Request.Cookies.Where((cookie) => cookie.Key == id.ToString()).FirstOrDefault();
            if (!cookie.Equals(default(KeyValuePair<string, string>)))
                Response.Cookies.Append(cookie.Key, (int.Parse(cookie.Value) + 1).ToString(), cookieOptions);
            else
                Response.Cookies.Append(id.ToString(), "1", cookieOptions);
            TempData["CartMessage"] = $"Dodano produkt {_context.Article.Where(ar => ar.Id == id).First().Name}";
            return Redirect("index");
        }
    }
}

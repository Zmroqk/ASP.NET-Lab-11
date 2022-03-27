using Lab_11.Data;
using Lab_11.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab_11.Controllers
{
    public class CartController : Controller
    {
        Lab11Context _context;

        public CartController(Lab11Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Price = 0;
            if (Request.Cookies.Count == 0)
            {
                return View(new List<(Article article, int count)>());
            }
            ViewBag.Categories = _context.Category.ToList();
            var ids = Request.Cookies.Select(c => c.Key).ToList();
            var articles = _context.Article.Where((ar) => ids.Contains(ar.Id.ToString())).ToList();
            var articlesTuple = articles
                .Select(ar => (
                    article: ar,
                    count: int.Parse(Request.Cookies.Where(c => c.Key == ar.Id.ToString()).First().Value)
                )).ToList();
            articlesTuple.ForEach(ar => ViewBag.Price += ar.article.Price * ar.count);
            return View(articlesTuple);
        }

        public IActionResult AddItem(int? id)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(7);
            var cookie = Request.Cookies.Where((cookie) => cookie.Key == id.ToString()).FirstOrDefault();
            if (!cookie.Equals(default(KeyValuePair<string, string>)))
                Response.Cookies.Append(cookie.Key, (int.Parse(cookie.Value) + 1).ToString(), cookieOptions);
            return Redirect("/cart/index");
        }

        public IActionResult RemoveItem(int? id)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(7);
            var cookie = Request.Cookies.Where((cookie) => cookie.Key == id.ToString()).FirstOrDefault();
            if (!cookie.Equals(default(KeyValuePair<string, string>)) && int.Parse(cookie.Value) - 1 > 0)
                Response.Cookies.Append(cookie.Key, (int.Parse(cookie.Value) - 1).ToString(), cookieOptions);
            else if(!cookie.Equals(default(KeyValuePair<string, string>)))
                Response.Cookies.Delete(cookie.Key);
            return Redirect("/cart/index");
        }
    }
}

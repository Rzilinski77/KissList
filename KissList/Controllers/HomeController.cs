using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KissList.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace KissList.Controllers
{
    public class HomeController : Controller
    {
        SearchController searchcontroller;
        KissListController klcontroller = new KissListController();
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _config;
        private KissListDBContext db;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            searchcontroller = new SearchController(config);
        }

        public IActionResult Index()
        {
            return View();
        }

        // we make an async method that will call the SearchController
        // and return to us a Listing Object
        public async Task<IActionResult> LuckySearch()
        {
            Listing listing = await searchcontroller.GetLucky();
            return View(listing);
        }

        public async Task<IActionResult> GiftSearch(string occasion, string recipient, string minPrice, string maxPrice, string keyword)
        {
            List<Listing> giftList = await searchcontroller.Search(occasion, recipient, minPrice, maxPrice, keyword);
            return View(giftList);
        }

        [Authorize]
        public IActionResult AddWishList(string item, string description, string imageUrl, string url, string price)
        {
            db = new KissListDBContext();

            WishList wishList = new WishList();
            wishList.UserName = User.Identity.Name;
            wishList.Item = item;
            wishList.Description = description;
            wishList.ImageUrl = imageUrl;
            wishList.Url = url;
            wishList.Price = price;

            db.WishList.Add(wishList);
            db.SaveChanges();

            
                return View("MyWishList", db);
            //return View("index");

        }


        public IActionResult Privacy()
        {
            var user = User.Identity;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //this method searchs through the DB to find a valid user to display their wishlists
        public IActionResult SearchUser(string username)
        {
            KissListDBContext db = new KissListDBContext();

            var searchedUser = db.AspNetUsers.SingleOrDefault(u => u.UserName == username);
            List<WishList> list = new List<WishList>();
            foreach (WishList wishlist in db.WishList)
            {
               
                if (wishlist.UserName == username)
                {
                    list.Add(wishlist);
                    ViewBag.Name = username;
                }    
                                
            }
            if (list.Count == 0 || list == null)
            {
                return View("nameerror");
            }              
            return View("~/views/home/UserSearch.cshtml", list);
            
        }
        public IActionResult NameError()
        {
            return View();
        }

        public IActionResult RemoveItem(string user, string item)
        {
            db = new KissListDBContext();
            klcontroller.RemoveItem(user, item);

            return View("MyWishList", db);
        }

    }
}

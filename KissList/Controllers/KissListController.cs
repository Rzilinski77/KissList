using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using KissList.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KissList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KissListController : ControllerBase
    {
        private JsonDocument jDoc;
        private IConfiguration _config;
        KissListDBContext db = new KissListDBContext();
        public IActionResult RemoveItem(string user, string item)
        {
            foreach (WishList list in db.WishList)
            {
                if (user == User.Identity.Name && item == list.Item)
                {
                    db.WishList.Remove(list);
                    db.SaveChanges();
                   
                }

            }
            return RedirectToAction("MyWishList", db);

        }

    }
}


//private void RemoveItem(string user, string item)
//{
//    foreach (WishList list in db.WishList)
//    {
//        if (user == User.Identity.Name && item == list.Item)
//        {
//            db.WishList.Remove(list);
//            db.SaveChanges();


//        }

//    }

//}
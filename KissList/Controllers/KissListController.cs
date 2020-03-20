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
        KissListDBContext db = new KissListDBContext();
        public void RemoveItem(string user, string item)
        {
            WishList confirmedListing = new WishList();
            foreach (WishList listing in db.WishList)
            {
                if (user == listing.UserName && item == listing.Item)
                {
                    confirmedListing = listing;
                }
            }

            db.WishList.Remove(confirmedListing);
            db.SaveChanges();
        }
    }
}
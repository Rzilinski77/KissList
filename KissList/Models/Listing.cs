using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KissList.Models
{
    public class Listing
    {
        public string title { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
        public string description { get; set; }
        public string price { get; set; }

        public Listing() { }

        public Listing(string Title, string Url, string ImageUrl, string Description, string Price)
        {
            title = Title;
            url = Url;
            imageUrl = ImageUrl;
            description = Description;
            price = Price;
        }
    }


}

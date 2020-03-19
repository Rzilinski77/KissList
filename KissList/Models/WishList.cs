using System;
using System.Collections.Generic;

namespace KissList.Models
{
    public partial class WishList
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string Price { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoRestaurant.Models
{
    public class Client
    {
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<ClientDetail> ClientDetail { get; set; }
        public string status { get; set; }
        public Client() {
        }
    }
}
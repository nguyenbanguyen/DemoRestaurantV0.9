using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.NetworkInformation;
namespace DemoRestaurant.Models
{
    public class ClientDetail
    {
        public string ClientDetailId { get; set; }
        public string ClientId { get; set; }
        public string ClientIpAndress { get; set; }
        public DateTime ClientTime { get; set; }
        public string GoogleAndress { get; set; }
        public string DeviceName { get; set; }
        public string BrowserName { get; set; }
        public virtual Client Client { get; set; }
        public string MyProperty { get; set; }


    }
}
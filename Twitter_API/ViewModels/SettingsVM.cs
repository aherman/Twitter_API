using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class SettingsVM
    {
        public string username { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int countryID { get; set; }
        public string countryName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class RegistrationVM
    {
        public string username { get; set; }
        public string nickname{ get; set; }
        public DateTime birthDate { get; set; }
        public string email { get; set; }
        public int countryID { get; set; }
        public string password { get; set; }
    }
}
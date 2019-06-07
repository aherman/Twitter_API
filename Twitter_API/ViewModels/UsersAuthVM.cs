using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class UsersAuthVM
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
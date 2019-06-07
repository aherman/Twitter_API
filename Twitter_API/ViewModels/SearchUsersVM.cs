using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class SearchUsersVM
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string nickname { get; set; }
        public bool following { get; set; }
        public string profileImg { get; set; }
    }
}
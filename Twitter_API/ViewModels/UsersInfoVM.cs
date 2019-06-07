using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class UsersInfoVM
    {
        public int userID { get; set; }
        public int followersCount { get; set; }
        public int followingCount { get; set; }
        public string username { get; set; }
        public string nickname { get; set; }
        public string birthDate { get; set; }
        public string location { get; set; }
        public string profileImg { get; internal set; }
        public string headerImg { get; set; }
    }
}
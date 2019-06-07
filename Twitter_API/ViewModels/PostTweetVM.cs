using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class PostTweetVM
    {
        public int userID { get; set; }
        public string content { get; set; }
        public string image { get; set; }
    }
}
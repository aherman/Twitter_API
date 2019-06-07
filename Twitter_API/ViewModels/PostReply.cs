using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class PostReply
    {
        public int tweetID { get; set; }
        public int userID { get; set; }
        public string tweetContent { get; set; }
        public string image { get; set; }
    }
}
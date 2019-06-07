using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class TweetEngagementVM
    {
        public int tweetID { get; set; }
        public int userID { get; set; }
        public bool engaged { get; set; }
    }
}
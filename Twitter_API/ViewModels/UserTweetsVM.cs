using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class UserTweetsVM
    {
        public int userID { get; set; }
        public int tweetID { get; set; }
        public string username { get; set; }
        public string nickname { get; set; }
        public string content { get; set; }
        public DateTime datetime { get; set; }
        public int likeCount { get; set; }
        public int retweetCount { get; set; }
        public string profileImg { get; set; }
        public string parentTweetUsername { get; set; }
        public int replyCount { get; set; }
        public string replyUsername { get; set; }
        public int replyUserID { get; set; }
        public string imgTweet { get; set; }
    }
}
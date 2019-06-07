using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class MentionsVM
    {
        public string username;
        public string nickname;
        public string replyUsername;
        public int likeCount;
        public int retweetCount;
        public int replyCount;
        public string time;
        public string replyContent;
        public string imgProfile;
    }
}
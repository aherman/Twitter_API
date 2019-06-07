using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class EngagementsVM
    {
        public int tweetID { get; set; }
        public string tweetContent { get; set; }
        public string type { get; set; }
        public string imgProfile { get; set; }
        public string nickname { get; set; }
        public DateTime time { get; set; }
    }
}
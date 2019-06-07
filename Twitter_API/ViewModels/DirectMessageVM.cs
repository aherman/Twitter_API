using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class DirectMessageVM
    {
        public string nickname { get; set; }
        public int senderID { get; set; }
        public string content { get; set; }
        public DateTime dateTime { get; set; }
        public int conversationID { get; set; }
        public string dmImg { get; set; }
    }
}
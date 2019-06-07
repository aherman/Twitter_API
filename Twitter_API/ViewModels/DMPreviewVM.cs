using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Twitter_API.ViewModels
{
    public class DMPreviewVM
    {
        public int conversationID { get; set; }
        public string username { get; set; }
        public string nickname { get; set; }
        public string imgProfile { get; set; }
        public string content { get; set; }
        public DateTime time { get; set; }
        public bool isMineLastMessage { get; set; }
    }
}
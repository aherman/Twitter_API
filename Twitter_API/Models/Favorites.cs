//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Twitter_API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Favorites
    {
        public int FavoriteID { get; set; }
        public int TweetID { get; set; }
        public int UserID { get; set; }
        public System.DateTime FavoriteTime { get; set; }
    
        public virtual Tweets Tweets { get; set; }
        public virtual Users Users { get; set; }
    }
}

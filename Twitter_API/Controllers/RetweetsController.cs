using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twitter_API.Models;
using Twitter_API.ViewModels;

namespace Twitter_API.Controllers
{
    [MyAuthentication]
    public class RetweetsController : ApiController
    {
        private TwitterDBEntities db = new TwitterDBEntities();

        [HttpPost]
        public TweetEngagementVM PostRetweets(int tweetID, int userID)
        {
            Retweets retweet = db.Retweets.Where(x => x.TweetID == tweetID && x.UserID == userID).SingleOrDefault();

            if (retweet == null)
            {
                db.Retweets.Add(new Retweets
                {
                    TweetID = tweetID,
                    UserID = userID,
                    RetweetTime = DateTime.Now
                });
                db.SaveChanges();
                return new TweetEngagementVM
                {
                    tweetID = tweetID,
                    userID = userID,
                    engaged = true
                };
            }
            else
            {
                db.Retweets.Remove(retweet);
                db.SaveChanges();
                return new TweetEngagementVM
                {
                    tweetID = tweetID,
                    userID = userID,
                    engaged = false
                };
            }            
        }

        [HttpGet]
        public TweetEngagementVM GetUserRetweet(int userID, int tweetID)
        {
            Retweets retweet = db.Retweets.Where(x => x.TweetID == tweetID && x.UserID == userID).SingleOrDefault();

            if (retweet != null)
                return new TweetEngagementVM
                {
                    tweetID = tweetID,
                    userID = userID,
                    engaged = true
                };
            else
                return new TweetEngagementVM
                {
                    tweetID = tweetID,
                    userID = userID,
                    engaged = false
                };
        }
    }
}

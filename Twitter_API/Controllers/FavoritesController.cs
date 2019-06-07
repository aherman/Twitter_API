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
    public class FavoritesController : ApiController
    {
        private TwitterDBEntities db = new TwitterDBEntities();

        [HttpPost]
        public TweetEngagementVM PostFavorites(int userID, int tweetID)
        {
            Favorites favorite = db.Favorites.Where(x => x.TweetID == tweetID && x.UserID == userID).SingleOrDefault();

            if (favorite == null)
            {
                db.Favorites.Add(new Favorites
                {
                    TweetID = tweetID,
                    UserID = userID,
                    FavoriteTime = DateTime.Now
                });
                db.SaveChanges();
                return new TweetEngagementVM {
                    tweetID = tweetID,
                    userID = userID,
                    engaged = true
                };
            }
            else
            {
                db.Favorites.Remove(favorite);
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
        public TweetEngagementVM GetUserFavorite(int userID, int tweetID)
        {
            Favorites favorite = db.Favorites.Where(x => x.TweetID == tweetID && x.UserID == userID).SingleOrDefault();

            if (favorite != null)
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

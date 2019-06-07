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
    public class FavoriteTweetsController : ApiController
    {
        private TwitterDBEntities db = new TwitterDBEntities();

        [HttpGet]
        [Route("api/FavoriteTweets/{userID}")]
        public List<UserTweetsVM> GetFavoriteTweets(int userID)
        {
            List<UserTweetsVM> lista = db.tsp_FavoriteTweets(userID).Select(x => new UserTweetsVM
            {
                userID = (int)x.UserID,
                content = x.Content,
                datetime = (DateTime)x.TweetDate,
                likeCount = (int)x.likeCount,
                nickname = x.Nickname,
                profileImg = x.ProfileImage,
                retweetCount = (int)x.retweetCount,
                username = x.Username,
                tweetID = (int)x.TweetID,
                parentTweetUsername = x.ParentTweetUsername
            }).ToList();

            foreach (var item in db.tsp_ReplyCount().ToList())
            {
                foreach (var tweet in lista)
                {
                    if (tweet.tweetID == item.TweetID)
                    {
                        tweet.replyCount = (int)item.ReplyCount;
                        break;
                    }
                }
            }
            return lista;
        }

        [HttpGet]
        [Route("api/FavoriteTweets/Check/{userID}/{tweetID}")]
        public ResponseVM CheckFavoriteTweets(int userID, int tweetID)
        {
            FavoriteTweets obj = db.FavoriteTweets.Where(x => x.UserID == userID && x.TweetID == tweetID).SingleOrDefault();

            if (obj == null)
                return new ResponseVM
                {
                    responseCode = 200,
                    responseMessage = "Ok."
                };
            else
                return new ResponseVM
                {
                    responseCode = 400,
                    responseMessage = "You have already added tweet to favorites."
                };
        }



        [HttpPost]
        [Route("api/FavoriteTweets")]
        public ResponseVM PostFavoriteTweets(FavoriteTweetsVM obj)
        {
            try
            {
                db.FavoriteTweets.Add(new FavoriteTweets
                {
                    TweetID = obj.TweetID,
                    UserID = obj.UserID
                });
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return new ResponseVM
                {
                    responseCode = 400,
                    responseMessage = "An error occured."
                };
            }
            return new ResponseVM
            {
                responseCode = 200,
                responseMessage = "Ok."
            };
        }
    }
}

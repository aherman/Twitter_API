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
    public class MentionsController : ApiController
    {
        private TwitterDBEntities db = new TwitterDBEntities();

        [HttpGet]
        [Route("api/Mentions/{userID}")]
        public List<UserTweetsVM> GetMentions(int userID)
        {
            List<UserTweetsVM> lista = db.tsp_UserMentions(userID).Select(x => new UserTweetsVM
            {
                profileImg = x.ProfileImage,
                likeCount = (int)x.likeCount,
                nickname = x.Nickname,
                content = x.TweetContent,
                replyUsername = x.UsernameMention,
                retweetCount = (int)x.retweetCount,
                datetime = (DateTime) x.TweetDate,
                username = x.Username,
                tweetID = (int)x.tweetID,
                userID = (int)x.userID,
                replyUserID = x.userReplyID,
                imgTweet = x.TweetImg
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
            if (lista.Count == 0)
                return null;
            return lista;
        }
    }
}

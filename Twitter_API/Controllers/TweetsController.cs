using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twitter_API.Models;
using Twitter_API.ViewModels;

namespace Twitter_API.Controllers
{
    [MyAuthentication]
    public class TweetsController : ApiController
    {
        private TwitterDBEntities db = new TwitterDBEntities();

        [HttpGet]
        [Route("api/Tweets/{userID}")]
        public List<UserTweetsVM> GetTweets(int userID)
        {
            List<UserTweetsVM> lista = db.tsp_AllTweets(userID).Select(x => new UserTweetsVM
            {
                userID = (int)x.UserID,
                content = x.Content,
                datetime = (DateTime)x.TweetDate,
                likeCount = (int)x.likeCount,
                nickname = x.Nickname,
                profileImg = x.ProfileImage,
                retweetCount = (int)x.retweetCount,
                imgTweet = x.TweetImg,
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
        [Route("api/Tweets/Media/{userID}")]
        public List<string> GetUserMedia(int userID)
        {
            List<string> mediaList = new List<string>();

            foreach (var tweet in db.Tweets.Where(x => x.UserID == userID).ToList())
            {
                if (tweet.TweetImg != null)
                    mediaList.Add(tweet.TweetImg);
            }

            return mediaList;
        }

        [HttpGet]
        [Route("api/Tweets/Search/{word}")]
        public List<UserTweetsVM> SearchTweets(string word)
        {
            List<UserTweetsVM> lista = db.tsp_AllTweetsEveryone().Select(x => new UserTweetsVM
            {
                userID = (int)x.UserID,
                content = x.Content,
                datetime = (DateTime)x.TweetDate,
                likeCount = (int)x.likeCount,
                imgTweet = x.TweetImg,
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

            List<UserTweetsVM> lista2 = new List<UserTweetsVM>();

            foreach (UserTweetsVM item in lista)
            {
                try
                {
                    if (item.content.Contains(word))
                        lista2.Add(item);
                }
                catch (Exception e)
                {
                }
            }

            return lista2;
        }

        [HttpGet]
        [Route("api/Tweets/Trends")]
        public List<TrendsVM> GetTrends()
        {
            List<TrendsVM> myList = new List<TrendsVM>();
            DateTime now = DateTime.Now;
            List<Tweets> listTweets = db.Tweets.ToList();
            foreach (var item in listTweets)
            {
                if(item.TweetDate > now.AddHours(-72) && item.TweetDate <= now)
                {
                    string[] wordList = item.Content.Split(' ');
                    foreach (var item2 in wordList)
                    {
                        if (item2.Length < 4)
                            continue;

                        TrendsVM temp = db.tsp_WordCount(item2).Select(x => new TrendsVM
                        {
                            word = x.word,
                            count = (int)x.count
                        }).FirstOrDefault();

                        bool exists = false;
                        foreach (var myItem in myList)
                        {
                            if (item2.Equals(myItem.word))
                                exists = true;
                        }
                        if (!exists && temp.count != 0)
                            myList.Add(temp);
                    }
                }
                
            }
            return myList.OrderByDescending(x => x.count).Take(10).ToList();
        }

        [HttpGet]
        [Route("api/Tweets/User/{userID}")]
        public List<UserTweetsVM> GetUserTweets(int userID)
        {
            List<UserTweetsVM> lista = db.tsp_UserTweets(userID).Select(x => new UserTweetsVM
            {
                userID = (int)x.UserID,
                content = x.Content,
                datetime = (DateTime)x.TweetDate,
                likeCount = (int)x.likeCount,
                nickname = x.Nickname,
                imgTweet = x.TweetImg,
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
        [Route("api/Tweets/UserLiked/{userID}")]
        public List<UserTweetsVM> GetUserLikedTweets(int userID)
        {
            List<UserTweetsVM> lista = db.tsp_UserLikedTweets(userID).Select(x => new UserTweetsVM
            {
                userID = (int)x.UserID,
                content = x.Content,
                datetime = (DateTime)x.TweetDate,
                likeCount = (int)x.likeCount,
                nickname = x.Nickname,
                profileImg = x.ProfileImage,
                retweetCount = (int)x.retweetCount,
                imgTweet = x.TweetImg,
                username = x.Username,
                tweetID = (int)x.TweetID
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
        public List<EngagementsVM> GetEngagements(int userID)
        {
            return db.tsp_Engagements(userID).Select(x => new EngagementsVM
            {
                imgProfile = x.ProfileImage,
                nickname = x.Nickname,
                time = x.Time,
                tweetContent = x.Content,
                tweetID = x.TweetID,
                type = x.Type
            }).ToList();
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("api/Tweets")]
        public ResponseVM PostTweets(PostTweetVM obj)
        {
            Tweets tweet = new Tweets
            {
                UserID = obj.userID,
                Content = obj.content,
                TweetDate = DateTime.Now,
                TweetImg = obj.image
            };

            try
            {
                db.Tweets.Add(tweet);
                db.SaveChanges();
                return new ResponseVM
                {
                    responseCode = 200,
                    responseMessage = "Tweet posted successfully."
                };
            }
            catch (Exception)
            {
                return new ResponseVM
                {
                    responseCode = 400,
                    responseMessage = "An error occurred."
                };
            } 
        }

        [HttpDelete]
        [AcceptVerbs("DELETE")]
        [Route("api/Tweets/{tweetID}")]
        public ResponseVM DeleteTweets(int tweetID)
        {
            try
            {
                Tweets obj = db.Tweets.Find(tweetID);
                foreach (var favorite in db.Favorites.Where(x=>x.TweetID == tweetID).ToList())
                { 
                    db.Favorites.Remove(favorite);
                }
                foreach (var retweet in db.Retweets.Where(x => x.TweetID == tweetID).ToList())
                {
                    db.Retweets.Remove(retweet);
                }
                foreach (var mention in db.Mentions.Where(x => x.TweetID == tweetID ||
                    x.TweetMentionID == tweetID).ToList())
                {
                    db.Mentions.Remove(mention);
                }
                foreach (var favoriteTweet in db.FavoriteTweets.Where(x => x.TweetID == tweetID).ToList())
                {
                    db.FavoriteTweets.Remove(favoriteTweet);
                }
                db.Tweets.Remove(obj);
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
                responseMessage = "Tweet deleted successfully."
            };
        }

        [HttpGet]
        [Route("api/Tweets/Tweet/{tweetID}")]
        public UserTweetsVM GetTweet(int tweetID)
        {
            UserTweetsVM obj = db.tsp_GetTweet(tweetID).Select(x => new UserTweetsVM
            {
                content = x.Content,
                datetime = (DateTime)x.TweetDate,
                likeCount = (int)x.likeCount,
                nickname = x.Nickname,
                parentTweetUsername = x.ParentTweetUsername,
                profileImg = x.ProfileImage,
                retweetCount = (int)x.retweetCount,
                tweetID = (int)x.TweetID,
                userID = (int)x.UserID,
                imgTweet = x.TweetImg,
                username = x.Username
            }).SingleOrDefault();

            foreach (var item in db.tsp_ReplyCount().ToList())
            {
                    if (obj.tweetID == item.TweetID)
                    {
                        obj.replyCount = (int)item.ReplyCount;
                        break;
                    }
            }
            return obj;
        }

        [HttpPost]
        [Route("api/Tweets/Reply")]
        public ResponseVM PostReply(PostReply obj)
        {
            Tweets tweet = new Tweets
            {
                UserID = obj.userID,
                Content = obj.tweetContent,
                TweetDate = DateTime.Now,
                TweetImg = obj.image
            };

            try
            {
                db.Tweets.Add(tweet);
                db.SaveChanges();

                Mentions mention = new Mentions
                {
                    TweetID = obj.tweetID,
                    TweetMentionID = tweet.TweetID,
                    UserID = obj.userID
                };

                db.Mentions.Add(mention);
                db.SaveChanges();

                return new ResponseVM
                {
                    responseCode = 200,
                    responseMessage = "Reply posted successfully."
                };
            }
            catch (Exception)
            {
                return new ResponseVM
                {
                    responseCode = 400,
                    responseMessage = "An error occurred."
                };
            }
        }

    }
}

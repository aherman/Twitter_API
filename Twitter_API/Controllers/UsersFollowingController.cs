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
    public class UsersFollowingController : ApiController
    {
        private TwitterDBEntities db = new TwitterDBEntities();

        [HttpPost]
        [Route("api/UsersFollowing/Follow/{followerID}/{followingID}")]
        public ResponseVM Follow(int followerID, int followingID)
        {
            UsersFollowing obj = new UsersFollowing()
            {
                DateModified = DateTime.Now,
                FollowerID = followerID,
                FollowingID = followingID
            };
            
            if(obj == null)
                return new ResponseVM
                {
                    responseCode = 400,
                    responseMessage = "An error occured."
                };

            db.UsersFollowing.Add(obj);
            db.SaveChanges();

            return new ResponseVM
            {
                responseCode = 200,
                responseMessage = "Ok."
            };
        }

        [HttpGet]
        [Route("api/UsersFollowing/IsFollowing/{followerID}/{followingID}")]
        public bool IsFollowing(int followerID, int followingID)
        {
            UsersFollowing obj = db.UsersFollowing.Where(x => x.FollowerID == followerID && x.FollowingID == followingID).SingleOrDefault();

            return obj != null;
        }
        
        [HttpDelete]
        [Route("api/UsersFollowing/Unfollow/{followerID}/{followingID}")]
        public ResponseVM Unfollow(int followerID, int followingID)
        {
            UsersFollowing obj = db.UsersFollowing.Where(x => x.FollowerID == followerID &&
            x.FollowingID == followingID).SingleOrDefault();

            if (obj == null)
                return new ResponseVM
                {
                    responseCode = 400,
                    responseMessage = "An error occured."
                };

            db.UsersFollowing.Remove(obj);
            db.SaveChanges();

            return new ResponseVM
            {
                responseCode = 200,
                responseMessage = "Ok."
            };
        }

    }
}

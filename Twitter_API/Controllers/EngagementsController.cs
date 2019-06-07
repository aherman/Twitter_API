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
    public class EngagementsController : ApiController
    {
        private TwitterDBEntities db = new TwitterDBEntities();

        [HttpGet]
        [Route("api/Engagements/{userID}")]
        public List<EngagementsVM> GetEngagements(int userID)
        {
            List<EngagementsVM> lista = db.tsp_Engagements(userID).Select(x => new EngagementsVM
            {
                tweetID = x.TweetID,
                imgProfile = x.ProfileImage,
                nickname = x.Nickname,
                time = x.Time,
                tweetContent = x.Content,
                type = x.Type
            }).ToList();

            if (lista.Count == 0)
                return null;
            return lista;
        }
    }
}
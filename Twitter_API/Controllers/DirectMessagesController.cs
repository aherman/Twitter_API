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
    public class DirectMessagesController : ApiController
    {
        private TwitterDBEntities db = new TwitterDBEntities();

        [HttpGet]
        [Route("api/DirectMessages/Preview/{myUserID}")]
        public List<DMPreviewVM> GetDMPreview(int myUserID)
        {
            List<DMPreviewVM> lista = new List<DMPreviewVM>();
            foreach (var user in db.Users.ToList())
            {
                Conversations conversation = db.Conversations
                .Where(x => x.User1ID == myUserID && x.User2ID == user.UserID).SingleOrDefault();
                if (conversation == null)
                    conversation = db.Conversations
                    .Where(x => x.User1ID == user.UserID && x.User2ID == myUserID).SingleOrDefault();
                if (conversation != null)
                {
                    int usrID = -1;
                    if (conversation.User1ID == myUserID)
                        usrID = conversation.User2ID;
                    else
                        usrID = conversation.User1ID;

                    Users usr = db.Users.Find(usrID);

                    try
                    {
                        DMPreviewVM listItem = db.tsp_LatestMessage(conversation.User1ID, conversation.User2ID).Select(x => new DMPreviewVM
                        {
                            conversationID = conversation.ConversationID,
                            nickname = usr.Nickname,
                            username = usr.Username,
                            content = x.Content,
                            time = x.DateCreated,
                            imgProfile = usr.ProfileImage
                        }).SingleOrDefault();

                        if (db.tsp_LatestMessage(conversation.User1ID, conversation.User2ID).SingleOrDefault().SenderID == myUserID)
                            listItem.isMineLastMessage = true;
                        else
                            listItem.isMineLastMessage = false;

                        lista.Add(listItem);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return lista.OrderByDescending(x => x.time).ToList();
        }

        [HttpGet]
        [Route("api/DirectMessages")]
        public List<DirectMessageVM> GetDirectMessages(int conversationID, int currentUser)
        {
            List<DirectMessageVM> lista =  db.tsp_getConversationDM(conversationID).Select(x => new DirectMessageVM
            {
                senderID = x.SenderID,
                content = x.Content,
                dateTime = x.DateCreated
            }).ToList();

            Conversations conversation = db.Conversations.Find(conversationID);

            int otherUserID = conversation.User1ID;
            if (currentUser == conversation.User1ID)
                otherUserID = conversation.User2ID;

            Users user = db.Users.Find(otherUserID);

            foreach (var item in lista)
            {
                item.dmImg = user.ProfileImage;
            }

            return lista;
        }

        [HttpPost]
        [Route("api/DirectMessages/Conversation/{userID1}/{userID2}")]
        public int PostConversation(int userID1, int userID2)
        {
            Conversations conversation = new Conversations()
            {
                User1ID = userID1,
                User2ID = userID2
            };

            try
            {
                db.Conversations.Add(conversation);
                db.SaveChanges();
                return conversation.ConversationID;
            } catch(Exception e)
            {
                return 0;
            }
        }

        [HttpGet]
        [Route("api/DirectMessages/CheckConversation/{userID1}/{userID2}")]
        public int GetConversation(int userID1, int userID2)
        {
            Conversations conversation = db.Conversations.Where(x => (x.User1ID == userID1 && x.User2ID == userID2)
            || (x.User1ID == userID2 && x.User2ID == userID1)).SingleOrDefault();

            if (conversation != null)
                return conversation.ConversationID;
            else
                return 0;
        }

        [HttpPost]
        [Route("api/DirectMessages")]
        public ResponseVM PostDirectMessages(DirectMessageVM obj)
        {
            Conversations conversation = db.Conversations.Find(obj.conversationID);

            if (conversation == null)
                return new ResponseVM
                {
                    responseCode = 404,
                    responseMessage = "Not found."
                };

            int receiverID = conversation.User1ID;
            if (obj.senderID == conversation.User1ID)
                receiverID = conversation.User2ID;

            DirectMessages directMessage = new DirectMessages()
            {
                ConversationID = obj.conversationID,
                SenderID = obj.senderID,
                ReceiverID = receiverID,
                Content = obj.content,
                DateCreated = DateTime.Now
            };

            try
            {
                db.DirectMessages.Add(directMessage);
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
                responseMessage = "Direct message posted successfully"
            };
        }
    }
}

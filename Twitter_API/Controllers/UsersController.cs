using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twitter_API.Models;
using Twitter_API.ViewModels;
using System.Data.Entity;

namespace Twitter_API.Controllers
{
    public class UsersController : ApiController
    {
        private TwitterDBEntities db = new TwitterDBEntities();

        [HttpGet]
        [MyAuthentication]
        [Route("api/Users/ProfileImg/{userID}")]
        public string GetProfileImg(int userID)
        {
            return db.Users.Find(userID).ProfileImage;
        }

        [HttpGet]
        [MyAuthentication]
        [Route("api/Users/{userID}")]
        public UsersInfoVM GetUserInfo(int userID)
        {
            Users user = db.Users.Where(x => x.UserID == userID).SingleOrDefault();
            return new UsersInfoVM {
                userID = user.UserID,
                birthDate = user.BirthDate.ToLongDateString(),
                location = db.Countries.Where(x=>x.CountryID == user.CountryID).SingleOrDefault().CountryName,
                nickname = user.Nickname,
                username = user.Username,
                followersCount = db.UsersFollowing.Where(x=>x.FollowingID == userID).Count()-1,
                followingCount = db.UsersFollowing.Where(x=>x.FollowerID == userID).Count()-1,
                profileImg = user.ProfileImage,
                headerImg = user.HeaderImage
            };
        }

        [HttpGet]
        [MyAuthentication]
        [Route("api/Users/GetEditData/{userID}")]
        public UsersEditVM GetEditUserInfo(int userID)
        {
            Users user = db.Users.Where(x => x.UserID == userID).SingleOrDefault();
            return new UsersEditVM
            {
                nickname = user.Nickname,
                imageProfile = user.ProfileImage,
                imageHeader = user.HeaderImage
            };
        }

        [HttpGet]
        [Route("api/Users/Authentication")]
        public UsersAuthVM Authentication(string username, string password)
        {
            UsersAuthVM userVM = db.Users.Where(x => x.Username == username && password == x.Password).Select(x => new UsersAuthVM()
            {
                username = x.Username,
                password = x.Password,
                userID = x.UserID
            })
            .SingleOrDefault();

            if(userVM != null)
            {
                Users user = db.Users.Find(userVM.userID);
                if (user.Active == false)
                {
                    user.Active = true;
                    db.SaveChanges();
                }
            }

            return userVM;
        }

        [HttpGet]
        [MyAuthentication]
        [Route("api/Users/AuthSession/{username}")]
        public UsersAuthVM AuthSession(string username)
        {
            return db.Users.Where(x => x.Username == username).Select(x => new UsersAuthVM()
            {
                username = x.Username,
                password = x.Password,
                userID = x.UserID
            })
            .SingleOrDefault();
        }

        [HttpGet]
        [MyAuthentication]
        [Route("api/Users/AuthRegister")]
        public UsersAuthVM AuthnRegister(string username)
        {
            UsersAuthVM userVM = db.Users.Where(x => x.Username == username).Select(x => new UsersAuthVM()
            {
                username = x.Username,
                password = x.Password,
                userID = x.UserID
            })
            .SingleOrDefault();

            return userVM;
        }

        [HttpPost]
        [Route("api/Users")]
        public IHttpActionResult PostUser(RegistrationVM obj)
        {
            Users newUser = new Users()
            {
                Active = true,
                BirthDate = obj.birthDate,
                CountryID = obj.countryID,
                Email = obj.email,
                Password = obj.password,
                Nickname = obj.nickname,
                Username = obj.username
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            UsersFollowing firstFollowing = new UsersFollowing()
            {
                DateModified = DateTime.Now,
                FollowerID = newUser.UserID,
                FollowingID = newUser.UserID
            };

            db.UsersFollowing.Add(firstFollowing);
            db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [MyAuthentication]
        [Route("api/Users/Settings/{userID}")]
        public SettingsVM GetUserSettings(int userID)
        {
            return db.Users
                .Include(x=>x.Countries)
                .Where(x=>x.UserID == userID)
                .Select(x => new SettingsVM
            {
                countryID = x.CountryID,
                email = x.Email,
                password = x.Password,
                phone = x.Phone,
                username = x.Username,
                countryName = x.Countries.CountryName
            }).SingleOrDefault();
        }

        [HttpGet]
        [MyAuthentication]
        [Route("api/Users/Search/{userID}/{word}")]
        public List<SearchUsersVM> SearchUsers(string word, int userID)
        {
            List<SearchUsersVM> lista = db.Users
                .Where(x => x.Username.Contains(word) || x.Nickname.Contains(word))
                .Select(x => new SearchUsersVM
                {
                    userID = x.UserID,
                    nickname = x.Nickname,
                    username = x.Username,
                    following = false,
                    profileImg = x.ProfileImage
                }).ToList();

            SearchUsersVM myProfile = null;

            foreach (var item in lista)
            {
                UsersFollowing temp = db.UsersFollowing
                    .Where(x => x.FollowingID == item.userID && x.FollowerID == userID).FirstOrDefault();
                if (temp != null)
                    item.following = true;
                if (item.userID == userID)
                    myProfile = item;
            }
            if (myProfile != null)
                lista.Remove(myProfile);
            return lista;
        }

        [HttpGet]
        [MyAuthentication]
        [Route("api/Users/CheckUsername/{username}")]
        public bool CheckUsername(string username)
        {
            foreach (Users user in db.Users.ToList())
            {
                if (user.Username.Equals(username))
                    return false;
            }

            return true;
        }

        [HttpPut]
        [MyAuthentication]
        [Route("api/Users/{userID}")]
        public IHttpActionResult EditUser(UsersEditVM obj, int userID)
        {
            Users user = db.Users.Find(userID);
            if (user == null)
                return NotFound();

            try
            {
                user.Nickname = obj.nickname;
                if (obj.imageProfile != null)
                    user.ProfileImage = obj.imageProfile;
                if (obj.imageHeader != null)
                    user.HeaderImage = obj.imageHeader;

                db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut]
        [MyAuthentication]
        [Route("api/Users/ProfileUpdate/{userID}")]
        public ResponseVM ProfileUpdate(SettingsVM obj, int userID)
        {
            Users user = db.Users.Find(userID);

            user.Username = obj.username;
            if (!string.IsNullOrWhiteSpace(obj.phone))
                user.Phone = obj.phone;
            else
                user.Phone = null;
            user.Email = obj.email;
            user.Password = obj.password;
            user.CountryID = obj.countryID;

            db.SaveChanges();

            return new ResponseVM
            {
                responseCode = 200,
                responseMessage = "Ok."
            };
        }

        [HttpPatch]
        [MyAuthentication]
        [Route("api/Users/Deactivate/{userID}")]
        public ResponseVM Deactivate(SettingsVM obj, int userID)
        {
            try
            {
                Users user = db.Users.Find(userID);
                user.Active = false;
                db.SaveChanges();
                return new ResponseVM
                {
                    responseCode = 200,
                    responseMessage = "User account deactivated successfully."
                };
            }
            catch(Exception e)
            {
                return new ResponseVM
                {
                    responseCode = 400,
                    responseMessage = "An error occured."
                };
            }
        }
    }
}

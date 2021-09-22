using Authorization_Service.Data;
using Authorization_Service.Models;
using Authorization_Service.Repository.IRepository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorization_Service.Repository
{
    
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }
        public UserModel Authenticate(string username, string password)
        {
            var user = _db.Users.SingleOrDefault(x=> x.UserName == username && x.Password ==password);

            if (user == null) {
                return null;
            }
            else
            {
                //we will generte a Jwt token.

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),

                }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials
                                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);


                user.Password = "";
                return user;
            }
        }

        public bool isUserUnique(string username)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == username);

            // return null if user not found
            if (user == null)
                return true;

            return false;
        }

        public UserModel Register(string username, string password)
        {
            UserModel userObj = new UserModel()
            {
                UserName = username,
                Password = password,
            };

            _db.Users.Add(userObj);
            _db.SaveChanges();
            userObj.Password = "";
            return userObj;
        }
    }
}

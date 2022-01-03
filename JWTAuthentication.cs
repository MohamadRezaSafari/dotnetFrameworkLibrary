using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Web.Http;
using System.Net.Http;
using System.Net;

namespace Providers
{
    /***
     *
     * public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class UserRepository
        {
            public List<User> TestUsers;

            public UserRepository()
            {
                TestUsers = new List<User>();
                TestUsers.Add(new User() {Username = "Test1", Password = "Pass1"});
                TestUsers.Add(new User() {Username = "Test2", Password = "Pass2"});
            }

            public User GetUser(string username)
            {
                try
                {
                    return TestUsers.First(user => user.Username.Equals(username));
                }
                catch
                {
                    return null;
                }
            }

        }

     *[HttpPost]
        [Route("validate")]
        public IHttpActionResult Validate(JObject data)
        {
            string username = data["username"].Value<string>();
            string token = data["token"].Value<string>();

            bool exists = new UserRepository().GetUser(username) != null;
            if (!exists) return NotFound();
            string tokenUsername = JWTAuthentication.ValidateToken(token);
            return Ok(tokenUsername);
        }


        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(User user)
        {
            User u = new UserRepository().GetUser(user.Username);
            if (u == null)
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    "The user was not found.");
            bool credentials = u.Password.Equals(user.Password);
            if (!credentials) return Request.CreateResponse(HttpStatusCode.Forbidden,
                "The username/password combination was wrong.");
            return Request.CreateResponse(HttpStatusCode.OK,
                JWTAuthentication.GenerateToken(user.Username));
        }
     *
     */
    public class JWTAuthentication
    {
        static HMACSHA256 hmac = new HMACSHA256();
        string key = Convert.ToBase64String(hmac.Key);
        private static string Secret = "RSRwcCtLZyU5dmVxKmheIzpJcihlczgxeCZLWHVCaHZYPFtUXF43O0d+Rl1Aelw3bmF8MGlAOnwtfUkzL3gjZkk9MSooSXdlKD5ickpgZUpZISFSMn5pZ1E0eSJ1bF5oUldyRVohJll4L0xIPTMuRnV5STc9YG8pJzJUbCJbcXRAJiprLCQvSmRePFFIfDJbc1F+d34rNi1MbTBvPShEe2VZUjt1UGtLXV86WFtuciYlfVFwQXkzXmJoPWJJMWdNfDgiNlNNcTBbblVHOCwySSQ9WjEwa054ZVNZTG9JblNeLi5kLG82R3JLP3BuPT06Qn0zUTkxZVMibS9pbVZaMEBbaUoyPkNjLG1TJCJtPiJnZXwsKnJASEJOPHROdmtdZE02djYvOHZzYFhKTHNUQUJ1VDosUGZ7W2hWNHdafC8wW1VnTGg3MXF9RURTKlZFRWBDdX5DXD9qOGhUOysqPFVdbFZRVkhsQlFWQT8zTGZyTSQ6ZDh5e31+KlZxfntRYy8zcFVDTXwpczRpJlBYNwo=";

        public static string GenerateToken(string username)
        {
            try
            {
                byte[] key = Convert.FromBase64String(Secret);
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name, username)}),
                    Expires = DateTime.UtcNow.AddSeconds(Convert.ToInt32(10)),
                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                };
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
                return handler.WriteToken(token);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }


        public static string ValidateToken(string token)
        {
            try
            {
                string username = null;
                ClaimsPrincipal principal = GetPrincipal(token);
                if (principal == null)
                    return null;
                ClaimsIdentity identity = null;
                try
                {
                    identity = (ClaimsIdentity)principal.Identity;
                }
                catch (NullReferenceException)
                {
                    return null;
                }
                Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
                username = usernameClaim.Value;  
                return username;
            }
            catch 
            {
                return null; //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Oops!!!" });
            }
        }
    }
}
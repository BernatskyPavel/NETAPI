using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Clients;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.Rest.Serialization;
using Microsoft.Rest;
using MongoDB.Bson;
using Newtonsoft.Json;
using NETAPI.Models;
using MongoDB.Driver;
using System.Configuration;
using System.Numerics;
using Isopoh.Cryptography.Argon2;
using System.Text;
using NETAPI.Properties;
using System.Web;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace NETAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        MongoContext context = new MongoContext();
        // GET: api/User
       /* [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<string> Get(string id)
        {
            User user = await context.Users.Find(filter: new BsonDocument("_id", new BsonObjectId(new ObjectId(id)))).FirstOrDefaultAsync();
            if (user != null)
                await user.SetTours(context);
            return JsonConvert.SerializeObject(user);
        }

        // POST: api/User
        [HttpPost]
        public async Task Post([FromBody] User user)
        {
            try
            {
                new MailAddress(user.Email);
                User users = await context.Users.Find(filter: new BsonDocument("email", user.Email)).FirstOrDefaultAsync();
                if (users != null)
                {
                    Response.StatusCode = 400;
                    await Response.WriteAsync("User with such email exist.");
                }
                else
                {
                    Argon2Config config = new Argon2Config();
                    config.Type = Argon2Type.DataIndependentAddressing;
                    config.Version = Argon2Version.Nineteen;
                    config.Salt = Encoding.Default.GetBytes(Resources.LocalSalt);
                    config.Password = Encoding.Default.GetBytes(user.Password);
                    user.HPass = Argon2.Hash(config);
                    await context.Users.InsertOneAsync(user);
                    new StatusCodeResult(200);
                }
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                await HttpContext.Response.WriteAsync(exception.Message.ToString());
            }

        }

        [HttpGet]
        public async Task Get([FromQuery]string Email, [FromQuery]string Password)
        {
            try
            {
                User user = await context.Users.Find(filter: new BsonDocument("email", new BsonString(Email))).FirstOrDefaultAsync();
                if (user == null) throw new Exception("No User with such Email");
                Argon2Config config = new Argon2Config();
                config.Type = Argon2Type.DataIndependentAddressing;
                config.Version = Argon2Version.Nineteen;
                config.Salt = Encoding.Default.GetBytes(Resources.LocalSalt);
                config.Password = Encoding.Default.GetBytes(Password);
                string hPassword = Argon2.Hash(config);
                if (user.HPass.Equals(hPassword))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
                    };
                    ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);


                    var now = DateTime.UtcNow;
                    // создаем JWT-токен
                    var jwt = new JwtSecurityToken(
                            issuer: AuthOptions.ISSUER,
                            audience: AuthOptions.AUDIENCE,
                            notBefore: now,
                            claims: claimsIdentity.Claims,
                            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                    var response = new
                    {
                        access_token = encodedJwt,
                        username = claimsIdentity.Name
                    };

                    // сериализация ответа
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
                }
                else
                {
                    Response.StatusCode = 400;
                    await Response.WriteAsync("Wrong Password");
                }
            }
            catch (Exception exception)
            {
                Response.StatusCode = 404;
                await HttpContext.Response.WriteAsync(exception.Message.ToString());
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

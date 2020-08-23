using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CheckoutProj.Auth;
using CheckoutProj.Models;
using BC = BCrypt.Net.BCrypt;

namespace TokenApp.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthorizationController : Controller
    {
        private UserRepository UserRep;
        public AuthorizationController(DatabaseContext context)
        {
            UserRep = new UserRepository(context);
        }

        [HttpPost("/login")]
        public IActionResult Login(string email, string password)
        {
            var identity = GetIdentity(email, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name,
                role = identity.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value
            };

            return Json(response);
        }

        [HttpPost("/register")]
        public IActionResult Register(string email, string password, string name, string surname)
        {
            User user = new User();
            user.Name = name;
            user.PasswordHash = BC.HashPassword(password);
            user.EMail = email;
            user.Surname = surname;
            UserRep.List.Add(user);
            UserRep.DbContext.SaveChanges();
            return Login(email, password);
        }

        private ClaimsIdentity GetIdentity(string email, string password)
        {
            User user = UserRep.FindByEmail(email);
            if (user != null && BC.Verify(password, user.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.EMail),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
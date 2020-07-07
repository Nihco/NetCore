using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Propins.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using RestSharp;

namespace Propins.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
       
        private readonly IConfiguration configuration;

        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private string GenerarTokenJwt(LoginModel login)
        {
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:ClaveSecreta"]));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            JwtHeader header = new JwtHeader(signingCredentials);

            Claim[] claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, login.id.ToString()),
                new Claim("nombre", login.name),
                new Claim("apellidos", login.lastName)
            };

            JwtPayload payload = new JwtPayload(
                  issuer: configuration["JWT:Issuer"],
                  audience: configuration["JWT:Audience"],
                  claims: claims,
                  notBefore: DateTime.UtcNow,
                  // Exipra a la 24 horas.
                  expires: DateTime.UtcNow.AddHours(24)
              );

            JwtSecurityToken token = new JwtSecurityToken(
                  header,
                  payload
              );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private List<LoginModel> GetUsers()
        {
            RestClient client = new RestClient("http://arsene.azurewebsites.net/UserData");
            client.Timeout = -1;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", "ARRAffinity=b7654489a1cd63362ae25dd5c2faf0e50d856890f952add19099d48cf360ebc6");
            IRestResponse response = client.Execute(request);
            List<LoginModel> usuariosApi = JsonConvert.DeserializeObject<List<LoginModel>>(response.Content);

            return usuariosApi;
        }
        
        [AllowAnonymous]
        public LoginModel Autenticar(string username, string password)
        {
            string secret = string.Empty;
            LoginModel user = GetUsers().SingleOrDefault(x => x.username == username && x.password == password);

            if (user==null)
            {
                return null;
            }
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                  new Claim(ClaimTypes.Name,user.id.ToString()),   
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.token = tokenHandler.WriteToken(token);

            user.password = null;
            return user;
        }
        
    }
}



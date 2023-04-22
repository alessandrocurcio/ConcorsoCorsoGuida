using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ConcorsoCorsoGuidaEntities;
using ConcorsoCorsoGuidaRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Security.Cryptography;

namespace ConcorsoCorsoGuidaManager.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        User GetUserInfoByToken(AuthenticateRequest model);
        User GetById(int id);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AppSettings _appSettings;

        public UserService(ApplicationDbContext applicationDbContext, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {

            var user = _applicationDbContext.Users.SingleOrDefault(x => x.Username == model.Username);
                        
            if (user == null || user.Password != ComputeSha256Hash(model.Password)) throw new Exception("Credenziali non corrette");
            if (!user.Enabled) throw new Exception("Utente non autorizzato, contattare l'amministratore");

            user.Password = String.Empty;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }
        public User GetUserInfoByToken(AuthenticateRequest model)
        {

            if (string.IsNullOrEmpty(model.Token))
            {
                return new User();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            tokenHandler.ValidateToken(model.Token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            return GetById(userId);
        }

        public User GetById(int id)
        {
            return _applicationDbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData + "3R1K"));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

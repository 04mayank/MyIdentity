using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyIdentity.DataAccessLayer;
using MyIdentity.EntityLayer;

namespace MyIdentity.BusinessLayer
{
    public class TokenLogic : ITokenLogic
    {
        readonly private ITokenStrategy _dalLayer;
        private readonly UserManagerStrategy _userManager;
        private readonly AppSetting _appSettings;

        public TokenLogic(ITokenStrategy dalLayer, UserManagerStrategy userManager, IOptions<AppSetting> appSettings)
        {
            _dalLayer = dalLayer;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }


        public async Task<TokenResponseModel> Auth(TokenRequestModel model) // granttype = "refresh_token"
        {
            switch (model.GrantType)
            {
                case "password":
                    return await GenerateNewToken(model);
                case "refresh_token":
                    return await RefreshToken(model);
                default:
                    // not supported - return a HTTP 401 (Unauthorized)
                    return null;
            }
        }


        // Method to Create New JWT and Refresh Token
        private async Task<TokenResponseModel> GenerateNewToken(TokenRequestModel model)
        {
            // check if there's an user with the given username
            var user = await _userManager.FindByEmail(model.UserName);

            // Validate credentials
            if (user != null && await _userManager.CheckPassword(user, model.Password))
            {
                //If the user has confirmed his email
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return null;
                }

                // username & password matches: create the refresh token
                var newRtoken = CreateRefreshToken(_appSettings.ClientId, user.Id);

                // first we delete any existing old refreshtokens
                await RemoveTokensById(user.Id);

                // Add new refresh token to Database
                await _dalLayer.AddToken(newRtoken);

                // Create & Return the access token which contains JWT and Refresh Token
                TokenResponseModel accessToken = await CreateAccessToken(user, newRtoken.Value);

                return accessToken;
            }
            return null;
        }

        // Create access Tokenm
        private async Task<TokenResponseModel> CreateAccessToken(ApplicationUser user, string refreshToken)
        {

            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));

            var roles = await _userManager.GetRoles(user);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                        new Claim("LoggedOn", DateTime.Now.ToString()),
                     }),

                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.Site,
                Audience = _appSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)
            };

            // Generate token
            var newtoken = tokenHandler.CreateToken(tokenDescriptor);

            var encodedToken = tokenHandler.WriteToken(newtoken);

            return new TokenResponseModel()
            {
                token = encodedToken,
                expiration = newtoken.ValidTo,
                refresh_token = refreshToken,
                roles = roles.FirstOrDefault(),
                username = user.Email
            };
        }


        private TokenModel CreateRefreshToken(string clientId, string userId)
        {
            return new TokenModel()
            {
                ClientId = clientId,
                UserId = userId,
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddHours(12)
            };
        }


        //Method to Refresh JWT and Refresh Token
        private async Task<TokenResponseModel> RefreshToken(TokenRequestModel model)
        {

            TokenModel rt = await _dalLayer.GetRefreshToken(_appSettings.ClientId, model.RefreshToken.ToString());

            // check if refresh token is expired
            if (rt.ExpiryTime < DateTime.UtcNow)
            {
                return null;
            }

            // check if there's an user with the refresh token's userId
            var user = await _userManager.FindById(rt.UserId);
            if (user == null)
            {
                // UserId not found or invalid
                return null;
            }

            // generate a new refresh token 
            var rtNew = CreateRefreshToken(rt.ClientId, rt.UserId);

            // invalidate the old refresh token (by deleting it)
            List<TokenModel> rtList = new List<TokenModel>() { rt };
            await _dalLayer.RemoveTokens(rtList);

            // add the new refresh token
            await _dalLayer.AddToken(rtNew);

            // 
            var response = await CreateAccessToken(user, rtNew.Value);

            return response;
        }


        private async Task<string> RemoveTokensById(string id)
        {
            List<TokenModel> oldrTokens = _dalLayer.GetToken(id);
            string result = null;
            if (oldrTokens != null)
            {
                foreach (var oldrt in oldrTokens)
                {
                    result = await _dalLayer.RemoveTokens(oldrTokens);
                }

            }
            return result;
        }
    }
}

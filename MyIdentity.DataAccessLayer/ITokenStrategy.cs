using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyIdentity.EntityLayer;

namespace MyIdentity.DataAccessLayer
{
    public interface ITokenStrategy
    {
        List<TokenModel> GetToken(string id);

        Task<TokenModel> GetRefreshToken(string clientId, string token);

        Task<string> AddToken(TokenModel tokken);

        Task<string> RemoveTokens(List<TokenModel> tokens);
    }
}

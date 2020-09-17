using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyIdentity.EntityLayer;

namespace MyIdentity.DataAccessLayer
{
    public class TokenStrategy : ITokenStrategy
    {
        //private readonly DbContextOptionsBuilder<MyIdentityDbContext> optionsBuilder = new DbContextOptionsBuilder<MyIdentityDbContext>();
        private readonly MyIdentityDbContext _db;

        public TokenStrategy(MyIdentityDbContext db)
        {
            _db = db;
        }

        public List<TokenModel> GetToken(string id)
        {
            List<TokenModel> tokens = new List<TokenModel>();
            try
            {
               tokens = _db.tokens.Where(rt => rt.UserId == id).ToList();
            }
            catch (Exception ex)
            {

            }
            return tokens;
        }

        public async Task<TokenModel> GetRefreshToken(string clientId, string token)
        {
            TokenModel rToken = await _db.tokens.FirstOrDefaultAsync(t => t.ClientId == clientId && t.Value == token);
            return rToken;
        }

        public async Task<string> AddToken(TokenModel tokken)
        {
            var result = await _db.tokens.AddAsync(tokken);
            await _db.SaveChangesAsync();
            return "Successfull";
        }

        public async Task<string> RemoveTokens(List<TokenModel> tokens)
        {
            _db.tokens.RemoveRange(tokens);
            await _db.SaveChangesAsync();
            return "succeddfull";
        }
    }
}

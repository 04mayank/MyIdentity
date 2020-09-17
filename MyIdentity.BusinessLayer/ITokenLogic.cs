using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyIdentity.EntityLayer;

namespace MyIdentity.BusinessLayer
{
    public interface ITokenLogic
    {
        public Task<TokenResponseModel> Auth(TokenRequestModel model);
    }
}

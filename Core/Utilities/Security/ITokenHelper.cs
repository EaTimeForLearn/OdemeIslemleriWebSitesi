using Core.Entities.Concrete;
using Core.Utilities.Security.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security
{
    public interface ITokenHelper
    {
        Token CreateToken(User user, List<OperationClaim> operationClaims, string DBName);
    }
}

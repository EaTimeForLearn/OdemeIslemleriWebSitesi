using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Enums
{
    public class Enums
    {
        public enum ResponseCodes : int
        {
            UnknownServerError = 1000,
            ValidationError = 1100,
            TokenNotFoundError=1200,
            InvalidTokenError = 1300,
            TokenClaimError = 1400,
            TokenExpiredError = 1500
        }
    }
}

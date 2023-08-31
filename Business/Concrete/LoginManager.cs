using Business.Abstract;
using Business.Contants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;

namespace Business.Concrete
{
    public class LoginManager : ILoginService
    {
        private readonly IMusteriDal _musteriDal;

        public LoginManager(IMusteriDal musteriDal)
        {
            this._musteriDal = musteriDal;
        }
        public IDataResult<int> Login(string userName, string Password)
        {
            var retval = _musteriDal.ExecuteSPReturnValue("PAYMENT_Login", new string[] { "@UserName", "@PassWord" }, new object[] { userName, ConvertToHash(Password) });
            if (retval > -1)
            {
                return new SuccessDataResult<int>(retval);
            }
            else return new ErrorDataResult<int>(Messages.InvalidCredentials);
        }

        public byte[] ConvertToHash(string Veri)
        {
            HashAlgorithm HA = HashAlgorithm.Create(HashAlgorithmName.SHA1.Name);
            return HA.ComputeHash(Encoding.ASCII.GetBytes(Veri));
        }
    }
}

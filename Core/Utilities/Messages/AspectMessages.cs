using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Messages
{
    public static class AspectMessages
    {
        public static string WrongValidationType = "Wrong validation type";
        public static string WrongLoggerType = "Wrong logger type";

        public static string TokenNotFound = "Erişim anahtarı yok.";
        public static string InvalidToken = "Geçersiz erişim anahtarı.";
        public static string AuthorizationDenied = "Bu işlem için yetkiniz yok.";
        public static string TokenExpired = "Erişim anahtarının geçerlilik süresi doldu.";
    }
}

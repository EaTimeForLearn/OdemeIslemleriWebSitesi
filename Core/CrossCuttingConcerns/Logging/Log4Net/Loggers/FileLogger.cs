using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Logging.Log4Net.Loggers
{
    public class FileLogger : LoggerServiceBase
    {
        //JsonFileLogger log4net.config dosyasındaki Logger node unun Name attribute u
        public FileLogger() : base("JsonFileLogger")
        {
        }
    }
}

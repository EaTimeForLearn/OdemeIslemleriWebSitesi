using Newtonsoft.Json;

namespace WebAppMVC.Models
{
    public class GetInstallmentModel
    {
        public string BinNumber { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

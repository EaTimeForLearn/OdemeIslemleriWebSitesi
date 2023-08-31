using Newtonsoft.Json;

namespace WebAppMVC.Models
{
  
    

    
    public class CreditCardData
    {
        public string bankName { get; set; }
        public string cardType { get; set; }
        public string cardAssociation { get; set; }
        public string cardFamilyName { get; set; }
        public List<Installment> installments { get; set; }
        public int isCommercial { get; set; }
        public int force3D { get; set; }
    }
    public class Installment
    {
        public int installmentNumber { get; set; }
        public decimal price { get; set; }
        public decimal totalPrice { get; set; }
    }
   


}

namespace WebAppMVC.Models
{
    public class PaymentServiceResultModel<T> where T : class
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

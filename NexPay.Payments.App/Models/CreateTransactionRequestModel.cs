namespace NexPay.Payments.App.Models
{
    public class CreateTransactionRequestModel
    {
        public string BSB { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Reference { get; set; }
        public double Amount { get; set; }
    }
}

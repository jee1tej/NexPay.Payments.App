using System;
using System.Runtime.Serialization;

namespace NexPay.Payments.Services.DTOS
{
    [Serializable]
    [DataContract]
    public class CreateTransactionResponse
    {
        [DataMember]
        public string TransactionStatus { get; set; }
    }
}

using System;
using System.Runtime.Serialization;

namespace NexPay.Payments.Services.DTOS
{
    [Serializable]
    [DataContract]
    public class CreateTranscationRequest
    {
        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string BSB { get; set; }

        [DataMember]
        public string Reference { get; set; }

        [DataMember]
        public double Amount { get; set; }
    }
}

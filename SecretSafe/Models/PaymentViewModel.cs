using SecretSafe.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace SecretSafe.Models
{
    public class PaymentViewModel : IMapFrom<UserPayments>
    {
        public int PaymentID { get; set; }

        public string UserId { get; set; }

        public string PaymentNumber { get; set; }

        public string PaymentRole { get; set; }

        public string BeforeRole { get; set; }

        public decimal Total { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

    }
}
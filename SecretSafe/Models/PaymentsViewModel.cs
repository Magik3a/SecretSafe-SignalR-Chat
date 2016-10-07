using SecretSafe.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSafe.Models
{
    public class PaymentsViewModel : IMapFrom<UserPayments>
    {
        public string PaymentNumber { get; set; }

        public string PaymentRole { get; set; }

        public string BeforeRole { get; set; }

        public decimal Total { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
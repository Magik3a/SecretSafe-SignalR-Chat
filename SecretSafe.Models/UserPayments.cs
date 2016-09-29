using Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecretSafe.Models
{
    public class UserPayments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int PaymentID { get; set; }

        public string UserId { get; set; }

        public string PaymentNumber { get; set; }

        public string PaymentRole { get; set; }

        public string BeforeRole { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        public virtual SecretSafeUser User { get; set; }

    }
}

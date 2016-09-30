using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretSafe.Models;
using Data;

namespace SecretSafe.DataServices
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IRepository<UserPayments> db;

        public PaymentsService(IRepository<UserPayments> db)
        {
            this.db = db;
        }

        public int CreatePayment(UserPayments Payment)
        {
            //Payment.DateCreated = DateTime.Now;

            this.db.Add(Payment);
            return Payment.PaymentID;
        }

        public IQueryable<UserPayments> GetPaymentsForUser(string UserId)
        {
            return db.All().Where(u => u.UserId == UserId).OrderByDescending(p => p.DateCreated);
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}

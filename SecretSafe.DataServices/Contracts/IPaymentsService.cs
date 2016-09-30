namespace SecretSafe.DataServices
{
    using Models;
    using System;
    using System.Linq;

    public interface IPaymentsService
    {
        int CreatePayment(UserPayments Payment);

        IQueryable<UserPayments> GetPaymentsForUser(string UserId);

        void Save();
    }
}
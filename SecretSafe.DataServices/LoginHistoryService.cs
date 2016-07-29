using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretSafe.Models;
using Data;

namespace DataServices
{
    public class LoginHistoryService : ILoginHistoryService
    {
        private readonly IRepository<LoginHistory> db;

        public LoginHistoryService(IRepository<LoginHistory> db)
        {
            this.db = db;
        }

        public int Add(LoginHistory loginHistory)
        {
            db.Add(loginHistory);
            db.SaveChanges();

            return loginHistory.Id;
        }

        public void Login(string UserName)
        {
            var login = new LoginHistory();
            login.UserId = UserName;
            login.StartSession = DateTime.Now;
            db.Add(login);
            db.SaveChanges();
        }

        public void Logoff(string UserName)
        {
            throw new NotImplementedException();
        }

        public LoginHistory Update(LoginHistory loginHistory)
        {
            db.Update(loginHistory);
            db.SaveChanges();
            return loginHistory;
        }
    }
}

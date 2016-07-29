namespace DataServices
{
    using System;
    using System.Linq;
    using SecretSafe.Models;
    using Data;

    public class LoginHistoryService : ILoginHistoryService
    {
        private readonly IRepository<LoginHistory> db;

        public LoginHistoryService(IRepository<LoginHistory> db)
        {
            this.db = db;
        }

        public void Login(string UserName, string BrowserInfo)
        {
            var login = new LoginHistory();
            login.UserId = UserName;
            login.StartSession = DateTime.Now;
            login.EndSession = DateTime.Now;
            login.BrowserInfo = BrowserInfo;
            db.Add(login);
            db.SaveChanges();
        }

        public void Logoff(string UserName)
        {
            var logetUser = db.All().Where(s => s.UserId == UserName).OrderByDescending(l => l.StartSession).FirstOrDefault();
            logetUser.EndSession = DateTime.Now;
            db.SaveChanges();
        }
    }
}

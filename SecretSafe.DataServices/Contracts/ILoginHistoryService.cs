using SecretSafe.Models;

namespace DataServices
{
    public interface ILoginHistoryService
    {
        int Add(LoginHistory loginHistory);

        LoginHistory Update(LoginHistory loginHistory);

        void Login(string UserName);

        void Logoff(string UserName);
    }
}

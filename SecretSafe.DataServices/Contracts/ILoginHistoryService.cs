using SecretSafe.Models;

namespace DataServices
{
    public interface ILoginHistoryService
    {
        void Login(string UserName,string BrowserInfo);

        void Logoff(string UserName);
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SecretSafe.Startup))]
namespace SecretSafe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

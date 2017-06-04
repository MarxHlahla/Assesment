using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OutWeb.Startup))]
namespace OutWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

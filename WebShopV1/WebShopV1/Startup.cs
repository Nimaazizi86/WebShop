using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebShopV1.Startup))]
namespace WebShopV1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

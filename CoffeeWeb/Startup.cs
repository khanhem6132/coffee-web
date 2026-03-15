using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoffeeWeb.Startup))]
namespace CoffeeWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

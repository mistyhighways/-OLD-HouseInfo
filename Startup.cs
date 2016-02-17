using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HouseCase.Startup))]
namespace HouseCase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

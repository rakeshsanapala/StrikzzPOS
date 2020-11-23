using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StrikzzPOS.Startup))]
namespace StrikzzPOS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

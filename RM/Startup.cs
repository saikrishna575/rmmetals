using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RM.Startup))]
namespace RM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

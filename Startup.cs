using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedManager.Startup))]
namespace MedManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MTGWatcher.Startup))]
namespace MTGWatcher
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

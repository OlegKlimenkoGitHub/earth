using Microsoft.Owin;
using Owin;
using Battle.WebUI;

[assembly: OwinStartup(typeof(Battle.WebUI.Startup))]
namespace Battle.WebUI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
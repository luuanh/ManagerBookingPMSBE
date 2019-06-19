using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(BookingEnginePMS.Startup))]

namespace BookingEnginePMS
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BusinessManagement.Startup))]
namespace BusinessManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}

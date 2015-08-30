using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ItemTrees.Startup))]
namespace ItemTrees
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
        }
    }
}

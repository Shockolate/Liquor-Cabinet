using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace LiquorCabinet
{
    public class LocalEntryPoint
    {
        public static void Main(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}

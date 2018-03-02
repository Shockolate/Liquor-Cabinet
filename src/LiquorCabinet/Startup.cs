using System.Buffers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LiquorCabinet
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.OutputFormatters.Add(new JsonOutputFormatter(JsonSerializerFactory.DefaultSettings, ArrayPool<char>.Shared)));

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
        }

        public void Configure(IApplicationBuilder appBuilder, IHostingEnvironment environment)
        {
            if(environment.IsDevelopment())
            {
                appBuilder.UseDeveloperExceptionPage();
            }

            appBuilder.UseMvc();
        }
    }
}

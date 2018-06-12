using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreIntro {

  public class Startup {

    // Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services) {
      services.AddSession();
      services.AddMvc();
    }

    // Use this method to configure the HTTP request pipeline
    public void Configure(
      IApplicationBuilder app,
      IHostingEnvironment env,
      ILoggerFactory loggerFactory
    ) {
      if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
      loggerFactory.AddConsole();
      app.UseStaticFiles();
      app.UseSession();
      app.UseMvc();
    }

  }
}

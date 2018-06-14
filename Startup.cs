using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;
using AspNetCoreIntro.Models;

namespace AspNetCoreIntro {

  public class Startup {

    public IConfiguration Configuration { get; private set; }

    // Connects appsettings.json config file as an object
    public Startup(IHostingEnvironment env) {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    // Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services) {
      services.AddMvc();
      services.AddSession();

      // Add options config sections to services for injection
      Action<DbContextOptionsBuilder> useMySqlAppSettings =
        options => options.UseMySql(Configuration["DbConfig:ConsnectionString"]);
      services.AddDbContext<Context>(useMySqlAppSettings);
      services.Configure<ApiConfig>(Configuration.GetSection("ApiConfig"));
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

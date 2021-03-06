using DncIds4.ApiGatewayOcelot.Config;
using DncIds4.Common.IS4;
using DncIds4.Common.IS4.Extensions;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace DncIds4.ApiGatewayOcelot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            this.IdentityServerConfig = this.Configuration.GetIdentityServerConfig();
        }

        public IConfiguration Configuration { get; }
        private IdentityServerConfig IdentityServerConfig { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication()
                .AddIdentityServerAuthentication(Constants.Apis.ResourceApi, opts =>
                {
                    opts.Authority = this.IdentityServerConfig.IdentityServerUrl;
                    opts.RequireHttpsMetadata = false;
                    opts.ApiName = Constants.Apis.ResourceApi;
                    opts.ApiSecret = "secret";
                    opts.SupportedTokens = SupportedTokens.Both;
                })
                .AddIdentityServerAuthentication(Constants.Apis.IdentityServerApi, opts =>
                {
                    opts.Authority = this.IdentityServerConfig.IdentityServerUrl;
                    opts.RequireHttpsMetadata = false;
                    opts.ApiName = Constants.Apis.IdentityServerApi;
                    opts.ApiSecret = "secret";
                    opts.SupportedTokens = SupportedTokens.Both;
                });

            services
                .AddOcelot(this.Configuration)
                .AddConsul();

            services.AddCors(opts =>
            {
                opts.AddDefaultPolicy(cfg =>
                {
                    cfg.AllowAnyHeader();
                    cfg.AllowAnyOrigin();
                    cfg.AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseOcelot().Wait();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.Extensions;
using APICatalogo.Filter;
using APICatalogo.Logging;
using APICatalogo.Repository;
using APICatalogo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace APICatalogo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //adiciona UnitOfWork (camada superior a Repository)
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //adiciona / configura serviço de log
            services.AddScoped<ApiLoggingFilter>();

            //essa eu inclui para o contexto de BD
            //a string de conexao esta no appsettings.json
            services.AddDbContext<AppDbContext>(options => 
                                                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            //indica que o tempo de vida da instancia do servico que for criada, vai ser criada cada vez q for solicitada
            //implementacao de MeuServico.cs
            services.AddTransient<IMeuServico, MeuServico>();

            services.AddControllers()
                    .AddNewtonsoftJson(options => //se der erro nessa linha, tem que referenciar o Microsoft.AspNetCore.Mvc.NewtonsoftJson
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore ); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //adicionando middleware de tratamento de erro
            app.ConfigureExceptionHandler();

            //adicionando middleware para redirecionar para https
            app.UseHttpsRedirection();

            //adicionando middleware  de roteamento
            app.UseRouting();

            //adicionando middleware que habilita a autorizacao
            app.UseAuthorization();

            //adicionando middleware  que executa o endpoint do request atual
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //adiciona Provider para o Logger
            loggerFactory.AddProvider(
                new CustomLoggerProvider(
                    new CustomLoggerProviderConfiguration
                                        {
                                            LogLevel = LogLevel.Information
                                        }                           )
                                    );;
        }
    }
}

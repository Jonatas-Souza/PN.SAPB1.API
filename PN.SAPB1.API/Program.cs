
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PN.SAPB1.API.Context;
using System.Reflection;

namespace PN.SAPB1.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // Recupera a string de conexão do SQL
            string? sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
                            options.UseSqlServer(sqlConnection));


            // Informações da API Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "API - PN SAP B1", 
                    Description = "Essa API permite buscar os dados de parceiros de negócios do banco de dados da API, também é possível criar novos PNs no banco de integração e na base do SAP B1.",
                    Version = "v1" , 
                    Contact = new OpenApiContact { Name = "Jonatas Souza", Email = "jonatassouzapereira@gmail.com" }
                
                });

                // Localizar o arquivo de comentários XML e incluir no Swagger
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

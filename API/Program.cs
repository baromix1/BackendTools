using API.Data;
using API.Data.Repositories;
using API.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(opt =>
{
    var connectionString = "server=toolsserverforpolsl.mysql.database.azure.com;user=root1;password=admin123$;database=toolsdb";

    var serverVersion = new MySqlServerVersion(new Version(8, 0));
    opt.UseMySql(connectionString, serverVersion);
}, ServiceLifetime.Transient);

builder.Services.AddScoped<WspolnotaRepository>();
builder.Services.AddScoped<UzytkownikRepository>();
builder.Services.AddScoped<OfertaRepository>();
builder.Services.AddScoped<PostForumRepository>();
builder.Services.AddScoped<KonwersacjaRepository>();
builder.Services.AddScoped<HistoriaTransakcjiRepository>();

var app = builder.Build();
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:3000"));
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

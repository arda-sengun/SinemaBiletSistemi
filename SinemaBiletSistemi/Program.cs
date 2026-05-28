using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.Business.Concrete;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.DataAccess.Concrete;

var builder = WebApplication.CreateBuilder(args);
const string FrontendCorsPolicy = "FrontendCorsPolicy";

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "http://127.0.0.1:5173", "http://localhost:4173", "http://127.0.0.1:4173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MssqlConnection")
    ?? throw new InvalidOperationException("MssqlConnection baglanti cumlesi appsettings.json icinde bulunamadi.");

// Katmanlar DI ile baglanir; DAL tarafinda yalnizca stored procedure calistirilir.
builder.Services.AddSingleton<IConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
builder.Services.AddScoped<IFilmlerDAL, FilmlerDAL>();
builder.Services.AddScoped<IFilmlerBL, FilmlerBL>();
builder.Services.AddScoped<ISalonlarDAL, SalonlarDAL>();
builder.Services.AddScoped<ISalonlarBL, SalonlarBL>();
builder.Services.AddScoped<IKoltuklarDAL, KoltuklarDAL>();
builder.Services.AddScoped<IKoltuklarBL, KoltuklarBL>();
builder.Services.AddScoped<ISeanslarDAL, SeanslarDAL>();
builder.Services.AddScoped<ISeanslarBL, SeanslarBL>();
builder.Services.AddScoped<IMusterilerDAL, MusterilerDAL>();
builder.Services.AddScoped<IMusterilerBL, MusterilerBL>();
builder.Services.AddScoped<IBiletlerDAL, BiletlerDAL>();
builder.Services.AddScoped<IBiletlerBL, BiletlerBL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(FrontendCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();

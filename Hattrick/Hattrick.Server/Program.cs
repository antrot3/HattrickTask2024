using Hattrick.ServiceLayer;
using Hattrick.ServiceLayer.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default");


builder.Services.AddDbContext<HattrickDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddTransient<IBetType, BetTypesService>();
builder.Services.AddTransient<ICoefficient, CoefficientService>();
builder.Services.AddTransient<IMatch, MatchService>();
builder.Services.AddTransient<ISports, SportsService>();
builder.Services.AddTransient<ITopOffers, TopOffersService>();
builder.Services.AddTransient<IUser, UserService>();
builder.Services.AddTransient<IWalletTransactionService, WalletTransactionService>();
builder.Services.AddTransient<ITicketService, TicketService>();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HattrickDbContext>();
    dbContext.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

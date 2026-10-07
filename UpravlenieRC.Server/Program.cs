using UpravlenieRC.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RC_SkladContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.





app.Run();


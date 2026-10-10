using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UpravlenieRC.Server.Models;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Controllers;
using UpravlenieRC.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddDbContext<RC_SkladContext>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = JWT.validation;
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



/*app.MapGet("/hash", (RC_SkladContext context) =>
   {
       foreach (var user in context.Users.ToList())
       {
           user.Password = hasher.HashPassword(user, user.Password);
       }
       
       context.SaveChanges();
       return Results.Ok(new
       {
           access_token = CreateToken(context.Users.Include(u => u.IdtypeNavigation).First(i => i.Id == 1)),
           token_type = "Bearer"
       });
   }).AllowAnonymous();*/ //один раз захешировать тестовые данные и удалить

app.Run();


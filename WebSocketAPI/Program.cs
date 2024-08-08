using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebSocketAPI.Service;
using WebSocketAPI.SignalR;
using WebSocketAPI.TokenService;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"]);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed((host) => true) // for any origin
            .AllowCredentials());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Swagger WebSocket API",
        Version = "v1",
        Description = "WEB-SOCKET"
    });
    options.UseAllOfToExtendReferenceSchemas();
    options.IgnoreObsoleteActions();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Token gir",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});
builder.Services.AddSignalR();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<CryptoHub>();
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//           .AddJwtBearer(options =>
//           {
//               //options.Authority = "Authority URL"; // TODO: Update URL

//               options.TokenValidationParameters = new TokenValidationParameters
//               {
//                   ValidateLifetime = true,
//                   ValidateIssuerSigningKey = true,
//                   ValidateIssuer = false,
//                   ValidateAudience = false,
//                   IssuerSigningKey = new SymmetricSecurityKey(key),
//               };

//               options.Events = new JwtBearerEvents
//               {

//                   OnMessageReceived = context =>
//                   {
//                       var accessToken = context.Request.Query["access_token"];
//                       var path = context.HttpContext.Request.Path;
//                       if (!string.IsNullOrEmpty(accessToken) &&
//                           (path.StartsWithSegments("/cryptoHub")))
//                       {
//                           context.Token = accessToken;
//                       }
//                       return Task.CompletedTask;
//                   }
//               };
//           });
builder.Services.AddHostedService<WebSoketService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapHub<CryptoHub>("/cryptoHub");

app.MapControllers();

app.Run();

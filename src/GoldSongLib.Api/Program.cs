using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Azure.Storage.Blobs;
using GoldSongLib.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

Console.WriteLine($"Starting up in process {System.Diagnostics.Process.GetCurrentProcess().Id}");

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration!;

var privateKey = File.ReadAllText("../../cert/private_key.pem");
var rsa = System.Security.Cryptography.RSA.Create();
rsa.ImportFromPem(privateKey);
var jwtSigningKey = new RsaSecurityKey(rsa);

// Add services to the container.
builder.Services.AddCore(configuration);
builder.Services.AddSingleton<AsymmetricSecurityKey>(jwtSigningKey);
builder.Services.AddScoped<ITenantDataClient>(p => {
    var data = p.GetRequiredService<IData>();
    var httpContext = p.GetRequiredService<IHttpContextAccessor>().HttpContext;
    var tenantId = httpContext!.User.FindFirst(x => x.Type == "tenant")!.Value;
    var tenant = new Tenant(tenantId);

    return data.GetTenantDataClient(tenant);
});

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services
    .AddAuthentication(ConfigureAuthentication)
        .AddJwtBearer(ConfigureJwtBearer);

builder.Services
    .AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<BlobServiceClient>(new BlobServiceClient("UseDevelopmentStorage=True"));
builder.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

void ConfigureAuthentication(AuthenticationOptions options)
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}

void ConfigureJwtBearer(JwtBearerOptions options)
{

    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = false,
        ValidateIssuer = true,
        ValidIssuer = "SongSetBuilder",
        ValidateAudience = true,
        ValidAudience = "SongSetBuilder",
        IssuerSigningKey = jwtSigningKey
    };
}
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GoldSongLib.Api.Controllers;

[Route("api/user")]
public class UserController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetUser(
        [FromServices] Core.IData dataClient,
        CancellationToken cancellationToken    
    )
    {
        var username = this.User.Claims.First(x => x.Type == "sub").Value;
        var user = await dataClient.GetUser(username, cancellationToken);
        return this.Ok(user);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login(
        [FromBody, Required]LoginBody body,
        [FromServices]HttpClient httpClient,
        [FromServices]SigningCredentials signingCredentials,
        [FromServices] Core.IData dataClient,
        CancellationToken cancellationToken
    )
    {
        var googleTokenInfoUrl = $"https://oauth2.googleapis.com/tokeninfo?id_token={body.GoogleToken}";
        var response = await httpClient.GetAsync(googleTokenInfoUrl);

        var content = await (response.Content?.ReadFromJsonAsync<Dictionary<string, string>>() ?? Task.FromResult<Dictionary<string, string>?>(null));

        if (!response.IsSuccessStatusCode)
        {
            return this.StatusCode((int)response.StatusCode, content);
        }

        var tenant = "GoldAveChurch";
        var email = content!["email"];

        var user = await dataClient.GetUser(email, cancellationToken);
        if (user == null)
        {
            var name = content!["name"];
            var givenName = content!["given_name"];
            var familyName = content!["family_name"];

            user = new Core.Models.UserModel(
                Guid.NewGuid(),
                email,
                givenName,
                familyName,
                name,
                new [] { tenant } 
            );
            await dataClient.AddUser(user, cancellationToken);
        }

        var token = new JwtSecurityToken(
            issuer: "SongSetBuilder",
            audience: "SongSetBuilder",
            claims: new [] {
                new Claim("sub", user.Username!),
                new Claim("name", user.FullName),
                new Claim("gn", user.GivenName),
                new Claim("fn", user.FamilyName),
                new Claim("tenant", user.Tenants.First()),
            },
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        await dataClient.GetTenantDataClient(new Core.Tenant(tenant)).EnsureInitialized();
        
        return this.Ok(new {
            token = jwt,
            user = user
        });
    }

    public record LoginBody(
        string GoogleToken
    );
}
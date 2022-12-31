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
    public ActionResult GetUser()
    {
        var payload = new Dictionary<string, string>();

        foreach (var claim in this.User.Claims)
        {
            payload[claim.Type] = claim.Value;
        }

        return this.Ok(payload);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login(
        [FromBody, Required]LoginBody body,
        [FromServices]HttpClient httpClient,
        [FromServices]AsymmetricSecurityKey jwtSigningKey,
        [FromServices] Core.IData dataClient
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
        var name = content!["name"];
        var givenName = content!["given_name"];
        var familyName = content!["family_name"];

        var signingCredentials = new SigningCredentials(jwtSigningKey, SecurityAlgorithms.RsaSha256Signature);

        var token = new JwtSecurityToken(
            issuer: "SongSetBuilder",
            audience: "SongSetBuilder",
            claims: new [] {
                new Claim("sub", email!),
                new Claim("name", name),
                new Claim("gn", givenName),
                new Claim("fn", familyName),
                new Claim("tenant", tenant),
            },
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        await dataClient.GetTenantDataClient(new Core.Tenant(tenant)).EnsureInitialized();
        
        return this.Ok(new {
            token = jwt
        });
    }

    public record LoginBody(
        string GoogleToken
    );
}
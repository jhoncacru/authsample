using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenidDictClient.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    [AllowAnonymous]
    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        //var tt = User.Identity;
        var claims = (ClaimsIdentity)User.Identity;
        var datas = claims.Claims.Select(p => new { p.Issuer, SubjectName = p.Subject!.Name, ClaimType = p.Type,  p.Value });
        return Ok(datas);
    }


    [AllowAnonymous]
    [HttpPost("ValidateToken")]
    public IActionResult ValidateToken([FromBody]string token)
    {
        try
        {

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://localhost:44395/",
                ValidAudience = "rs_dataEventRecordsApi",
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY="))                   
            };

            var principal = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            return Ok(principal.Identity.Name);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

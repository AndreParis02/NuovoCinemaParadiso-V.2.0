using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Helpers;

public class JwtHelper
{
  private readonly IConfiguration _configurazione;
  public JwtHelper(IConfiguration configurazione) 
  {
    _configurazione = configurazione; 
  }

  public string GenerateToken(Utente utente, IList<string> ruoli) 
  {

    string? key = _configurazione["Jwt:Key"];
    string? issuer = _configurazione["Jwt:Issuer"]; 
    string? audience = _configurazione["Jwt:Audience"];  

    if((string.IsNullOrEmpty(key)) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
    {
        throw new Exception("Configurazione JWT mancante.");
    }

    List<Claim> claims = new List<Claim>();
    
    // Claim base utente

        claims.Add(new Claim(ClaimTypes.NameIdentifier, utente.Id));
        claims.Add(new Claim(ClaimTypes.Name, utente.UserName ?? ""));
        claims.Add(new Claim(ClaimTypes.Email, utente.Email ?? ""));

    // Claim di ruolo
    for(int i = 0; i < ruoli.Count; i++)
    {
        claims.Add(new Claim(ClaimTypes.Role, ruoli[i]));
    }

    SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    SigningCredentials credentials   = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    JwtSecurityToken token = new JwtSecurityToken(
        issuer   : issuer,
        audience : audience,
        claims   : claims,
        expires : DateTime.UtcNow.AddHours(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
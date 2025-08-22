using Microsoft.AspNetCore.Mvc;
using ProductApi.Contracts;
using ProductApi.Services;

namespace ProductApi.Controllers;
[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwt;
    public AuthController(IJwtTokenService jwt) => _jwt = jwt;

    // POST /api/token
    [HttpPost("token")]
    public ActionResult<TokenResponse> Token([FromBody] LoginRequest req)
    {
        // spec: username=admin, password=admin123
        if (req.Username == "admin" && req.Password == "admin123")
        {
            var token = _jwt.CreateToken(req.Username);
            return Ok(new TokenResponse(token));
        }
        return Unauthorized();
    }
}

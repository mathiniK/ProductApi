namespace ProductApi.Contracts;
public record LoginRequest(string Username, string Password);
public record TokenResponse(string Token);

namespace IAM.Domain.Models;

public record AuthResponse(string Token, DateTime Expiration);
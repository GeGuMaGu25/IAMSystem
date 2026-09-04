using IAM.Domain.Entities;

namespace IAM.Domain.Services;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}
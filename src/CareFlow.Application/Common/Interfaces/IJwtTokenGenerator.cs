using CareFlow.Domain.Entities;

namespace CareFlow.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}

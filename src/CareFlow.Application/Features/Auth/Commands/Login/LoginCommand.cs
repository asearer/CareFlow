using CareFlow.Application.Common.Interfaces;
using CareFlow.Application.DTOs.Auth;
using MediatR;

namespace CareFlow.Application.Features.Auth.Commands.Login;

public record LoginCommand(LoginRequest Request) : IRequest<AuthResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<AuthResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Request.Email, cancellationToken);
        if (user == null)
        {
            throw new Exception("Invalid credentials.");
        }

        if (!_passwordHasher.VerifyPassword(command.Request.Password, user.PasswordHash))
        {
            throw new Exception("Invalid credentials.");
        }

        var token = _tokenGenerator.GenerateToken(user);

        return new AuthResponse(token, user.Id, user.Email, user.FirstName, user.LastName, user.Role.ToString());
    }
}

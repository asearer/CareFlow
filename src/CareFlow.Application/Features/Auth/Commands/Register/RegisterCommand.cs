using CareFlow.Application.Common.Interfaces;
using CareFlow.Domain.Interfaces;
using CareFlow.Application.DTOs.Auth;
using CareFlow.Domain.Entities;
using MediatR;

namespace CareFlow.Application.Features.Auth.Commands.Register;

public record RegisterCommand(RegisterRequest Request) : IRequest<AuthResponse>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository; // Using Repo abstraction
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork; // Using UnitOfWork to save

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(command.Request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new Exception("User with this email already exists."); // TODO: Custom Exception
        }

        var passwordHash = _passwordHasher.HashPassword(command.Request.Password);

        var user = new User(
            command.Request.Email,
            passwordHash,
            command.Request.FirstName,
            command.Request.LastName,
            command.Request.Role
        );

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _tokenGenerator.GenerateToken(user);

        return new AuthResponse(token, user.Id, user.Email, user.FirstName, user.LastName, user.Role.ToString());
    }
}

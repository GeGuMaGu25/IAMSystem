using IAM.Domain.Entities;
using IAM.Domain.Models;
using IAM.Domain.Repositories;

namespace IAM.Domain.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    
    // Inyectamos las dependencias
    public AuthService(
        IUserRepository userRepository,
        ITokenGenerator tokenGenerator,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> RegisterAsync(AuthRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new InvalidOperationException("El email ya está registrado");

        var hashedPassword = _passwordHasher.Hash(request.Password);
        var user = new User(request.Email, hashedPassword);
        
        await _userRepository.AddAsync(user);
        
        return user;
    }

    public async Task<AuthResponse> LoginAsync(AuthRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Creedenciales inválidos");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Creedenciales inválidas");

        var token = _tokenGenerator.GenerateToken(user);
        return new AuthResponse(token, DateTime.UtcNow.AddHours(1));
    }
}
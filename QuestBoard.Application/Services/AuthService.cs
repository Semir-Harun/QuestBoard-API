using System;
using System.Security.Cryptography;
using System.Text;
using QuestBoard.Application.Abstractions;
using QuestBoard.Application.DTOs.Auth;
using QuestBoard.Domain.Entities;

namespace QuestBoard.Application.Services;

public sealed class AuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var user = new User
        {
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            DisplayName = request.DisplayName,
        };

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenService.GenerateToken(user, new[] { request.Role });
        return new AuthResponse(user.Id, user.Email, user.DisplayName, token, new[] { request.Role });
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken)
                   ?? throw new InvalidOperationException("Invalid credentials.");

        if (!VerifyPassword(user.PasswordHash, request.Password))
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        var roles = user.Role is null ? Array.Empty<string>() : new[] { user.Role.Name };
        var token = _jwtTokenService.GenerateToken(user, roles);
        return new AuthResponse(user.Id, user.Email, user.DisplayName, token, roles);
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = SHA256.HashData(Combine(salt, Encoding.UTF8.GetBytes(password)));
        return Convert.ToBase64String(Combine(salt, hash));
    }

    private static bool VerifyPassword(string storedHash, string password)
    {
        var data = Convert.FromBase64String(storedHash);
        var salt = data[..16];
        var hash = data[16..];
        var computed = SHA256.HashData(Combine(salt, Encoding.UTF8.GetBytes(password)));
        return hash.SequenceEqual(computed);
    }

    private static byte[] Combine(byte[] first, byte[] second)
    {
        var output = new byte[first.Length + second.Length];
        Buffer.BlockCopy(first, 0, output, 0, first.Length);
        Buffer.BlockCopy(second, 0, output, first.Length, second.Length);
        return output;
    }
}

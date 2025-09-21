using ProductApi.Application.DTOs;

namespace ProductApi.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(UserRegisterDto dto);
    Task<AuthResponseDto?> LoginAsync(UserLoginDto dto);
}
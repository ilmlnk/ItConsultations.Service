using ItConsultations.Business.Dtos.AuthDtos;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Services.AuthService;

public interface IFirebaseAuthService
{
    Task<LoginResponseDto> LoginAsync(string idToken);

    Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);

    Task<bool> RevokeTokenAsync(string refreshToken, string userId);

    Task<UserInfoDto?> GetUserInfoAsync(string firebaseUid);

    Task<bool> ValidateTokenAsync(string accessToken);

    Task<UserInfoDto> RegisterAsync(RegisterDto registerDto);

    Task<UserInfoDto?> GetUserByRoleAsync(string firebaseUid, UserRole role);

    Task<UserEntity?> GetUserByFirebaseUidAsync(string firebaseUid);

    Task<UserEntity> CreateUserAsync(UserInfoDto userInfo);
    
    Task UpdateUserLastLoginAsync(string firebaseUid);
} 
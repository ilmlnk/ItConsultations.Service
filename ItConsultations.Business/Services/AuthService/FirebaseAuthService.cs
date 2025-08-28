using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.AuthDtos;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.Configs;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using ItConsultations.Business.Entities.RefreshTokens;
using ItConsultations.Business.Entities.Coaches;
using ItConsultations.Business.Entities.Students;
using ItConsultations.Business.AutoMapperConfiguration;

namespace ItConsultations.Business.Services.AuthService;

public class FirebaseAuthService : IFirebaseAuthService
{
    private readonly IRepository<UserEntity, long> _userRepository;
    private readonly IRepository<RefreshToken, long> _refreshTokenRepository;
    private readonly IRepository<Coach, long> _coachRepository;
    private readonly IRepository<Student, long> _studentRepository;
    private readonly FirebaseConfig _firebaseConfig;
    private readonly string _jwtSecret;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;

    public FirebaseAuthService(
        IRepository<UserEntity, long> userRepository,
        IRepository<RefreshToken, long> refreshTokenRepository,
        IRepository<Coach, long> coachRepository,
        IRepository<Student, long> studentRepository,
        IOptions<FirebaseConfig> firebaseConfig,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _coachRepository = coachRepository;
        _studentRepository = studentRepository;
        _firebaseConfig = firebaseConfig.Value;
        _jwtSecret = configuration["Jwt:Secret"] ?? "your-super-secret-key-with-at-least-32-characters";
        _jwtIssuer = configuration["Jwt:Issuer"] ?? "ItConsultations";
        _jwtAudience = configuration["Jwt:Audience"] ?? "ItConsultationsUsers";
        
        InitializeFirebase();
    }

    public async Task<LoginResponseDto> LoginAsync(string idToken)
    {
        var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
        var uid = decodedToken.Uid;

        var user = await GetUserByFirebaseUidAsync(uid);

        await UpdateUserLastLoginAsync(uid);

        var accessToken = await GenerateAccessTokenAsync(user);
        var refreshToken = await GenerateRefreshTokenAsync(user);

        var userInfo = await GetUserInfoAsync(uid);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = userInfo
        };
    }

    public async Task<UserInfoDto> RegisterAsync(RegisterDto registerDto)
    {
        var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(registerDto.IdToken);
        var uid = decodedToken.Uid;
        var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
        var user = MapperManager.Map<UserEntity>(registerDto);
        await _userRepository.CreateAsync(user);

        return MapperManager.Map<UserInfoDto>(user);
    }

    public async Task<bool> ValidateTokenAsync(string accessToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtIssuer,
                ValidateAudience = true,
                ValidAudience = _jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var tokenEntity = _refreshTokenRepository.Get(rt => rt.Token == refreshToken && !rt.IsRevoked).FirstOrDefault();
        
        if (tokenEntity == null)
        {
            throw new InvalidOperationException("Invalid refresh token");
        }
        
        if (tokenEntity.ExpiresAt <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Refresh token has expired");
        }

        var user = await _userRepository.GetAsync(tokenEntity.UserId);

        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var newAccessToken = await GenerateAccessTokenAsync(user);
        var newRefreshToken = await GenerateRefreshTokenAsync(user);

        tokenEntity.IsRevoked = true;
        tokenEntity.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(tokenEntity);

        var userInfo = await GetUserInfoAsync(user.FirebaseUid);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = userInfo
        };
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken, string userId)
    {
        var tokenEntity = _refreshTokenRepository.Get(rt => rt.Token == refreshToken && !rt.IsRevoked).FirstOrDefault();
        
        if (tokenEntity == null)
        {
            throw new InvalidOperationException("Invalid refresh token");
        }

        var user = await _userRepository.GetAsync(tokenEntity.UserId);

        if (user == null || user.FirebaseUid != userId)
        {
            throw new InvalidOperationException("User not found or unauthorized");
        }

        tokenEntity.IsRevoked = true;
        tokenEntity.RevokedAt = DateTime.UtcNow;
        tokenEntity.RevokedBy = userId;

        await _refreshTokenRepository.UpdateAsync(tokenEntity);
        return true;
    }

    public async Task<UserEntity?> GetUserByFirebaseUidAsync(string firebaseUid)
    {
        return _userRepository.Get(u => u.FirebaseUid == firebaseUid).FirstOrDefault();
    }

    private async Task<UserEntity> CreateUserAsync(RegisterDto registerDto)
    {
        var user = MapperManager.Map<UserEntity>(registerDto);
        await _userRepository.CreateAsync(user);
        return user;
    }

    private async Task UpdateUserLastLoginAsync(string firebaseUid)
    {
        var user = await GetUserByFirebaseUidAsync(firebaseUid);
        
        if (user == null)
        {
            throw new InvalidOperationException($"User with Firebase UID {firebaseUid} not found");
        }
        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
    }

    private async Task<string> GenerateAccessTokenAsync(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FirebaseUid),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("firebase_uid", user.FirebaseUid)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _jwtIssuer,
            Audience = _jwtAudience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private async Task<string> GenerateRefreshTokenAsync(UserEntity user)
    {
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsRevoked = false
        };

        await _refreshTokenRepository.CreateAsync(refreshTokenEntity);
        return refreshToken;
    }

    private void InitializeFirebase()
    {
        try
        {
            if (FirebaseApp.DefaultInstance != null)
            {
                return;
            }

            var serviceAccountJson = GetFirebaseServiceAccountJson();

            if (string.IsNullOrEmpty(serviceAccountJson))
            {
                throw new InvalidOperationException("Failed to generate Firebase service account JSON");
            }

            var credential = GoogleCredential.FromJson(serviceAccountJson);
            FirebaseApp.Create(new AppOptions
            {
                Credential = credential,
                ProjectId = _firebaseConfig.ProjectId
            });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to initialize Firebase: {ex.Message}", ex);
        }
    }

    private string GetFirebaseServiceAccountJson()
    {
        if (string.IsNullOrEmpty(_firebaseConfig.ProjectId))
        {
            throw new InvalidOperationException("Firebase ProjectId is not configured");
        }

        if (string.IsNullOrEmpty(_firebaseConfig.PrivateKey))
        {
            throw new InvalidOperationException("Firebase PrivateKey is not configured");
        }

        if (string.IsNullOrEmpty(_firebaseConfig.ClientEmail))
        {
            throw new InvalidOperationException("Firebase ClientEmail is not configured");
        }

        if (string.IsNullOrEmpty(_firebaseConfig.PrivateKeyId))
        {
            throw new InvalidOperationException("Firebase PrivateKeyId is not configured");
        }

        if (_firebaseConfig.PrivateKey.Contains("YOUR_PRIVATE_KEY_HERE"))
        {
            throw new InvalidOperationException("Firebase PrivateKey is not properly configured. Please replace 'YOUR_PRIVATE_KEY_HERE' with actual private key");
        }

        var escapedPrivateKey = _firebaseConfig.PrivateKey
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\"", "\\\"");

        return $@"{{
            ""type"": ""{_firebaseConfig.Type}"",
            ""project_id"": ""{_firebaseConfig.ProjectId}"",
            ""private_key_id"": ""{_firebaseConfig.PrivateKeyId}"",
            ""private_key"": ""{escapedPrivateKey}"",
            ""client_email"": ""{_firebaseConfig.ClientEmail}"",
            ""client_id"": ""{_firebaseConfig.ClientId}"",
            ""auth_uri"": ""{_firebaseConfig.AuthUri}"",
            ""token_uri"": ""{_firebaseConfig.TokenUri}"",
            ""auth_provider_x509_cert_url"": ""{_firebaseConfig.AuthProviderX509CertUrl}"",
            ""client_x509_cert_url"": ""{_firebaseConfig.ClientX509CertUrl}""
        }}";
    }

    Task<UserEntity> IFirebaseAuthService.CreateUserAsync(UserInfoDto userInfo)
    {
        throw new NotImplementedException();
    }

    Task IFirebaseAuthService.UpdateUserLastLoginAsync(string firebaseUid)
    {
        throw new NotImplementedException();
    }
} 
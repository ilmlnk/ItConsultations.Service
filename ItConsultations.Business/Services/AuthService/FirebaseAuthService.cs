using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.AuthDtos;
using ItConsultations.Business.Entities.User;
using ItConsultations.Business.Entities.Consultation;
using ItConsultations.Business.SharedTypes.Enums.System;
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
using ItConsultations.Utilities.Guards;

namespace ItConsultations.Business.Services.AuthService;

public class FirebaseAuthService : IFirebaseAuthService
{
    private readonly IRepository<User, long> _userRepository;
    private readonly IRepository<RefreshToken, long> _refreshTokenRepository;
    private readonly IRepository<Coach, long> _coachRepository;
    private readonly IRepository<Student, long> _studentRepository;
    private readonly FirebaseConfig _firebaseConfig;
    private readonly string _jwtSecret;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;

    public FirebaseAuthService(
        IRepository<User, long> userRepository,
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
        // Проверяем, что все необходимые поля заполнены
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

    public async Task<LoginResponseDto> LoginAsync(string idToken)
    {
        try
        {
            // Проверяем формат токена
            if (string.IsNullOrEmpty(idToken))
            {
                throw new ArgumentException("ID token cannot be null or empty");
            }

            var tokenParts = idToken.Split('.');
            if (tokenParts.Length != 3)
            {
                throw new ArgumentException($"Invalid ID token format. Expected 3 segments, got {tokenParts.Length}. Token should be in format: header.payload.signature");
            }

            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            var uid = decodedToken.Uid;

            var user = await GetUserByFirebaseUidAsync(uid);
            if (user == null)
            {
                throw new InvalidOperationException($"User with Firebase UID {uid} not found");
            }

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
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException($"Invalid ID token: {ex.Message}", ex);
        }
        catch (FirebaseAuthException ex)
        {
            throw new InvalidOperationException($"Firebase authentication failed: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Authentication failed: {ex.Message}", ex);
        }
    }

    public async Task<UserInfoDto> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            // Проверяем формат токена
            if (string.IsNullOrEmpty(registerDto.IdToken))
            {
                throw new ArgumentException("ID token cannot be null or empty");
            }

            var tokenParts = registerDto.IdToken.Split('.');
            if (tokenParts.Length != 3)
            {
                throw new ArgumentException($"Invalid ID token format. Expected 3 segments, got {tokenParts.Length}. Token should be in format: header.payload.signature");
            }

            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(registerDto.IdToken);
            var uid = decodedToken.Uid;

            var existingUser = await GetUserByFirebaseUidAsync(uid);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"User with Firebase UID {uid} already exists");
            }

            var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            var user = new User
            {
                FirebaseUid = uid,
                Email = userRecord.Email ?? string.Empty,
                DisplayName = userRecord.DisplayName ?? string.Empty,
                PhotoUrl = userRecord.PhotoUrl ?? string.Empty,
                EmailVerified = false, // TODO: change this verification
                Role = registerDto.Role,
                LastLoginAt = DateTime.UtcNow,
                IsActive = true
            };

            try
            {
                await _userRepository.CreateAsync(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create user in database: {ex.Message}", ex);
            }

            if (registerDto.Role == UserRole.Coach)
            {
                var coach = new Coach
                {
                    CoachConsId = GenerateCoachId(),
                    FirstName = registerDto.FirstName ?? string.Empty,
                    LastName = registerDto.LastName ?? string.Empty,
                    BirthDate = registerDto.BirthDate,
                    Description = registerDto.Description ?? string.Empty,
                    Email = user.Email,
                    PictureUrl = registerDto.PictureUrl ?? user.PhotoUrl,
                    LinkedInUrl = registerDto.LinkedInUrl,
                    GitHubUrl = registerDto.GitHubUrl,
                    AverageRating = 0,
                    User = user
                };

                try
                {
                    await _coachRepository.CreateAsync(coach);
                    user.CoachId = coach.Id;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to create coach in database: {ex.Message}", ex);
                }
            }
            else if (registerDto.Role == UserRole.Student)
            {
                var student = new Student
                {
                    StudentConsId = GenerateStudentId(),
                    FirstName = registerDto.FirstName ?? string.Empty,
                    LastName = registerDto.LastName ?? string.Empty,
                    BirthDate = registerDto.BirthDate,
                    Email = user.Email,
                    PictureUrl = registerDto.PictureUrl ?? user.PhotoUrl,
                    LinkedInUrl = registerDto.LinkedInUrl,
                    GitHubUrl = registerDto.GitHubUrl
                };

                try
                {
                    Console.WriteLine($"Creating student with StudentConsId: {student.StudentConsId}");
                    Console.WriteLine($"Student Email: {student.Email}");
                    Console.WriteLine($"Student FirstName: {student.FirstName}");
                    Console.WriteLine($"Student LastName: {student.LastName}");
                    Console.WriteLine($"Student BirthDate: {student.BirthDate}");
                    
                    await _studentRepository.CreateAsync(student);
                    user.StudentId = student.Id;
                    
                    Console.WriteLine($"Student created successfully with ID: {student.Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating student: {ex.Message}");
                    Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                    throw new InvalidOperationException($"Failed to create student in database: {ex.Message}", ex);
                }
            }

            try
            {
                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update user in database: {ex.Message}", ex);
            }

            return await GetUserInfoAsync(uid);
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException($"Invalid ID token: {ex.Message}", ex);
        }
        catch (FirebaseAuthException ex)
        {
            throw new InvalidOperationException($"Firebase authentication failed: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Registration failed: {ex.Message}", ex);
        }
    }

    public async Task<UserInfoDto?> GetUserInfoAsync(string firebaseUid)
    {
        try
        {
            var user = await GetUserByFirebaseUidAsync(firebaseUid);

            if (user == null)
            {
                return null;
            }

            var userInfo = new UserInfoDto
            {
                Id = user.Id,
                FirebaseUid = user.FirebaseUid,
                Email = user.Email,
                DisplayName = user.DisplayName,
                PhotoUrl = user.PhotoUrl,
                EmailVerified = user.EmailVerified,
                Role = user.Role,
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive,
                CoachId = user.CoachId,
                StudentId = user.StudentId
            };

            if (user.Role == UserRole.Coach && user.CoachId.HasValue)
            {
                var coach = await _coachRepository.GetAsync(user.CoachId.Value);
                if (coach != null)
                {
                    userInfo.FirstName = coach.FirstName;
                    userInfo.LastName = coach.LastName;
                    userInfo.BirthDate = coach.BirthDate;
                    userInfo.Description = coach.Description;
                    userInfo.LinkedInUrl = coach.LinkedInUrl;
                    userInfo.GitHubUrl = coach.GitHubUrl;
                }
            }
            else if (user.Role == UserRole.Student && user.StudentId.HasValue)
            {
                var student = await _studentRepository.GetAsync(user.StudentId.Value);
                if (student != null)
                {
                    userInfo.FirstName = student.FirstName;
                    userInfo.LastName = student.LastName;
                    userInfo.BirthDate = student.BirthDate;
                    userInfo.LinkedInUrl = student.LinkedInUrl;
                    userInfo.GitHubUrl = student.GitHubUrl;
                }
            }

            return userInfo;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to get user info: {ex.Message}", ex);
        }
    }

    public async Task<UserInfoDto?> GetUserByRoleAsync(string firebaseUid, UserRole role)
    {
        var user = await GetUserByFirebaseUidAsync(firebaseUid);

        if (user == null)
        {
            return null;
        }
        
        if (user.Role != role)
        {
            throw new InvalidOperationException($"User role {user.Role} does not match expected role {role}");
        }
        
        return await GetUserInfoAsync(firebaseUid);
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

    public async Task<User?> GetUserByFirebaseUidAsync(string firebaseUid)
    {
        return _userRepository.Get(u => u.FirebaseUid == firebaseUid).FirstOrDefault();
    }

    private async Task<User> CreateUserAsync(UserInfoDto userInfo)
    {
        var user = new User
        {
            FirebaseUid = userInfo.FirebaseUid,
            Email = userInfo.Email,
            DisplayName = userInfo.DisplayName,
            PhotoUrl = userInfo.PhotoUrl,
            EmailVerified = userInfo.EmailVerified,
            Role = UserRole.Student, // По умолчанию студент
            LastLoginAt = DateTime.UtcNow,
            IsActive = true
        };

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

    private async Task<string> GenerateAccessTokenAsync(User user)
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

    private async Task<string> GenerateRefreshTokenAsync(User user)
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

    private string GenerateCoachId()
    {
        return $"0001{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";
    }

    private string GenerateStudentId()
    {
        return $"0003{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";
    }

    Task<User> IFirebaseAuthService.CreateUserAsync(UserInfoDto userInfo)
    {
        throw new NotImplementedException();
    }

    Task IFirebaseAuthService.UpdateUserLastLoginAsync(string firebaseUid)
    {
        throw new NotImplementedException();
    }
} 
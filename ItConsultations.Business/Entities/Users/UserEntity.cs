using ItConsultations.Business.Entities.RefreshTokens;
using ItConsultations.Business.SharedTypes.Enums.System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ItConsultations.Business.Entities.Users;

public class UserEntity : Entity<long>
{
    [Required]
    [MaxLength(128)]
    public string FirebaseUid { get; set; }

    [Required]
    [MaxLength(36)]
    public string ConsId { get; set; }

    [Required]
    public long UserId { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    [Required]
    [MaxLength(255)]
    public string Email { get; set; }

    public string Username { get; set; }

    [MaxLength(500)]
    public string PhotoUrl { get; set; }

    [Required]
    public UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime LastLoginAt { get; set; }

    [JsonIgnore]
    public List<RefreshToken> RefreshTokens { get; set; } = new();

    public string? LinkedInUrl { get; set; }

    public string? GitHubUrl { get; set; }

    public string? PhoneNumber { get; set; }
}
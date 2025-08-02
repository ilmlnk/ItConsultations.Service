using ItConsultations.Business.Entities.Coaches;
using ItConsultations.Business.Entities.RefreshTokens;
using ItConsultations.Business.Entities.Students;
using ItConsultations.Business.SharedTypes.Enums.System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ItConsultations.Business.Entities.Users;

public class UserEntity : Entity<long>
{
    [Required]
    [MaxLength(128)]
    public string FirebaseUid { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string PhotoUrl { get; set; } = string.Empty;

    public bool EmailVerified { get; set; }

    [Required]
    public UserRole Role { get; set; } = UserRole.Student;

    public DateTime LastLoginAt { get; set; }

    public bool IsActive { get; set; } = true;

    public long? CoachId { get; set; }

    public long? StudentId { get; set; }

    [JsonIgnore]
    public List<RefreshToken> RefreshTokens { get; set; } = new();

    [JsonIgnore]
    public Student? Student { get; set; }

    [JsonIgnore]
    public Coach? Coach { get; set; }
}
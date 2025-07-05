using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Dtos.AuthDtos;

public class UserInfoDto
{
    public long Id { get; set; }

    public string FirebaseUid { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string PhotoUrl { get; set; } = string.Empty;

    public bool EmailVerified { get; set; }

    public UserRole Role { get; set; }

    public DateTime LastLoginAt { get; set; }

    public bool IsActive { get; set; }

    public long? CoachId { get; set; }

    public long? StudentId { get; set; }
    
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? Description { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? GitHubUrl { get; set; }
} 
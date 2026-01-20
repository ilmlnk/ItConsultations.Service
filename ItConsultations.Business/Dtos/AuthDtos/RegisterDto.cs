using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Dtos.AuthDtos;

public class RegisterDto
{
    public string FirebaseUid { get; set; }

    public string? ConsId { get; set; }
    
    public string AccessToken { get; set; }

    public UserRole Role { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    public string Username { get; set; }

    public string? Email { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? PictureUrl { get; set; }

    public string? LinkedInUrl { get; set; }
    
    public string? GitHubUrl { get; set; }

    public string? PhoneNumber { get; set; }
} 
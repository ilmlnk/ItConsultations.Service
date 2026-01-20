using ItConsultations.Business.Entities.RefreshTokens;
using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Dtos.UserDtos;

public class UserDto
{
    public string FirebaseUid { get; set; }
    
    public string ConsId { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public string Email { get; set; }
    
    public string Username { get; set; }
    
    public string PhotoUrl { get; set; }
    
    public UserRole Role { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public DateTime LastLoginAt { get; set; }
    
    public List<RefreshToken> RefreshTokens { get; set; }
    
    public string LinkedInUrl { get; set; }
    
    public string GitHubUrl { get; set; }
    
    public string TelegramUrl { get; set; }
    
    public string PhoneNumber { get; set; }
}

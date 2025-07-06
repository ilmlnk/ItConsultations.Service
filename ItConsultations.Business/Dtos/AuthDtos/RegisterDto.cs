using System.ComponentModel.DataAnnotations;
using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Dtos.AuthDtos;

public class RegisterDto
{
    [Required]
    public string IdToken { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    public string? Description { get; set; }

    public string? PictureUrl { get; set; }

    public string? LinkedInUrl { get; set; }
    
    public string? GitHubUrl { get; set; }
} 
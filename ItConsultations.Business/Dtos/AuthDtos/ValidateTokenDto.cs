using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Dtos.AuthDtos;

public class ValidateTokenDto
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
} 
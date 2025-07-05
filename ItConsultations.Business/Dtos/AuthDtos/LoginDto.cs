using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Dtos.AuthDtos;

public class LoginDto
{
    [Required]
    public string IdToken { get; set; } = string.Empty;
}
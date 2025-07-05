using System.ComponentModel.DataAnnotations;

public class LogoutDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
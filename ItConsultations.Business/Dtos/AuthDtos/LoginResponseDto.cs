namespace ItConsultations.Business.Dtos.AuthDtos;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public UserInfoDto User { get; set; } = new();
}

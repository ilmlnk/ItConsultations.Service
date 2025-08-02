using ItConsultations.Business.Entities.Users;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Entities.RefreshTokens;

public class RefreshToken : Entity<long>
{
    [Required]
    [MaxLength(500)]
    public string Token { get; set; } = string.Empty;

    [Required]
    public long UserId { get; set; }

    [JsonIgnore]
    public UserEntity User { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime? RevokedAt { get; set; }

    [MaxLength(50)]
    public string? RevokedBy { get; set; }
}
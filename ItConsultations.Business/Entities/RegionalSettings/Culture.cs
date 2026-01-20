using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Entities.RegionalSettings;

public class Culture : Entity<string>
{
    [Key]
    [Required]
    [MaxLength(15)]
    public override string Id { get; set; }
    
    [Required]
    [MaxLength(60)]
    public string DisplayName { get; set; }
}
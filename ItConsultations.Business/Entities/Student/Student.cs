using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Entities.Student;

public class Student : Entity<long>
{
    [Required]
    [MaxLength(32)]
    public long Id { get; set; }

    [MaxLength(32)]
    public string ConsId { get; set; }
    [Required]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}

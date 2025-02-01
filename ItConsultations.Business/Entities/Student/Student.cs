using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItConsultations.Business.Entities.Student;

public class Student : Entity<long>
{
    [Required]
    [MaxLength(64)]
    public string Id {  get; set; }
    [Required]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

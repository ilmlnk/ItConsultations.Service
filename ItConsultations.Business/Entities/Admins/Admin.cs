namespace ItConsultations.Business.Entities.Admins;

public class Admin : Entity<long>
{
    public long Id { get; set; }

    public string AdminConsId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }
}

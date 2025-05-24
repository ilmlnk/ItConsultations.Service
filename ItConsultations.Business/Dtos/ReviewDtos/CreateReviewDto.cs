using ItConsultations.Business.Entities;

namespace ItConsultations.Business.Dtos.ReviewDtos;

public class CreateReviewDto
{
    public long Id { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int Rating { get; set; }

    public Entity<long> Reviewer { get; set; }
}

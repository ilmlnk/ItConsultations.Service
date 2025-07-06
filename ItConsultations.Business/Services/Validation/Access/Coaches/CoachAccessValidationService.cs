using ItConsultations.Business.Services.CoachService;
using ItConsultations.Utilities.Guards;
using ItConsultations.Utilities.Validation.Access;

namespace ItConsultations.Business.Services.Validation.Access.Coaches;

public class CoachAccessValidationService : AccessValidationServiceBase, ICoachAccessValidationService
{
    private readonly ICoachService _coachService;
    public CoachAccessValidationService(ICoachService coachService)
    {
        _coachService = coachService;
    }

    public void ValidateCoachAccessAsync(long id)
    {
        var coach = _coachService.GetById(id.ToString());

        if (coach == null)
        {
            throw new ArgumentException($"Coach with id {id} not found");
        }

        Guard.That(coach.Email == null, "Coach does not have a specified email.");
        Guard.That(coach.Username == null, "Coach does not have a specified username.");
    }
}

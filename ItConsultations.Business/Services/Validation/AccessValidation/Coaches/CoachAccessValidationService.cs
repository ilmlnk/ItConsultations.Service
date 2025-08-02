using ItConsultations.Business.Services.CoachService;
using ItConsultations.Utilities.Validation.Access;

namespace ItConsultations.Business.Services.Validation.AccessValidation.Coaches;

public class CoachAccessValidationService : AccessValidationServiceBase, ICoachAccessValidationService
{
    private readonly ICoachService _coachService;
    public CoachAccessValidationService(ICoachService coachService)
    {
        _coachService = coachService;
    }

    public async void ValidateCoachAccessAsync(long coachId)
    {
        var coach = await _coachService.GetAsync(coachId);

        if (coach == null)
        {
            throw new ArgumentException($"Coach with id {coachId} not found");
        }
    }
}

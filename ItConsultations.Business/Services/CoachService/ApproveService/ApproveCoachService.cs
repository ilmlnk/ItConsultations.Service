
namespace ItConsultations.Business.Services.CoachService.ApproveService;

public class ApproveCoachService : IApproveCoachService
{
    private readonly ICoachService _coachService;

    public ApproveCoachService()
    {

    }

    public Task ApproveCoachAsync(string coachId)
    {
        throw new NotImplementedException();
    }

    public Task ApproveCoachByFirebaseUid(string firebaseUid)
    {
        throw new NotImplementedException();
    }

    public Task ApproveCoachByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task DeclineCoachAsync(string coachId)
    {
        throw new NotImplementedException();
    }

    public Task DeclineCoachByFirebaseUidAsync(string firebaseUid)
    {
        throw new NotImplementedException();
    }

    public Task DeclineCoachByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}

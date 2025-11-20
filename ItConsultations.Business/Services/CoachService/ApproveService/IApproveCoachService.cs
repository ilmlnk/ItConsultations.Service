namespace ItConsultations.Business.Services.CoachService.ApproveService;

public interface IApproveCoachService
{
    Task ApproveCoachAsync(string coachConsId);

    Task ApproveCoachByIdAsync(int id);

    Task ApproveCoachByFirebaseUid(string firebaseUid);

    Task DeclineCoachAsync(string coachConsId);

    Task DeclineCoachByIdAsync(int id);

    Task DeclineCoachByFirebaseUidAsync(string firebaseUid);
}

using ItConsultations.Business.Entities.UnsubscribeResult;

namespace ItConsultations.Business.Services.UnsubscribeService;

public interface IUnsubscribeService
{
    Task<string> GenerateSecureToken(long userId);

    Task<UnsubscribeResult> ProcessUnsubscribeAsync(string token);
}

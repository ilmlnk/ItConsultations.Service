
using ItConsultations.Business.Entities.UnsubscribeResult;

namespace ItConsultations.Business.Services.UnsubscribeService;

public class UnsubscribeService : IUnsubscribeService
{
    public Task<string> GenerateSecureToken(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<UnsubscribeResult> ProcessUnsubscribeAsync(string token)
    {
        throw new NotImplementedException();
    }
}

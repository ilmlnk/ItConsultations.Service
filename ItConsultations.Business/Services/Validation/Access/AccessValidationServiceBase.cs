
namespace ItConsultations.Utilities.Validation.Access;

public abstract class AccessValidationServiceBase : ValidationService
{
    public override void Validate()
    {
        if (Errors.Any())
        {
            Clear();
        }
    }
}

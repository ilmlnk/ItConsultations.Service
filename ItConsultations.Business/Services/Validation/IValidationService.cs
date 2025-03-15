namespace ItConsultations.Business.Services.Validation;

public interface IValidationService
{
    Task ValidateAsync();

    void Validate();

    Task<bool> IsValidAsync();

    bool IsValid();

    void Clear();
}

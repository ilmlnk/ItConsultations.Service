namespace ItConsultations.Business.Services.Validation;

public class ValidationService : IValidationService
{
    private readonly IList<Task> _tasks;

    protected const string _emptyField = "Field is empty";

    protected ValidationService() { }

    public virtual async Task ValidateAsync()
    {
        foreach (var task in _tasks)
        {
            await task;
        }

        ValidateNoErrors();
    }

    protected virtual void ValidateNoErrors()
    {

    }

    public void Validate()
    {
        ValidateNoErrors();
    }

    public virtual async Task<bool> IsValidAsync()
    {
        foreach(var task in _tasks)
        {
            await task;
        }

        return HasNoErrors();
    }

    public bool IsValid()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    // TODO: Add other condition for isValid variable
    private bool HasNoErrors()
    {
        var isValid = true;
        Clear();

        return isValid;
    }
}

using ItConsultations.Business.Entities.ErrorObject;
using System.Collections.ObjectModel;

namespace ItConsultations.Business.Services.Validation;

public class ValidationService : IValidationService
{
    private readonly IList<ErrorObject> _errors;
    private readonly IList<Task> _tasks;

    protected ValidationService() { }

    public IReadOnlyCollection<ErrorObject> Errors => new ReadOnlyCollection<ErrorObject>(_errors);

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

    protected void Expect(Func<bool> condition, ErrorObject error)
    {
        

        if (!condition())
        {
            _errors.Add(error);
        }
    }

    public virtual void Validate()
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

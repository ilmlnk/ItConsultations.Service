using ItConsultations.Business.Entities.ErrorObject;
using ItConsultations.Business.Exceptions;
using ItConsultations.Utilities.Guards;
using System.Collections.ObjectModel;

namespace ItConsultations.Business.Services.Validation;

public class ValidationService : IValidationService
{
    private readonly IList<ErrorObject> _errors;
    private readonly IList<Task> _tasks;

    public ValidationService()
    {
        _errors = new List<ErrorObject>();
        _tasks = new List<Task>();
    }

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
        if (!_errors.Any())
        {
            return;
        }

        throw new ConsultationsValidationException(_errors);
    }

    protected void Expect(Func<bool> condition, ErrorObject error)
    {
        Guard.NotNull(condition);
        Guard.NotNull(error);

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
        return !_errors.Any();
    }

    public void Clear()
    {
        _errors.Clear();
        _tasks.Clear();
    }

    // TODO: Add other condition for isValid variable
    private bool HasNoErrors()
    {
        return !_errors.Any();
    }
}

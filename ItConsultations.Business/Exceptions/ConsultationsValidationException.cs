using ItConsultations.Business.Entities.ErrorObject;

namespace ItConsultations.Business.Exceptions;

public class ConsultationsValidationException : Exception
{
    public ConsultationsValidationException() : base("Consultations validation model contains error")
    {

    }

    public ConsultationsValidationException(ErrorObject errorObject) : base("Consultations validation model contains error")
    {
        Errors = [errorObject];
    }

    public ConsultationsValidationException(IEnumerable<ErrorObject> errors) : base("Consultations validation model contains error")
    {
        Errors = errors;
    }

    public IEnumerable<ErrorObject> Errors { get; set; }
}

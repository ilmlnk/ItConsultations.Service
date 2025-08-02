namespace ItConsultations.Business.Entities.ErrorObjects;

public class ErrorObject
{
    public string ErrorName { get; set; }

    public object ObjectValue { get; set; }

    public string ErrorMessage { get; set; }
}

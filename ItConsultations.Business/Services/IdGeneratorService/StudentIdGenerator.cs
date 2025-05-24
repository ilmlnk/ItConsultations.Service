using ItConsultations.Business.DataAccess.Interfaces;

namespace ItConsultations.Business.Services.IdGeneratorService;

public class StudentIdGenerator<T> : IIdGenerator<T> where T : class
{
    public string GenerateConsId()
    {
        return GenerateConsId("STNT");
    }

    private string GenerateConsId(string prefix)
    {
        var randomString = GenerateRandomString(20);
        return $"0{prefix}{randomString}";
    }

    private string GenerateRandomString(int length)
    {
        return null;
    }
}

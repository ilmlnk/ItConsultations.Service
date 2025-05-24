using AutoFixture;
using ItConsultations.Business.AutoMapperConfiguration;

namespace ItConsultations.Tests;

public class BaseTest
{
    public Fixture fixture { get; set; }

    protected BaseTest()
    {
        fixture = new Fixture();
        MapperManager.Initialize
    }
}

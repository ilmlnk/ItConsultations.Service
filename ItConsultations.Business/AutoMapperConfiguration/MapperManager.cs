using AutoMapper;

namespace ItConsultations.Business.AutoMapperConfiguration;

public class MapperManager
{
    public static void Initialize(Action<IMapperConfigurationExpression> config)
    {
        var configurationProvider = new MapperConfiguration(config);
        Instance = new Mapper(configurationProvider);
    }

    public static TDestination Map<TDestination>(object source)
    {
        return Instance.Map<TDestination>(source);
    }

    public static TDestination Map<TSource, TDestination>(TSource source)
    {
        return Instance.Map<TSource, TDestination>(source);
    }

    public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return Instance.Map(source, destination);
    }

    public static TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts)
    {
        return Instance.Map(source, opts);
    }

    public static void AssertConfigurationIsValid()
    {
        Instance.ConfigurationProvider.AssertConfigurationIsValid();
    }

    private static Mapper Instance { get; set; }
}

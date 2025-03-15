using System.Runtime.CompilerServices;
using AutoMapper;
using AutoMapper.Configuration;

namespace ItConsultations.Business.AutoMapperConfiguration;

internal static class AutoMapperExtensions
{
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_TypeMapActions")]
    private static extern List<Action<TypeMap>> GetTypeMapActions(TypeMapConfiguration typeMapConfiguration);

    public static void ForAllOtherMembers<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, Action<IMemberConfigurationExpression<TSource, TDestination, object>> memberOptions)
    {
        var typeMapConfiguration = (TypeMapConfiguration)expression;
        var typeMapActions = GetTypeMapActions(typeMapConfiguration);

        typeMapActions.Add(typeMap =>
        {
            var destinationTypeDetails = typeMap.DestinationTypeDetails;

            foreach (var accessor in destinationTypeDetails.WriteAccessors.Where(m => typeMapConfiguration.GetDestinationMemberConfiguration(m) == null))
            {
                expression.ForMember(accessor.Name, memberOptions);
            }
        });
    }
}

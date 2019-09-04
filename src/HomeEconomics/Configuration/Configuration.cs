using System;
using AutoMapper;
using Domain.Movements;
using HomeEconomics.Features.Movements;

namespace HomeEconomics.Configuration
{
    internal static class Configuration
    {
        internal static Action<IMapperConfigurationExpression> ConfigureAutoMapper()
        {
            return mapperConfigurationExpression =>
            {
                mapperConfigurationExpression
                    .CreateMap<Movement, Index.Result.Movement>()
                    .ForMember(destination => destination.Type,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => (int) source.Type))
                    .ForMember(destination => destination.FrequencyType,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => (int) source.Frequency.Type));
            };
        }
    }
}
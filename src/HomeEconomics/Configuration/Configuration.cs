using System;
using System.Linq;
using AutoMapper;
using Domain.MovementMonth;
using Domain.Movements;
using HomeEconomics.Features.MovementMonths;
using Index = HomeEconomics.Features.Movements.Index;

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
                            memberConfigurationExpression.MapFrom(source => (int) source.Frequency.Type))
                    .ForMember(destination => destination.FrequencyMonth,
                        memberConfigurationExpression => memberConfigurationExpression.MapFrom(source => source.Frequency.Type == FrequencyType.Yearly
                            ? Array.IndexOf(source.Frequency.Months, true) + 1
                            : 0));

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, Create.Result>()
                    .ForMember(destination => destination.PendingTotalExpenses,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid).Sum(mm => mm.Amount)))
                    .ForMember(destination => destination.PendingTotalIncomes,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid).Sum(mm => mm.Amount)));

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, Detail.Result>()
                    .ForMember(destination => destination.PendingTotalExpenses,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid).Sum(mm => mm.Amount)))
                    .ForMember(destination => destination.PendingTotalIncomes,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid).Sum(mm => mm.Amount)));

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, PayMonthMovement.Result>()
                    .ForMember(destination => destination.PendingTotalExpenses,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid).Sum(mm => mm.Amount)))
                    .ForMember(destination => destination.PendingTotalIncomes,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid).Sum(mm => mm.Amount)));

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, UnPayMonthMovement.Result>()
                    .ForMember(destination => destination.PendingTotalExpenses,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid).Sum(mm => mm.Amount)))
                    .ForMember(destination => destination.PendingTotalIncomes,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid).Sum(mm => mm.Amount)));

                mapperConfigurationExpression
                    .CreateMap<MonthMovement, MovementMonthResponse.MonthMovementResult>();

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, UpdateMonthMovementAmount.Result>()
                    .ForMember(destination => destination.PendingTotalExpenses,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid).Sum(mm => mm.Amount)))
                    .ForMember(destination => destination.PendingTotalIncomes,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid).Sum(mm => mm.Amount)));

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, AddMonthMovement.Result>()
                    .ForMember(destination => destination.PendingTotalExpenses,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid).Sum(mm => mm.Amount)))
                    .ForMember(destination => destination.PendingTotalIncomes,
                        memberConfigurationExpression =>
                            memberConfigurationExpression.MapFrom(source => source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid).Sum(mm => mm.Amount)));
            };
        }
    }
}
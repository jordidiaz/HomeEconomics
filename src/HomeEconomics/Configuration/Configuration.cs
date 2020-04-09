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
                    .AfterMap((source, destination) => destination.MonthMovements = destination.MonthMovements.OrderBy(mm => mm.Name).ToArray())
                    .ForPath(destination => destination.Status.AccountAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().AccountAmount))
                    .ForPath(destination => destination.Status.CashAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().CashAmount))
                    .ForPath(destination => destination.Status.PendingTotalExpenses,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid)
                                .Sum(mm => mm.Amount)))
                    .ForPath(destination => destination.Status.PendingTotalIncomes,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid)
                                .Sum(mm => mm.Amount)));

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, Detail.Result>()
                    .AfterMap((source, destination) => destination.MonthMovements = destination.MonthMovements.OrderBy(mm => mm.Name).ToArray())
                    .ForPath(destination => destination.Status.AccountAmount,
                        expression => expression.MapFrom(source => source.Statuses.Count > 0 ? source.Statuses.OrderByDescending(s => s.Day).First().AccountAmount : 0))
                    .ForPath(destination => destination.Status.CashAmount,
                        expression => expression.MapFrom(source => source.Statuses.Count > 0 ? source.Statuses.OrderByDescending(s => s.Day).First().CashAmount : 0))
                    .ForPath(destination => destination.Status.PendingTotalExpenses,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid)
                                .Sum(mm => mm.Amount)))
                    .ForPath(destination => destination.Status.PendingTotalIncomes,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid)
                                .Sum(mm => mm.Amount)));

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, PayMonthMovement.Result>()
                    .AfterMap((source, destination) => destination.MonthMovements = destination.MonthMovements.OrderBy(mm => mm.Name).ToArray())
                    .ForPath(destination => destination.Status.AccountAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().AccountAmount))
                    .ForPath(destination => destination.Status.CashAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().CashAmount))
                    .ForPath(destination => destination.Status.PendingTotalExpenses,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid)
                                .Sum(mm => mm.Amount)))
                    .ForPath(destination => destination.Status.PendingTotalIncomes,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid)
                                .Sum(mm => mm.Amount)));

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, UnPayMonthMovement.Result>()
                    .AfterMap((source, destination) => destination.MonthMovements = destination.MonthMovements.OrderBy(mm => mm.Name).ToArray())
                    .ForPath(destination => destination.Status.AccountAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().AccountAmount))
                    .ForPath(destination => destination.Status.CashAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().CashAmount))
                    .ForPath(destination => destination.Status.PendingTotalExpenses,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid)
                                .Sum(mm => mm.Amount)))
                    .ForPath(destination => destination.Status.PendingTotalIncomes,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid)
                                .Sum(mm => mm.Amount)));

                mapperConfigurationExpression
                    .CreateMap<MonthMovement, MovementMonthResponse.MonthMovementResult>();

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, UpdateMonthMovementAmount.Result>()
                    .AfterMap((source, destination) => destination.MonthMovements = destination.MonthMovements.OrderBy(mm => mm.Name).ToArray())
                    .ForPath(destination => destination.Status.AccountAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().AccountAmount))
                    .ForPath(destination => destination.Status.CashAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().CashAmount))
                    .ForPath(destination => destination.Status.PendingTotalExpenses,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid)
                                .Sum(mm => mm.Amount)))
                    .ForPath(destination => destination.Status.PendingTotalIncomes,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid)
                                .Sum(mm => mm.Amount)));

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, AddMonthMovement.Result>()
                    .AfterMap((source, destination) => destination.MonthMovements = destination.MonthMovements.OrderBy(mm => mm.Name).ToArray())
                    .ForPath(destination => destination.Status.AccountAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().AccountAmount))
                    .ForPath(destination => destination.Status.CashAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().CashAmount))
                    .ForPath(destination => destination.Status.PendingTotalExpenses,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid)
                                .Sum(mm => mm.Amount)))
                    .ForPath(destination => destination.Status.PendingTotalIncomes,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid)
                                .Sum(mm => mm.Amount)));

                mapperConfigurationExpression
                    .CreateMap<MovementMonth, AddStatus.Result>()
                    .AfterMap((source, destination) => destination.MonthMovements = destination.MonthMovements.OrderBy(mm => mm.Name).ToArray())
                    .ForPath(destination => destination.Status.AccountAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().AccountAmount))
                    .ForPath(destination => destination.Status.CashAmount,
                        expression => expression.MapFrom(source => source.Statuses.OrderByDescending(s => s.Day).First().CashAmount))
                    .ForPath(destination => destination.Status.PendingTotalExpenses,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Expense && !mm.Paid)
                                .Sum(mm => mm.Amount)))
                    .ForPath(destination => destination.Status.PendingTotalIncomes,
                        expression => expression.MapFrom(source =>
                            source.MonthMovements.Where(mm => mm.Type == MovementType.Income && !mm.Paid)
                                .Sum(mm => mm.Amount)));
            };
        }
    }
}
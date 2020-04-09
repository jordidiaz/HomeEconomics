using System.Linq;
using AutoMapper;
using Domain.MovementMonth;
using Domain.Movements;
using HomeEconomics.Features.MovementMonths;

namespace HomeEconomics.AutoMapper
{
    public class MovementMonthToResultProfile : Profile
    {
        public MovementMonthToResultProfile()
        {
            CreateMap<MovementMonth, MovementMonthResponse>()
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
        }
    }
}
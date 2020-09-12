using AutoMapper;
using Domain.MovementMonth;

namespace HomeEconomics.Features.MovementMonths
{
    public class MovementMonthResponse
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public bool NextMovementMonthExists { get; set; }

        public StatusResult Status { get; set; }

        public MonthMovementResult[] MonthMovements { get; set; }

        public class MonthMovementResult
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public decimal Amount { get; set; }

            public int Type { get; set; }

            public bool Paid { get; set; }
        }

        public class StatusResult
        {
            public decimal PendingTotalExpenses { get; set; }

            public decimal PendingTotalIncomes { get; set; }

            public decimal AccountAmount { get; set; }

            public decimal CashAmount { get; set; }
        }
    }

    public class MovementMonthResponseProfile : Profile
    {
        public MovementMonthResponseProfile()
        {
            CreateMap<MonthMovement, MovementMonthResponse.MonthMovementResult>();
        }
    }
}

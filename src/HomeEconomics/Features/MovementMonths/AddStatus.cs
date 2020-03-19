using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.MovementMonth;
using FluentValidation;
using HomeEconomics.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.MovementMonths
{
    public class AddStatus
    {
        public class Command : IRequest<Result>
        {
            public int Year { get; set; }

            public Month Month { get; set; }
            
            public decimal AccountAmount { get; set; }

            public decimal CashAmount { get; set; }
        }

        public class Result : MovementMonthResponse
        {

        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.Year).GreaterThanOrEqualTo(DateTime.Now.Year);
                RuleFor(command => command.Month).Must(Enums.IsAValidEnumValue);
                RuleFor(command => command.AccountAmount).GreaterThanOrEqualTo(Constants.MinAmount);
                RuleFor(command => command.CashAmount).GreaterThanOrEqualTo(Constants.MinAmount);
            }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly HomeEconomicsDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(HomeEconomicsDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var movementMonth = await _dbContext
                    .MovementMonths
                    .Include(mm => mm.MonthMovements)
                    .Include(mm => mm.Statuses)
                    .SingleOrDefaultAsync(mm => mm.Year == request.Year && mm.Month == request.Month, cancellationToken: cancellationToken);

                if (movementMonth is null)
                {
                    throw new InvalidOperationException(Properties.Messages.MovementMonthNotExists);
                }

                var day = IsCurrentMonth(request.Year, (int)request.Month)
                    ? DateTime.Now.Day
                    : 0;

                movementMonth.AddStatus(day, request.AccountAmount, request.CashAmount);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return _mapper.Map<Result>(movementMonth);
            }

            private static bool IsCurrentMonth(int year, int month)
            {
                return DateTime.Now.Year == year && DateTime.Now.Month == month;
            }
        }
    }
}

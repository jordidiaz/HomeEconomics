using AutoMapper;
using Domain.Movements;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeEconomics.Features.Movements
{
    public class Index
    {
        public record Query : IRequest<Result>
        {
        }

        public record Result
        {
            public Movement[] Movements { get; init; } = { };

            public record Movement
            {
                public int Id { get; init; }

                public string Name { get; init; } = string.Empty;

                public decimal Amount { get; init; }

                public int Type { get; init; }

                public int FrequencyType { get; init; }

                public bool[] FrequencyMonths { get; init; } = new bool[12];

                public int FrequencyMonth { get; init; }
            }
        }

        public class MovementsIndexProfile : Profile
        {
            public MovementsIndexProfile()
            {
                CreateMap<Movement, Result.Movement>()
                .ForMember(destination => destination.Type,
                    memberConfigurationExpression =>
                        memberConfigurationExpression.MapFrom(source => (int)source.Type))
                .ForMember(destination => destination.FrequencyType,
                    memberConfigurationExpression =>
                        memberConfigurationExpression.MapFrom(source => (int)source.Frequency.Type))
                .ForMember(destination => destination.FrequencyMonth,
                    memberConfigurationExpression => memberConfigurationExpression.MapFrom(source => source.Frequency.Type == FrequencyType.Yearly
                        ? Array.IndexOf(source.Frequency.Months.ToArray(), true) + 1
                        : 0));
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly HomeEconomicsDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(HomeEconomicsDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var queryable = _dbContext.GetMovements()
                    .OrderBy(m => m.Name)
                    .AsQueryable();
                
                var movements = _mapper
                    .ProjectTo<Result.Movement>(queryable)
                    .ToArray();

                return Task.FromResult(new Result
                {
                    Movements = movements
                });
            }
        }
    }
}
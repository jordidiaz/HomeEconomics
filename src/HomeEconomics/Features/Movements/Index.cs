using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.Movements
{
    public class Index
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public Movement[] Movements { get; set; }

            public class Movement
            {
                public int Id { get; set; }

                public string Name { get; set; }

                public decimal Amount { get; set; }

                public int Type { get; set; }

                public int FrequencyType { get; set; }

                public bool[] FrequencyMonths { get; set; }
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

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var movements = await _mapper.ProjectTo<Result.Movement>(_dbContext.Movements
                        .Include(m => m.Frequency)
                        .OrderBy(m => m.Name))
                        .ToArrayAsync(cancellationToken: cancellationToken);

                return new Result
                {
                    Movements = movements
                };
            }
        }
    }
}
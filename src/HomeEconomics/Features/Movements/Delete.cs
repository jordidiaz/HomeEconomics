using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.Features.Movements
{
    public class Delete
    {
        public class Command : IRequest<bool>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly HomeEconomicsDbContext _dbContext;

            public Handler(HomeEconomicsDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var movement = await _dbContext.Movements
                    .SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken: cancellationToken);

                if (movement == null)
                {
                    return false;
                }

                _dbContext.Movements.Remove(movement);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
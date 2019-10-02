using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using Xunit;
using System.Net.Http;

namespace HomeEconomics.IntegrationTests.Features.Movements
{
    public class EditTest : IntegrationTestBase
    {
        private readonly Edit.Command _command;

        private string _uri;

        public EditTest(Fixture fixture) : base(fixture)
        {
            _command = new Edit.Command
            {
                Id = 42,
                Name = "EPSV",
                Amount = 50m,
                Type = MovementType.Expense,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            };
        }

        [Fact]
        public async Task Should_Return_204_NoContent()
        {
            _uri = "api/movements/42";

            var response = await HttpClient
                .PutAsync(_uri, _command);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        public class Handler : IRequestHandler<Edit.Command, Unit>
        {
            public Task<Unit> Handle(Edit.Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(Unit.Value);
            }
        }
    }
}
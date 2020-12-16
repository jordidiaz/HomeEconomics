using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.Movements
{
    public class CreateTests : IntegrationTestBase
    {
        private Create.Command _command;

        private const string Uri = "api/movements";

        public CreateTests(Fixture fixture) : base(fixture)
        {
            _command = new Create.Command
            {
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
        public async Task Should_Return_200_Ok()
        {
            var response = await HttpClient
                .PostAsync(Uri, _command);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_400_BadRequest()
        {
            _command = new Create.Command
            {
                Name = string.Empty,
                Amount = 50m,
                Type = MovementType.Expense,
                Frequency = new Create.Frequency
                {
                    Type = FrequencyType.Monthly
                }
            };

            var response = await HttpClient
                .PostAsync(Uri, _command);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public class Handler : IRequestHandler<Create.Command, int>
        {
            public Task<int> Handle(Create.Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(1);
            }
        }
    }
}
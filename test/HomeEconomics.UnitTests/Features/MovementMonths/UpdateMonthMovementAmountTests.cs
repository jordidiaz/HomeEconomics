using FluentAssertions;
using FluentValidation.TestHelper;
using HomeEconomics.Features.MovementMonths;
using JetBrains.Annotations;
using Xunit;

namespace HomeEconomics.UnitTests.Features.MovementMonths;

[UsedImplicitly]
public class UpdateMonthMovementAmountTests
{
    public class CommandValidatorTests
    {
        private readonly UpdateMonthMovementAmount.Validator _sut = new();

        [Fact]
        public void Should_Have_Error_If_Amount_Invalid()
        {
            var result = _sut.TestValidate(new UpdateMonthMovementAmount.Command(1, 1, -0.1m));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Not_Have_Error_If_Amount_Valid()
        {
            var result = _sut.TestValidate(new UpdateMonthMovementAmount.Command(1, 1, 0.1m));
            result.IsValid.Should().BeTrue();
        }
    }
}

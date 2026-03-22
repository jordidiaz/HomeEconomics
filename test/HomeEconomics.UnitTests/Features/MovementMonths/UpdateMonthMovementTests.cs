using Domain.Movements;
using FluentAssertions;
using FluentValidation.TestHelper;
using HomeEconomics.Features.MovementMonths;
using JetBrains.Annotations;
using Xunit;

namespace HomeEconomics.UnitTests.Features.MovementMonths;

[UsedImplicitly]
public class UpdateMonthMovementTests
{
    private const string Name = nameof(Name);
    private const decimal Amount = 10;
    private const MovementType Type = MovementType.Expense;

    public class CommandValidatorTests
    {
        private readonly UpdateMonthMovement.Validator _sut = new();

        [Theory]
        [InlineData("")]
        [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        public void Should_Have_Error_If_Name_Invalid(string name)
        {
            var result = _sut.TestValidate(new UpdateMonthMovement.Command(1, 1, name, Amount, Type));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_Amount_Invalid()
        {
            var result = _sut.TestValidate(new UpdateMonthMovement.Command(1, 1, Name, -1, Type));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_Type_Invalid()
        {
            var result = _sut.TestValidate(new UpdateMonthMovement.Command(1, 1, Name, Amount, (MovementType)5));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Not_Have_Error_If_All_Valid()
        {
            var result = _sut.TestValidate(new UpdateMonthMovement.Command(1, 1, Name, Amount, MovementType.Income));
            result.IsValid.Should().BeTrue();
        }
    }
}

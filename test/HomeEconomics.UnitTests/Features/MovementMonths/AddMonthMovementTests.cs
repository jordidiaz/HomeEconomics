using Domain.Movements;
using FluentAssertions;
using FluentValidation.TestHelper;
using HomeEconomics.Features.Movements;
using JetBrains.Annotations;
using Xunit;

namespace HomeEconomics.UnitTests.Features.MovementMonths;

[UsedImplicitly]
public class AddMonthMovementTests
{
    private const string Name = nameof(Name);
    private const decimal Amount = 10;
    private const MovementType Type = MovementType.Expense;
    
        
    public class CommandValidatorTests
    {
        private readonly Create.Frequency _frequency = new(FrequencyType.Monthly, 0, []);
        private readonly Create.Validator _sut = new();

        [Theory]
        [InlineData("")]
        [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        public void Should_Have_Error_If_Name_Invalid(string name)
        {
            var result = _sut.TestValidate(new Create.Command(name, Amount, Type, _frequency));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_Amount_Invalid()
        {
            var result = _sut.TestValidate(new Create.Command(Name, -1, Type, _frequency));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_Type_Invalid()
        {
            var result = _sut.TestValidate(new Create.Command(Name, Amount, (MovementType)5, _frequency));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Not_Have_Error_If_All_Valid()
        {
            var result = _sut.TestValidate(new Create.Command(Name, Amount, MovementType.Expense, _frequency));
            result.IsValid.Should().BeTrue();
        }
    }
}

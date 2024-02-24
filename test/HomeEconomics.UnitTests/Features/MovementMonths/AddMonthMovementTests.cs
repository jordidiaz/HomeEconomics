using Domain.Movements;
using FluentAssertions;
using FluentValidation.TestHelper;
using HomeEconomics.Features.Movements;
using Xunit;

namespace HomeEconomics.UnitTests.Features.MovementMonths
{
    public class AddMonthMovementTests
    {
        private const string Name = nameof(Name);
        private const decimal Amount = 10;
        private const MovementType Type = MovementType.Expense;
        
        public class CommandValidatorTests
        {
            private readonly Create.Validator _sut;

            public CommandValidatorTests()
            {
                _sut = new Create.Validator();
            }

            [Theory]
            [InlineData("")]
            [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
            public void Should_Have_Error_If_Name_Invalid(string name)
            {
                var result = _sut.TestValidate(new Create.Command(name, Amount, Type, new Create.Frequency()));
                result.IsValid.Should().BeFalse();
            }

            [Fact]
            public void Should_Have_Error_If_Amount_Invalid()
            {
                var result = _sut.TestValidate(new Create.Command(Name, -1, Type, new Create.Frequency()));
                result.IsValid.Should().BeFalse();
            }

            [Fact]
            public void Should_Have_Error_If_Type_Invalid()
            {
                var result = _sut.TestValidate(new Create.Command(Name, Amount, (MovementType)5, new Create.Frequency()));
                result.IsValid.Should().BeFalse();
            }

            [Fact]
            public void Should_Not_Have_Error_If_All_Valid()
            {
                var result = _sut.TestValidate(new Create.Command(Name, Amount, MovementType.Expense, new Create.Frequency()));
                result.IsValid.Should().BeTrue();
            }
        }
    }
}

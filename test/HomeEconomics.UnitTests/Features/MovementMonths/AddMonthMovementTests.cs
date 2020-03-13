using Domain.Movements;
using FluentValidation.TestHelper;
using HomeEconomics.Features.Movements;
using Xunit;

namespace HomeEconomics.UnitTests.Features.MovementMonths
{
    public class AddMonthMovementTests
    {
        public class CommandValidatorTests
        {
            private readonly Create.Validator _sut;

            public CommandValidatorTests()
            {
                _sut = new Create.Validator();
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
            public void Should_Have_Error_If_Name_Invalid(string name)
            {
                _sut.ShouldHaveValidationErrorFor(x => x.Name, name);
            }

            [Fact]
            public void Should_Not_Have_Error_If_Name_Valid()
            {
                _sut.ShouldNotHaveValidationErrorFor(x => x.Name, "Valid");
            }

            [Fact]
            public void Should_Have_Error_If_Amount_Invalid()
            {
                _sut.ShouldHaveValidationErrorFor(x => x.Amount, -0.1m);
            }

            [Fact]
            public void Should_Not_Have_Error_If_Amount_Valid()
            {
                _sut.ShouldNotHaveValidationErrorFor(x => x.Amount, 10);
            }

            [Fact]
            public void Should_Have_Error_If_Type_Invalid()
            {
                _sut.ShouldHaveValidationErrorFor(x => x.Type, (MovementType)5);
            }

            [Fact]
            public void Should_Not_Have_Error_If_Type_Valid()
            {
                _sut.ShouldNotHaveValidationErrorFor(x => x.Type, MovementType.Expense);
            }
        }
    }
}

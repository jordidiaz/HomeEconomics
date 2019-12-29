using Domain.Movements;
using FluentValidation.TestHelper;
using HomeEconomics.Features.Movements;
using Xunit;

namespace HomeEconomics.UnitTests.Features.Movements
{
    public class CreateTests
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
            [InlineData("XXXXXXXXXXXXXXXXXXXXXXXX")]
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
                _sut.ShouldHaveValidationErrorFor(x => x.Amount, 0);
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

        public class FrequencyValidatorTests
        {
            private readonly Create.FrequencyValidator _sut;

            public FrequencyValidatorTests()
            {
                _sut = new Create.FrequencyValidator();
            }

            [Fact]
            public void Should_Have_Error_If_Type_Invalid()
            {
                _sut.ShouldHaveValidationErrorFor(x => x.Type, (FrequencyType)5);
            }

            [Fact]
            public void Should_Not_Have_Error_If_Type_Valid()
            {
                _sut.ShouldNotHaveValidationErrorFor(x => x.Type, FrequencyType.Yearly);
            }

            [Theory]
            [InlineData(0)]
            [InlineData(13)]
            public void Should_Have_Error_If_Month_Invalid(int month)
            {
                var frequency = new Create.Frequency
                {
                    Type = FrequencyType.Yearly,
                    Month = month
                };
                _sut.ShouldHaveValidationErrorFor(x => x.Month, frequency);
            }

            [Fact]
            public void Should_Not_Have_Error_If_Month_Valid()
            {
                _sut.ShouldNotHaveValidationErrorFor(x => x.Month, 6);
            }

            [Fact]
            public void Should_Have_Error_If_Months_Invalid()
            {
                var frequency1 = new Create.Frequency
                {
                    Type = FrequencyType.Custom,
                    Months = new[] { true, false }
                };
                _sut.ShouldHaveValidationErrorFor(x => x.Months, frequency1);

                var frequency2 = new Create.Frequency
                {
                    Type = FrequencyType.Custom,
                    Months = new[] { true, false, false, false, false, false, false, false, false, false, false, false }
                };
                _sut.ShouldHaveValidationErrorFor(x => x.Months, frequency2);
            }

            [Fact]
            public void Should_Not_Have_Error_If_Months_Valid()
            {
                _sut.ShouldNotHaveValidationErrorFor(x => x.Months, new[] { true, false, false, false, false, true, false, false, false, false, false, false });
            }
        }
    }
}

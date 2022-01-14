using Domain.MovementMonth;
using FluentAssertions;
using FluentValidation.TestHelper;
using HomeEconomics.Features.MovementMonths;
using Xunit;

namespace HomeEconomics.UnitTests.Features.MovementMonths
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
            [InlineData(2019)]
            [InlineData(1000)]
            public void Should_Have_Error_If_Year_Invalid(int year)
            {
                var result = _sut.TestValidate(new Create.Command(year, Month.Apr));
                result.IsValid.Should().BeFalse();
            }

            [Fact]
            public void Should_Have_Error_If_Month_Invalid()
            {
                var result = _sut.TestValidate(new Create.Command(2022, (Month)13));
                result.IsValid.Should().BeFalse();
            }

            [Fact]
            public void Should_Not_Have_Error_If_All_Valid()
            {
                var result = _sut.TestValidate(new Create.Command(2022, Month.Apr));
                result.IsValid.Should().BeTrue();
            }
        }
    }
}

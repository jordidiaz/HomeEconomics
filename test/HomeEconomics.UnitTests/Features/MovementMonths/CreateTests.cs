using System;
using Domain.MovementMonth;
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
                _sut.ShouldHaveValidationErrorFor(x => x.Year, year);
            }

            [Fact]
            public void Should_Not_Have_Error_If_Year_Valid()
            {
                _sut.ShouldNotHaveValidationErrorFor(x => x.Year, DateTime.Now.Year);
                _sut.ShouldNotHaveValidationErrorFor(x => x.Year, DateTime.Now.Year + 1);
            }

            [Fact]
            public void Should_Have_Error_If_Month_Invalid()
            {
                _sut.ShouldHaveValidationErrorFor(x => x.Month, (Month)13);
            }

            [Fact]
            public void Should_Not_Have_Error_If_Month_Valid()
            {
                _sut.ShouldNotHaveValidationErrorFor(x => x.Month, Month.Aug);
            }
        }
    }
}

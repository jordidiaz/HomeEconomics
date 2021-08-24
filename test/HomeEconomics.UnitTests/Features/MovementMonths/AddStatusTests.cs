using System;
using Domain.MovementMonth;
using FluentValidation.TestHelper;
using HomeEconomics.Features.MovementMonths;
using Xunit;

namespace HomeEconomics.UnitTests.Features.MovementMonths
{
    public class AddStatusTests
    {
        private readonly AddStatus.Validator _sut;

        public AddStatusTests()
        {
            _sut = new AddStatus.Validator();
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

        [Fact]
        public void Should_Have_Error_If_AccountAmount_Invalid()
        {
            _sut.ShouldHaveValidationErrorFor(x => x.AccountAmount, -0.1m);
        }

        [Fact]
        public void Should_Not_Have_Error_If_AccountAmount_Valid()
        {
            _sut.ShouldNotHaveValidationErrorFor(x => x.AccountAmount, 10);
        }

        [Fact]
        public void Should_Have_Error_If_CashAmount_Invalid()
        {
            _sut.ShouldHaveValidationErrorFor(x => x.CashAmount, -0.1m);
        }

        [Fact]
        public void Should_Not_Have_Error_If_CashAmount_Valid()
        {
            _sut.ShouldNotHaveValidationErrorFor(x => x.CashAmount, 10);
        }
    }
}

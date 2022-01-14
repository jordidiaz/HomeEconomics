using Domain.MovementMonth;
using FluentAssertions;
using FluentValidation.TestHelper;
using HomeEconomics.Features.MovementMonths;
using Xunit;

namespace HomeEconomics.UnitTests.Features.MovementMonths
{
    public class AddStatusTests
    {
        private const int Year = 2022;
        private const Month Month = Domain.MovementMonth.Month.Aug;
        private const decimal AccountAmount = 50m;
        private const decimal CashAmount = 10m;
        
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
            var result = _sut.TestValidate(new AddStatus.Command(year, Month, AccountAmount, CashAmount));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_Month_Invalid()
        {
            var result = _sut.TestValidate(new AddStatus.Command(Year, (Month)13, AccountAmount, CashAmount));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_AccountAmount_Invalid()
        {
            var result = _sut.TestValidate(new AddStatus.Command(Year, Month, -1, CashAmount));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_CashAmount_Invalid()
        {
            var result = _sut.TestValidate(new AddStatus.Command(Year, Month, AccountAmount, -1));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Not_Have_Error_If_CashAmount_Valid()
        {
            var result = _sut.TestValidate(new AddStatus.Command(Year, Month, AccountAmount, CashAmount));
            result.IsValid.Should().BeTrue();
        }
    }
}

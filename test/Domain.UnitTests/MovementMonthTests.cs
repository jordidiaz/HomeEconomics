using System;
using System.Linq;
using Domain.MovementMonth;
using Domain.Movements;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests
{
    public class MovementMonthTests
    {
        private const int Year = 2020;
        private const Month Month = MovementMonth.Month.Jan;
        private const string Name = nameof(Name);
        private const decimal Amount = 50m;
        private const MovementType MovementType = Movements.MovementType.Expense;

        private readonly MovementMonth.MovementMonth _sut;

        public MovementMonthTests()
        {
            _sut = new MovementMonth.MovementMonth(Year, Month);
        }

        [Fact]
        public void New_MovementMonth_Throws_ArgumentOutOfRangeException_If_Year_Invalid()
        {
            Action action = () => new MovementMonth.MovementMonth(2019, Month.Aug);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void New_MovementMonth_Creates_A_New_MovementMonth()
        {
            _sut.Year.Should().Be(Year);
            _sut.Month.Should().Be(Month);
            _sut.MonthMovements.Should().NotBeNull();
            _sut.MonthMovements.Count.Should().Be(0);
        }

        [Fact]
        public void AddMonthMovement_Adds_A_New_AddMonthMovement()
        {
            _sut.AddMonthMovement(Name, Amount, MovementType);

            _sut.MonthMovements.Count.Should().Be(1);
            _sut.MonthMovements.First().Name.Should().Be(Name);
            _sut.MonthMovements.First().Amount.Should().Be(Amount);
            _sut.MonthMovements.First().Type.Should().Be(MovementType);

        }
    }
}

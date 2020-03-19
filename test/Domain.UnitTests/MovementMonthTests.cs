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
        private const int Day = 1;
        private const decimal AccountAmount = 1500;
        private const decimal CashAmount = 100;

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
        public void AddMonthMovement_Adds_A_New_AddMonthMovement()
        {
            _sut.AddMonthMovement(Name, Amount, MovementType);

            _sut.MonthMovements.Count.Should().Be(1);
            _sut.MonthMovements.First().Name.Should().Be(Name);
            _sut.MonthMovements.First().Amount.Should().Be(Amount);
            _sut.MonthMovements.First().Type.Should().Be(MovementType);
            _sut.MonthMovements.First().Paid.Should().BeFalse();

        }

        [Fact]
        public void PayMonthMovement_Sets_MonthMovement_Paid()
        {
            _sut.AddMonthMovement(Name, Amount, MovementType);

            _sut.MonthMovements.First().Paid.Should().BeFalse();

            _sut.PayMonthMovement(0);

            _sut.MonthMovements.First().Paid.Should().BeTrue();
        }

        [Fact]
        public void PayMonthMovement_Throws_InvalidOperationException_If_MonthMovement_Not_Exists()
        {
            Action action = () => _sut.PayMonthMovement(0);

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MonthMovementNotExists);
        }

        [Fact]
        public void UnPayMonthMovement_Sets_MonthMovement_UnPaid()
        {
            _sut.AddMonthMovement(Name, Amount, MovementType);

            _sut.PayMonthMovement(0);

            _sut.MonthMovements.First().Paid.Should().BeTrue();

            _sut.UnPayMonthMovement(0);

            _sut.MonthMovements.First().Paid.Should().BeFalse();
        }

        [Fact]
        public void UnPayMonthMovement_Throws_InvalidOperationException_If_MonthMovement_Not_Exists()
        {
            Action action = () => _sut.UnPayMonthMovement(0);

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MonthMovementNotExists);
        }

        [Fact]
        public void UpdateMonthMovementAmount_Updates_MonthMovement_Amount()
        {
            _sut.AddMonthMovement(Name, Amount, MovementType);

            _sut.MonthMovements.First().Amount.Should().Be(50m);

            _sut.UpdateMonthMovementAmount(0, 60);

            _sut.MonthMovements.First().Amount.Should().Be(60m);
        }

        [Fact]
        public void UpdateMonthMovementAmount_Throws_InvalidOperationException_If_MonthMovement_Not_Exists()
        {
            Action action = () => _sut.UpdateMonthMovementAmount(0, 40m);

            action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MonthMovementNotExists);
        }

        [Fact]
        public void AddStatus_Adds_Status()
        {
            _sut.Statuses.Count.Should().Be(0);

            _sut.AddStatus(Day, AccountAmount, CashAmount);

            _sut.Statuses.Count.Should().Be(1);

        }

        [Fact]
        public void AddStatus_Updates_Status()
        {
            _sut.AddStatus(Day, AccountAmount, CashAmount);

            _sut.AddStatus(Day, AccountAmount, 33);

            _sut.Statuses.Count.Should().Be(1);
            _sut.Statuses.Single(s => s.Day == Day).AccountAmount.Should().Be(AccountAmount);
            _sut.Statuses.Single(s => s.Day == Day).CashAmount.Should().Be(33);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(32)]
        public void AddStatus_Throws_ArgumentOutOfRangeException_If_Day_Not_Valid(int day)
        {
            Action action = () => _sut.AddStatus(day, -1, CashAmount);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void AddStatus_Throws_ArgumentOutOfRangeException_If_AccountAmount_Not_Valid()
        {
            Action action = () => _sut.AddStatus(Day, -1, CashAmount);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void AddStatus_Throws_ArgumentOutOfRangeException_If_CashAmount_Not_Valid()
        {
            Action action = () => _sut.AddStatus(Day, AccountAmount, -1);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}

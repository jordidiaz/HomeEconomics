using Domain.MovementMonth;
using Domain.Movements;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.MovementMonth;

public class MovementMonthTests
{
    private readonly int _year = DateTime.Now.Year;
    private const Month Month = Domain.MovementMonth.Month.Jan;
    private const string Name1 = nameof(Name1);
    private const string Name2 = nameof(Name2);
    private const decimal Amount1 = 50m;
    private const decimal Amount2 = 60m;
    private const MovementType MovementType1 = MovementType.Expense;
    private const MovementType MovementType2 = MovementType.Income;
    private const int Day = 1;
    private const decimal AccountAmount = 1500;
    private const decimal CashAmount = 100;

    private readonly Domain.MovementMonth.MovementMonth _sut;

    public MovementMonthTests()
    {
        _sut = new Domain.MovementMonth.MovementMonth(_year, Month);

        _sut.AddMonthMovement(Name1, Amount1, MovementType1);
        _sut.AddMonthMovement(Name2, Amount2, MovementType2);

        _sut.GetMonthMovements().First().SetIdentity(1);
        _sut.GetMonthMovements().Last().SetIdentity(2);
    }

    [Fact]
    public void New_MovementMonth_Throws_ArgumentOutOfRangeException_If_Year_Invalid()
    {
        Action action = () => new Domain.MovementMonth.MovementMonth(2019, Month.Aug);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void AddMonthMovement_Throws_ArgumentNullException_If_Name_Invalid(string name)
    {
        Action action = () => _sut.AddMonthMovement(name, Amount1, MovementType1);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddMonthMovement_Throws_ArgumentOutOfRangeException_If_Amount_Invalid()
    {
        Action action = () => _sut.AddMonthMovement(Name1, -1, MovementType1);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void AddMonthMovement_Adds_A_New_AddMonthMovement()
    {
        _sut.ClearMovementMonths();
        _sut.AddMonthMovement(Name1, Amount1, MovementType1);

        _sut.GetMonthMovements().Count().Should().Be(1);
        _sut.GetMonthMovements().First().Name.Should().Be(Name1);
        _sut.GetMonthMovements().First().Amount.Should().Be(Amount1);
        _sut.GetMonthMovements().First().Type.Should().Be(MovementType1);
        _sut.GetMonthMovements().First().Paid.Should().BeFalse();

    }

    [Fact]
    public void PayMonthMovement_Sets_MonthMovement_Paid()
    {
        _sut.GetMonthMovements().First().Paid.Should().BeFalse();

        _sut.PayMonthMovement(1);

        _sut.GetMonthMovements().First().Paid.Should().BeTrue();
    }

    [Fact]
    public void PayMonthMovement_Throws_InvalidOperationException_If_MonthMovement_Not_Exists()
    {
        Action action = () => _sut.PayMonthMovement(3);

        action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MonthMovementNotExists);
    }

    [Fact]
    public void UnPayMonthMovement_Sets_MonthMovement_UnPaid()
    {
        _sut.PayMonthMovement(1);

        _sut.GetMonthMovements().First().Paid.Should().BeTrue();

        _sut.UnPayMonthMovement(1);

        _sut.GetMonthMovements().First().Paid.Should().BeFalse();
    }

    [Fact]
    public void UnPayMonthMovement_Throws_InvalidOperationException_If_MonthMovement_Not_Exists()
    {
        Action action = () => _sut.UnPayMonthMovement(3);

        action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MonthMovementNotExists);
    }

    [Fact]
    public void UpdateMonthMovementAmount_Throws_ArgumentOutOfRangeException_If_Amount_Invalid()
    {
        Action action = () => _sut.UpdateMonthMovementAmount(1, -1);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void UpdateMonthMovementAmount_Updates_MonthMovement_Amount()
    {
        _sut.GetMonthMovements().First().Amount.Should().Be(50m);

        _sut.UpdateMonthMovementAmount(1, 60);

        _sut.GetMonthMovements().First().Amount.Should().Be(60m);
    }

    [Fact]
    public void UpdateMonthMovementAmount_Throws_InvalidOperationException_If_MonthMovement_Not_Exists()
    {
        Action action = () => _sut.UpdateMonthMovementAmount(3, 40m);

        action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MonthMovementNotExists);
    }

    [Fact]
    public void DeleteMonthMovementAmount_Deletes_MonthMovement()
    {
        _sut.GetMonthMovements().SingleOrDefault(monthMovement => monthMovement.Id == 1).Should().NotBeNull();

        _sut.DeleteMonthMovement(1);

        _sut.GetMonthMovements().SingleOrDefault(monthMovement => monthMovement.Id == 1).Should().BeNull();
    }

    [Fact]
    public void DeleteMonthMovementAmount_Throws_InvalidOperationException_If_MonthMovement_Not_Exists()
    {
        Action action = () => _sut.DeleteMonthMovement(3);

        action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.MonthMovementNotExists);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(32)]
    public void AddStatus_Throws_ArgumentOutOfRangeException_If_Day_Invalid(int day)
    {
        Action action = () => _sut.AddStatus(day, AccountAmount, CashAmount);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void AddStatus_Throws_ArgumentOutOfRangeException_If_AccountAmount_Invalid()
    {
        Action action = () => _sut.AddStatus(Day, -1, CashAmount);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void AddStatus_Throws_ArgumentOutOfRangeException_If_CashAmount_Invalid()
    {
        Action action = () => _sut.AddStatus(Day, AccountAmount, -1);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void AddStatus_Adds_Status()
    {
        _sut.GetStatuses().Count().Should().Be(0);

        _sut.AddStatus(Day, AccountAmount, CashAmount);

        _sut.GetStatuses().Count().Should().Be(1);

    }

    [Fact]
    public void AddStatus_Updates_Status()
    {
        _sut.AddStatus(Day, AccountAmount, CashAmount);

        _sut.AddStatus(Day, AccountAmount, 33);

        _sut.GetStatuses().Count().Should().Be(1);
        _sut.GetStatuses().Single(s => s.Day == Day).AccountAmount.Should().Be(AccountAmount);
        _sut.GetStatuses().Single(s => s.Day == Day).CashAmount.Should().Be(33);
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
    public void GetMonthMovement_Returns_MonthMovement()
    {
        var monthMovement = _sut.GetMonthMovement(1);

        monthMovement?.Name.Should().Be(Name1);
        monthMovement?.Amount.Should().Be(Amount1);
        monthMovement?.Type.Should().Be(MovementType1);
    }

    [Fact]
    public void GetMonthMovement_Returns_Null()
    {
        var monthMovement = _sut.GetMonthMovement(6);

        monthMovement.Should().BeNull();
    }
}
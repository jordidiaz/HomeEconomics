using Domain.MovementMonth;
using Domain.Movements;
using FluentAssertions;
using Xunit;
// ReSharper disable ObjectCreationAsStatement

namespace Domain.UnitTests.Movement;

public class MovementTests
{
    private const string Name = nameof(Name);
    private const decimal Amount = 10m;

    private readonly Movements.Movement _sut = new(Name, Amount, MovementType.Income);

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void New_Movement_Throws_ArgumentNullException_If_Name_Invalid(string name)
    {
        Action action = () => new Movements.Movement(name, Amount, MovementType.Expense);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void New_Movement_Throws_ArgumentOutOfRangeException_If_Amount_Invalid()
    {
        Action action = () => new Movements.Movement(Name, -0.1m, MovementType.Income);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void SetNoneFrequency_Should_Set_Frequency_To_None()
    {
        _sut.SetMonthlyFrequency();

        _sut.Frequency.Type.Should().Be(FrequencyType.Monthly);

        _sut.SetNoneFrequency();

        _sut.Frequency.Type.Should().Be(FrequencyType.None);
        _sut.Frequency.Months.SequenceEqual(new bool[12]).Should().BeTrue();
    }

    [Fact]
    public void SetMonthlyFrequency_Should_Set_Frequency_To_Monthly()
    {
        _sut.Frequency.Type.Should().Be(FrequencyType.None);

        _sut.SetMonthlyFrequency();

        _sut.Frequency.Type.Should().Be(FrequencyType.Monthly);
        _sut.Frequency.Months.SequenceEqual(new bool[12]).Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void SetYearlyFrequency_Throws_ArgumentOutOfRangeException_If_Month_Invalid(int month)
    {
        var action = () => _sut.SetYearlyFrequency(month);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void SetYearlyFrequency_Should_Set_Frequency_To_Custom_And_Set_The_Month()
    {
        _sut.Frequency.Type.Should().Be(FrequencyType.None);

        _sut.SetYearlyFrequency(5);

        _sut.Frequency.Type.Should().Be(FrequencyType.Yearly);
        _sut.Frequency.Months.Count(m => !m).Should().Be(11);
        _sut.Frequency.Months[4].Should().BeTrue();
    }

    [Fact]
    public void SetYearlyFrequency_Should_Keep_Only_The_Last_Selected_Month()
    {
        _sut.SetYearlyFrequency(3);

        _sut.SetYearlyFrequency(7);

        _sut.Frequency.Type.Should().Be(FrequencyType.Yearly);
        _sut.Frequency.Months.Count(m => m).Should().Be(1);
        _sut.Frequency.Months[6].Should().BeTrue();
    }

    [Fact]
    public void SetCustomFrequency_Throws_ArgumentOutOfRangeException_If_Months_Count_Not_12()
    {
        var months = new[] { true, false, true };

        var action = () => _sut.SetCustomFrequency(months);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void SetCustomFrequency_Throws_InvalidOperationException_If_All_Months_False()
    {
        var months = new[] { false, false, false, false, false, false, false, false, false, false, false, false };

        var action = () => _sut.SetCustomFrequency(months);

        action.Should().Throw<InvalidOperationException>().WithMessage(Properties.Messages.NoMonthSelected);
    }

    [Fact]
    public void SetCustomFrequency_Should_Set_Frequency_To_Custom_And_Set_The_Months()
    {
        _sut.Frequency.Type.Should().Be(FrequencyType.None);

        var months = new[] { false, true, false, false, false, true, false, true, false, false, false, false };

        _sut.SetCustomFrequency(months);

        _sut.Frequency.Type.Should().Be(FrequencyType.Custom);
        _sut.Frequency.Months.SequenceEqual(months).Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void SetName_Throws_ArgumentNullException_If_Name_Is_Invalid(string name)
    {
        var action = () => _sut.SetName(name);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void SetName_Should_Set_The_Name()
    {
        _sut.Name.Should().Be(Name);

        _sut.SetName("thenewname");

        _sut.Name.Should().Be("thenewname");
    }

    [Fact]
    public void SetAmount_Throws_ArgumentOutOfRangeException_If_Amount_Is_Invalid()
    {
        var action = () => _sut.SetAmount(-0.1m);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void SetAmount_Should_Set_The_Amount()
    {
        _sut.Amount.Should().Be(Amount);

        _sut.SetAmount(56m);

        _sut.Amount.Should().Be(56m);
    }

    [Fact]
    public void GetFrequencyType_Should_Return_FrequencyType_None() => _sut.GetFrequencyType().Should().Be(FrequencyType.None);

    [Fact]
    public void GetFrequencyType_Should_Return_FrequencyType_Monthly()
    {
        _sut.SetMonthlyFrequency();
        _sut.GetFrequencyType().Should().Be(FrequencyType.Monthly);
    }

    [Fact]
    public void GetFrequencyType_Should_Return_FrequencyType_Yearly()
    {
        _sut.SetYearlyFrequency(2);
        _sut.GetFrequencyType().Should().Be(FrequencyType.Yearly);
    }

    [Fact]
    public void GetFrequencyType_Should_Return_FrequencyType_Custom()
    {
        _sut.SetCustomFrequency([
            true,
            false,
            true,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false
        ]);
        _sut.GetFrequencyType().Should().Be(FrequencyType.Custom);
    }

    [Fact]
    public void HasMonthInFrequency_Should_Return_False_If_FrequencyType_None() => _sut.HasMonthInFrequency(Month.Aug).Should().Be(false);

    [Fact]
    public void HasMonthInFrequency_Should_Return_False_If_FrequencyType_Monthly() => _sut.HasMonthInFrequency(Month.Aug).Should().Be(false);

    [Fact]
    public void HasMonthInFrequency_Should_Return_False_If_FrequencyType_Yearly()
    {
        _sut.SetYearlyFrequency(1);
        _sut.HasMonthInFrequency(Month.Aug).Should().Be(false);
    }

    [Fact]
    public void HasMonthInFrequency_Should_Return_False_If_FrequencyType_Custom()
    {
        _sut.SetCustomFrequency([
            true,
            false,
            true,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false
        ]);
        _sut.HasMonthInFrequency(Month.Aug).Should().Be(false);
    }

    [Fact]
    public void HasMonthInFrequency_Should_Return_True_If_FrequencyType_Yearly()
    {
        _sut.SetYearlyFrequency(8);
        _sut.HasMonthInFrequency(Month.Aug).Should().Be(true);
    }

    [Fact]
    public void HasMonthInFrequency_Should_Return_True_If_FrequencyType_Custom()
    {
        _sut.SetCustomFrequency([
            true,
            false,
            true,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false
        ]);
        _sut.HasMonthInFrequency(Month.Jan).Should().Be(true);
    }
}

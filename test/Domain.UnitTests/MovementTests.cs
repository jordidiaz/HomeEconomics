using System;
using System.Linq;
using Domain.Movements;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests
{
    public class MovementTests
    {
        private const string Name = nameof(Name);
        private const decimal Amount = 10m;

        private readonly Movement _sut;

        public MovementTests()
        {
            _sut = new Movement(Name, Amount, MovementType.Income);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void New_Movement_Throws_ArgumentNullException_If_Name_Invalid(string name)
        {
            Action action = () => new Movement(name, Amount, MovementType.Expense);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void New_Movement_Throws_ArgumentOutOfRangeException_If_Amount_Invalid()
        {
            Action action = () => new Movement(Name, 0.0m, MovementType.Income);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void New_Movement_Creates_A_New_Movement()
        {
            var movement = new Movement(Name, Amount, MovementType.Income);

            movement.Name.Should().Be(Name);
            movement.Amount.Should().Be(Amount);
            movement.Type.Should().Be(MovementType.Income);

            movement.Frequency.Type.Should().Be(FrequencyType.None);
            movement.Frequency.Months.Should().BeNull();
        }

        [Fact]
        public void SetMonthlyFrequency_Should_Set_Frequency_To_Monthly()
        {
            _sut.Frequency.Type.Should().Be(FrequencyType.None);

            _sut.SetMonthlyFrequency();

            _sut.Frequency.Type.Should().Be(FrequencyType.Monthly);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        public void SetYearlyFrequency_Throws_ArgumentOutOfRangeException_If_Month_Invalid(int month)
        {
            Action action = () => _sut.SetYearlyFrequency(month);

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
        public void SetCustomFrequency_Throws_ArgumentOutOfRangeException_If_Months_Count_Not_12()
        {
            var months = new[] {true, false, true};

            Action action = () => _sut.SetCustomFrequency(months);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void SetCustomFrequency_Throws_InvalidOperationException_If_All_Months_False()
        {
            var months = new[] { false, false, false, false, false, false, false, false, false, false, false, false };

            Action action = () => _sut.SetCustomFrequency(months);

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
    }
}

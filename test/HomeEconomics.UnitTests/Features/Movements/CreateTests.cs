using Domain.Movements;
using FluentAssertions;
using FluentValidation.TestHelper;
using HomeEconomics.Features.Movements;
using JetBrains.Annotations;
using Xunit;

namespace HomeEconomics.UnitTests.Features.Movements;

[UsedImplicitly]
public class CreateTests
{
    public class CommandValidatorTests
    {
        private readonly Create.Frequency _emptyFrequency = new(FrequencyType.Monthly, 0, []);
        private readonly Create.Validator _sut = new();

        private const string Name = nameof(Name);
        private const decimal Amount = 10;
        private const MovementType Type = MovementType.Expense;

        [Theory]
        [InlineData("")]
        [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        public void Should_Have_Error_If_Name_Invalid(string name)
        {
            var result = _sut.TestValidate(new Create.Command(name, Amount, Type, _emptyFrequency));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_Amount_Invalid()
        {
            var result = _sut.TestValidate(new Create.Command(Name, -1, Type, _emptyFrequency));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_Type_Invalid()
        {
            var result = _sut.TestValidate(new Create.Command(Name, Amount, (MovementType)4, _emptyFrequency));
            result.IsValid.Should().BeFalse();
        }
        
        [Fact]
        public void Should_Not_Have_Error_If_All_Valid()
        {
            var result = _sut.TestValidate(new Create.Command(Name, Amount, Type, _emptyFrequency));
            result.IsValid.Should().BeTrue();
        }
    }
        
    public class FrequencyValidatorTests
    {
        private readonly Create.FrequencyValidator _sut = new();

        [Fact]
        public void Should_Have_Error_If_Type_Invalid()
        {
            var result = _sut.TestValidate(new Create.Frequency((FrequencyType)5, 0, []));
            result.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        public void Should_Have_Error_If_Month_Invalid(int month)
        {
            var frequency = new Create.Frequency(FrequencyType.Yearly, month, []);
            var result = _sut.TestValidate(frequency);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_Months_Invalid()
        {
            var frequency1 = new Create.Frequency(FrequencyType.Custom, 0, [true, false]);
            var result = _sut.TestValidate(frequency1);
            result.IsValid.Should().BeFalse();

            var frequency2 = new Create.Frequency(FrequencyType.Custom, 0,
                [true, false, false, false, false, false, false, false, false, false, false, false]);
            result = _sut.TestValidate(frequency2);
            result.IsValid.Should().BeFalse();
        }
        
        [Fact]
        public void Should_Not_Have_Error_If_All_Valid()
        {
            var frequency = new Create.Frequency(FrequencyType.Custom, 0,
                [true, true, false, false, false, false, false, false, false, false, false, false]);
            var result = _sut.TestValidate(frequency);
            result.IsValid.Should().BeTrue();
        }
    }
}

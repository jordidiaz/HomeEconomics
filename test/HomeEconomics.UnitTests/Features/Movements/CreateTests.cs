using Domain.Movements;
using FluentAssertions;
using FluentValidation.TestHelper;
using HomeEconomics.Features.Movements;
using Xunit;

namespace HomeEconomics.UnitTests.Features.Movements;

public class CreateTests
{
    public class CommandValidatorTests
    {
        private readonly Create.Validator _sut;

        private const string Name = nameof(Name);
        private const decimal Amount = 10;
        private const MovementType Type = MovementType.Expense;

        public CommandValidatorTests() => _sut = new Create.Validator();

        [Theory]
        [InlineData("")]
        [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        public void Should_Have_Error_If_Name_Invalid(string name)
        {
            var result = _sut.TestValidate(new Create.Command(name, Amount, Type, new Create.Frequency()));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_Amount_Invalid()
        {
            var result = _sut.TestValidate(new Create.Command(Name, -1, Type, new Create.Frequency()));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_Type_Invalid()
        {
            var result = _sut.TestValidate(new Create.Command(Name, Amount, (MovementType)4, new Create.Frequency()));
            result.IsValid.Should().BeFalse();
        }
        
        [Fact]
        public void Should_Not_Have_Error_If_All_Valid()
        {
            var result = _sut.TestValidate(new Create.Command(Name, Amount, Type, new Create.Frequency()));
            result.IsValid.Should().BeTrue();
        }
    }
        
    public class FrequencyValidatorTests
    {
        private readonly Create.FrequencyValidator _sut;
        
        public FrequencyValidatorTests() => _sut = new Create.FrequencyValidator();

        [Fact]
        public void Should_Have_Error_If_Type_Invalid()
        {
            var result = _sut.TestValidate(new Create.Frequency
            {
                Type = (FrequencyType)5
            });
            result.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        public void Should_Have_Error_If_Month_Invalid(int month)
        {
            var frequency = new Create.Frequency
            {
                Type = FrequencyType.Yearly,
                Month = month
            };
            var result = _sut.TestValidate(frequency);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Error_If_Months_Invalid()
        {
            var frequency1 = new Create.Frequency
            {
                Type = FrequencyType.Custom,
                Months = new[] { true, false }
            };
            var result = _sut.TestValidate(frequency1);
            result.IsValid.Should().BeFalse();
        
            var frequency2 = new Create.Frequency
            {
                Type = FrequencyType.Custom,
                Months = new[] { true, false, false, false, false, false, false, false, false, false, false, false }
            }; 
            result = _sut.TestValidate(frequency2);
            result.IsValid.Should().BeFalse();
        }
        
        [Fact]
        public void Should_Not_Have_Error_If_All_Valid()
        {
            var frequency = new Create.Frequency
            {
                Type = FrequencyType.Custom,
                Months = new[] { true, true, false, false, false, false, false, false, false, false, false, false }
            }; 
            var result = _sut.TestValidate(frequency);
            result.IsValid.Should().BeTrue();
        }
    }
}
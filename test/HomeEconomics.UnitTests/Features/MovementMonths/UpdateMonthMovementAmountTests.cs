// using FluentValidation.TestHelper;
// using HomeEconomics.Features.MovementMonths;
// using Xunit;
//
// namespace HomeEconomics.UnitTests.Features.MovementMonths
// {
//     public class UpdateMonthMovementAmountTests
//     {
//         public class CommandValidatorTests
//         {
//             private readonly UpdateMonthMovementAmount.Validator _sut;
//
//             public CommandValidatorTests()
//             {
//                 _sut = new UpdateMonthMovementAmount.Validator();
//             }
//
//             [Fact]
//             public void Should_Have_Error_If_Amount_Invalid()
//             {
//                 _sut.ShouldHaveValidationErrorFor(x => x.Amount, -0.1m);
//             }
//
//             [Fact]
//             public void Should_Not_Have_Error_If_Amount_Valid()
//             {
//                 _sut.ShouldNotHaveValidationErrorFor(x => x.Amount, 10);
//             }
//         }
//     }
// }

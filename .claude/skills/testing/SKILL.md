---
name: testing
description: Unified full-stack testing skill — workflows, per-layer checklists, and decision rules for backend and frontend tests in this repository
---

## When to use this skill
- You are adding or changing behavior in domain entities, API features, or the frontend.
- You need to decide which test layer(s) to write for a given change.
- You are following the mandatory testing workflow in CLAUDE.md.

---

## Mandatory workflow

1. **Identify layers touched** — domain entity? feature handler/validator? API endpoint? service/hook/component?
2. **List specific tests** — for each layer, name the test class, method names, and key assertions before writing any code.
3. **Write tests** following the per-layer patterns below.
4. **Run relevant suites** and confirm all pass.

---

## Backend layers

### 1) Domain unit tests
**Project**: `test/Domain.UnitTests/`
**Path pattern**: `test/Domain.UnitTests/{Entity}/{Entity}Tests.cs`
**Framework**: xUnit + FluentAssertions

What to test:
- Entity construction: valid inputs create correct state; invalid inputs throw (`ArgumentNullException`, `ArgumentOutOfRangeException`, `InvalidOperationException`).
- State transitions: every public method that mutates entity state.
- Invariants: rules that must always hold (e.g. amount ≥ 0, name not blank).

Pattern:
```csharp
public class MovementTests
{
    private readonly Movement _sut = new("Name", 10m, MovementType.Income);

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void New_Movement_Throws_ArgumentNullException_If_Name_Invalid(string name)
    {
        Action action = () => new Movement(name, 10m, MovementType.Expense);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void SetAmount_Should_Set_The_Amount()
    {
        _sut.SetAmount(56m);
        _sut.Amount.Should().Be(56m);
    }
}
```

Naming: `{Method}_Should_{ExpectedOutcome}` or `{Method}_Throws_{Exception}_If_{Condition}`.

---

### 2) Feature unit tests (validator)
**Project**: `test/HomeEconomics.UnitTests/`
**Path pattern**: `test/HomeEconomics.UnitTests/Features/{Feature}/{Action}Tests.cs`
**Framework**: xUnit + FluentAssertions + FluentValidation.TestHelper

What to test:
- Each validator rule: one test per invalid input variant (use `[Theory]` for multiple bad values).
- A "happy path" test confirming `IsValid == true` with all valid inputs.
- Nested validators (e.g. `FrequencyValidator`) get their own nested class.

Pattern:
```csharp
[UsedImplicitly]
public class CreateTests
{
    public class CommandValidatorTests
    {
        private readonly Create.Validator _sut = new();

        [Theory]
        [InlineData("")]
        [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        public void Should_Have_Error_If_Name_Invalid(string name)
        {
            var result = _sut.TestValidate(new Create.Command(name, 10m, MovementType.Expense, new()));
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Not_Have_Error_If_All_Valid()
        {
            var result = _sut.TestValidate(new Create.Command("Name", 10m, MovementType.Expense, new()));
            result.IsValid.Should().BeTrue();
        }
    }
}
```

---

### 3) Integration tests
**Project**: `test/HomeEconomics.IntegrationTests/`
**Path pattern**: `test/HomeEconomics.IntegrationTests/Features/{Feature}/{Action}Tests.cs`
**Framework**: xUnit + FluentAssertions + Testcontainers PostgreSQL + Respawn

What to test:
- Happy path: correct HTTP status code + persisted state matches what was sent.
- Validation failure: 400 Bad Request for invalid input.
- Not found: 404 when resource doesn't exist.
- Business rule violations: appropriate 4xx when domain throws.

Pattern:
```csharp
public class CreateTests : IntegrationTestBase
{
    private readonly Fixture _fixture;

    public CreateTests(Fixture fixture) : base(fixture) => _fixture = fixture;

    [Fact]
    public async Task Should_Return_200_Ok_And_Create_A_New_Movement()
    {
        var command = new Create.Command("Gasolina", 60m, MovementType.Expense,
            new Create.Frequency { Type = FrequencyType.Monthly });

        var response = await HttpClient.PostAsync("api/movements", command);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var id = int.Parse(await response.Content.ReadAsStringAsync(), CultureInfo.InvariantCulture);

        var entity = await _fixture.QueryDbContextAsync(async db =>
            await db.Movements.SingleOrDefaultAsync(m => m.Id == id));

        entity!.Name.Should().Be("Gasolina");
    }

    [Fact]
    public async Task Should_Return_400_BadRequest_If_Command_Not_Valid()
    {
        var command = new Create.Command(string.Empty, 50m, MovementType.Expense, new());
        var response = await HttpClient.PostAsync("api/movements", command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
```

Key infrastructure:
- `IntegrationTestBase` resets DB via Respawn in `InitializeAsync()`.
- `Fixture` exposes `HttpClient`, `QueryDbContextAsync`, and `SendCommandToMediatorAsync`.
- Use `CreateMovementsAsync()` / `CreateMovementMonthAsync()` helpers from the base class to seed state.
- Decorate with `[Collection(Collections.IntegrationTestCollection)]` (inherited from base).

---

## Frontend layers

See `docs/frontend/testing-strategy.md` for the complete coverage matrix and test file index.
Summary of each layer:

### Service tests (Vitest)
**When**: any change to `services/*.ts`.
**What**: correct URL/method/body, non-2xx throws with status, in-flight caching.
**Pattern**: mock `fetch` globally; import the service function; assert calls and return values.

### Hook tests (Vitest)
**When**: any change to `hooks/*.ts`.
**What**: loading → data transition, error states, derived values, action side effects.
**Pattern**: `renderHook` from RTL; mock the service layer (not hooks). Test behavior, not internals.

### Component tests (RTL + Vitest)
**When**: any change to `components/*.tsx`.
**What**: render states (loading/error/data), user interactions, Spanish labels, disabled-while-submitting.
**Pattern**: render with mocked props/hooks; use `userEvent`; query by role or text.

### Integration tests (RTL + Vitest)
**When**: adding or changing `app/page.tsx` wiring.
**What**: hook + component wiring — actions trigger reloads, dialogs close on success, state updates propagate.
**Path**: `app/__tests__/page.test.tsx`.

### E2E tests (Playwright)
**When**: new critical user journey or end-to-end flow change.
**What**: full user journeys against local backend (create data via API in `beforeEach`).
**Path**: `e2e/` or per-feature test file.

---

## Decision rules

### 1) Choose the smallest layer that proves the behavior
- Domain entity logic → Domain unit tests only.
- Validator rules → Feature unit tests only.
- Handler + persistence round-trip → Integration tests.
- UI rendering + user interaction → Component or hook tests.
- Cross-layer user journey → E2E.

### 2) When to add new tests
- Always add tests for new behavior or changed outcomes.
- Skip only when the change is strictly mechanical and existing tests already assert the same outcome.

### 3) Regression prevention
- If a bug escaped detection, add a test that would have failed before the fix.
- Assert outcomes and externally visible behavior, not internal wiring.

### 4) Coverage vs. speed balance
- Favor fast tests for frequent changes; reserve slower integration/E2E tests for cross-layer risks.
- Avoid asserting the same thing at multiple layers unless it mitigates a known risk.

### 5) Quality checks
- Public behavior change → tests cover the new code path.
- Data integrity change → cover both valid and invalid inputs.
- User-facing change → include coverage verifying the UX outcome (Spanish copy, disabled state, error text).

---

## Guardrails
- No snapshot tests.
- Mock services, not hooks (frontend).
- Assert outcomes and intent, not internal wiring.
- One test = one behavioral expectation.
- Assert user-visible Spanish copy where applicable.

# Pragmatic Unit Testing — Patterns & Practices

A hands-on C# / .NET 9 reference project that demonstrates the key design patterns and practices behind **pragmatic unit testing**, built around a simple `BankManager` domain.

---

## Table of Contents

1. [What is Pragmatic Unit Testing?](#what-is-pragmatic-unit-testing)
2. [Why It Matters](#why-it-matters)
3. [Core Principles](#core-principles)
4. [Repository Structure](#repository-structure)
5. [Key Patterns Demonstrated](#key-patterns-demonstrated)
6. [Tech Stack](#tech-stack)
7. [Getting Started](#getting-started)
8. [How This Repo Differs from the Pluralsight Course](#how-this-repo-differs-from-the-pluralsight-course)
9. [Further Reading](#further-reading)

---

## What is Pragmatic Unit Testing?

**Pragmatic unit testing** is the discipline of writing automated tests that are:

- **Small** – each test covers one unit of behavior.
- **Fast** – tests run in milliseconds with no real I/O (no databases, no network calls).
- **Isolated** – tests do not depend on each other or on external state.
- **Readable** – a failing test immediately tells you *what* broke and *why*.
- **Trustworthy** – tests fail for the right reasons and only for the right reasons.

The word *pragmatic* sets it apart from dogmatic TDD or coverage-chasing. The goal is not 100 % coverage for its own sake; it is **confidence in your codebase at the lowest possible maintenance cost**. Tests that are hard to read, coupled to implementation details, or broken by unrelated refactors are worse than no tests at all.

> *"Good tests fail only when behavior changes — not when implementation changes."*

---

## Why It Matters

| Without pragmatic tests | With pragmatic tests |
|---|---|
| Regressions are caught in production | Regressions are caught in seconds locally |
| Refactoring is risky | Refactoring is safe |
| New developers fear touching old code | Tests act as living documentation |
| CI is slow and flaky | CI is fast and trustworthy |

---

## Core Principles

### 1. Test Behavior, Not Implementation
A test should verify *what* the system does, not *how* it does it internally. Avoid asserting on private state or internal call sequences unless they represent an observable contract.

### 2. Arrange – Act – Assert (AAA)
Every test is structured in three clearly separated phases:

```
Arrange  →  set up inputs and dependencies
Act      →  call the code under test
Assert   →  verify the expected outcome
```

### 3. DAMP over DRY in Tests
Production code should be DRY (Don't Repeat Yourself). Test code should be DAMP (Descriptive And Meaningful Phrases). A little duplication in tests is acceptable when it keeps each test self-contained and readable.

### 4. Use Test Doubles to Break Dependencies
Real databases, file systems, and third-party APIs have no place in a unit test. Replace them with:

| Double | Purpose |
|---|---|
| **Stub** | Returns canned data; no verification |
| **Mock** | Verifies that a call was (or was not) made |
| **Fake** | A lightweight, working stand-in (e.g., in-memory DB) |

### 5. One Logical Assertion per Test
Each test should have one reason to fail. Grouping unrelated assertions makes it harder to diagnose failures.

### 6. Descriptive Test Names
Follow the pattern `MethodName_Scenario_ExpectedResult`:
```
CheckBalance_WithNoTransactions_Returns0Balance
CalculateTotalTransaction_AmountAndFeeProvided_ReturnsAmountMinusFee
```

---

## Repository Structure

```
pragmatic-unit-testing/
│
├── BankManager/                    # Production code (domain)
│   ├── Transaction.cs              # Abstract base: BaseAmount, CalculateTotalTransaction()
│   ├── SimpleTransaction.cs        # Concrete: total == base amount
│   ├── FeeTransaction.cs           # Concrete: total == base amount − fee
│   ├── AccountRepository.cs        # Virtual methods — dependency to be mocked
│   ├── Teller.cs                   # Orchestrator: CheckBalance, ProcessTransaction
│   └── Logging.cs                  # Static logger (injectable via ILogger)
│
├── BankManager.Tests/              # MSTest test project
│   ├── GlobalBankManagerTestSetup.cs   # [AssemblyInitialize] — Global Fixture pattern
│   ├── BaseTestClass.cs                # [TestInitialize] base — Abstract Test Class pattern
│   ├── TransactionTests.cs             # Abstract template — Template Test Class pattern
│   ├── SimpleTransactionTests.cs       # Concrete: inherits TransactionTests
│   ├── FeeTransactionTests.cs          # Concrete: inherits TransactionTests
│   ├── TellerTests.cs                  # Moq mocking of AccountRepository
│   ├── AccountRepositoryTests.cs       # Direct repository tests
│   └── MSTestSettings.cs               # Test run configuration
│
├── notes/
│   ├── MSTestLifecycle.md              # Full MSTest execution lifecycle reference
│   └── TestFrameworkLifecycleComparison.md  # MSTest vs NUnit vs xUnit side-by-side
│
├── pragmatic_unit_testing.md       # Concept summary notes
└── BankManager.slnx                # .NET solution file
```

---

## Key Patterns Demonstrated

### Global Fixture
`GlobalBankManagerTestSetup.cs` uses `[AssemblyInitialize]` to set up a shared mock logger once for the entire test run, avoiding repeated setup and ensuring no real I/O leaks through.

```csharp
[AssemblyInitialize]
public static void AssemblyInit(TestContext context)
{
    Logging.Logger = Mock.Of<ILogger>();
}
```

### Abstract Test Class (Base Test Class)
`BaseTestClass.cs` centralises per-test setup (mock logger wiring) that every test class needs. Concrete test classes inherit from it and call `base.TestInit()` implicitly.

### Template Test Class
`TransactionTests.cs` is an abstract test class that defines a shared test (`BaseAmount_AmountPassedToConstructor_ReturnsSameAmount`) run for *every* concrete `Transaction` type. `SimpleTransactionTests` and `FeeTransactionTests` extend it and supply the concrete object via a factory method — no code duplication across transaction types.

```csharp
public abstract class TransactionTests
{
    public abstract Transaction GetTransactionWith(int baseAmount);

    [TestMethod]
    public void BaseAmount_AmountPassedToConstructor_ReturnsSameAmount() { ... }
}
```

### Mocking with Moq
`TellerTests.cs` shows how to isolate `Teller` from `AccountRepository` using `Mock.Of<T>()` and `.Setup()` / `.Verify()`:

```csharp
Mock.Get(_accountRepository)
    .Setup(ar => ar.CheckBalance())
    .Returns(0);
```

### MSTest Lifecycle (Assembly → Class → Test)
The `notes/MSTestLifecycle.md` and `notes/TestFrameworkLifecycleComparison.md` files document the full execution order and provide a side-by-side comparison with NUnit and xUnit.

---

## Tech Stack

| Component | Choice |
|---|---|
| Language | C# 13 |
| Runtime | .NET 9 |
| Test framework | MSTest v3 |
| Mocking library | Moq 4 |
| Solution format | `.slnx` (new SDK-style solution) |

---

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)

### Run all tests

```bash
dotnet test
```

### Run tests with detailed output

```bash
dotnet test --logger "console;verbosity=detailed"
```

---

## How This Repo Differs from the Pluralsight Course

The Pluralsight course **"Patterns for Pragmatic Unit Testing"** is a structured video curriculum taught by an instructor. This repository is a **self-study companion project** built alongside those ideas. Here is a clear breakdown of the differences:

| Dimension | Pluralsight Course | This Repository |
|---|---|---|
| **Format** | Video-based, instructor-led | Code-first, hands-on project |
| **Language** | Demonstrated in C# (may also include Java/JS examples depending on module) | C# only (.NET 9 / MSTest) |
| **Content** | Covers a wide range of patterns, anti-patterns, legacy code strategies, async testing, and more across many modules | Focuses on the *infrastructure* patterns: Global Fixture, Abstract Test Class, Template Test Class, and Moq-based isolation |
| **Depth** | Goes deeper into theory, rationale, and multiple language examples | Emphasises working, runnable code you can inspect and experiment with |
| **Golden Master / Legacy Code** | Covered in dedicated modules | Documented in `pragmatic_unit_testing.md` as notes, not yet implemented in code |
| **Async Testing** | Dedicated section with patterns like polling and avoiding `Thread.sleep` | Referenced in notes; not yet demonstrated in code |
| **Parameterized Tests** | Covered | Not yet implemented (could be added with `[DataRow]` in MSTest) |
| **Framework comparison** | Typically targets a single framework per course | Includes `notes/TestFrameworkLifecycleComparison.md` comparing MSTest, NUnit, and xUnit |
| **Pace** | Structured curriculum with a set order | Free to explore in any order; each class/file is independently understandable |
| **Access** | Requires a Pluralsight subscription | Fully open source |

### In summary

The Pluralsight course is the **theory and guided instruction**; this repository is the **practice and experimentation ground**. Use this project to:
- Reinforce what you watched in the course by reading and running the code.
- Experiment with patterns by adding new test classes or extending the `BankManager` domain.
- Use the `notes/` folder as a quick-reference cheat sheet when writing your own tests.

---

## Further Reading

- [Martin Fowler — Test Doubles](https://martinfowler.com/bliki/TestDouble.html)
- [Martin Fowler — Testing](https://martinfowler.com/testing/)
- [MSTest Documentation](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
- [Moq Documentation](https://github.com/devlooped/moq/wiki/Quickstart)
- [xUnit Documentation](https://xunit.net/docs/getting-started/netcore/cmdline)
- [Kent Beck — Test-Driven Development by Example](https://www.amazon.com/Test-Driven-Development-Kent-Beck/dp/0321146530)

# Test Framework Lifecycle Comparison — MSTest vs NUnit vs xUnit

---

## Quick Reference Table

| Scope | MSTest | NUnit | xUnit |
|---|---|---|---|
| **Assembly setup** | `[AssemblyInitialize]` (static) | `[SetUpFixture]` + `[OneTimeSetUp]` | `ICollectionFixture<T>` |
| **Assembly teardown** | `[AssemblyCleanup]` (static) | `[SetUpFixture]` + `[OneTimeTearDown]` | `ICollectionFixture<T>` (dispose) |
| **Class setup** | `[ClassInitialize]` (static) | `[OneTimeSetUp]` | Constructor |
| **Class teardown** | `[ClassCleanup]` (static) | `[OneTimeTearDown]` | `IDisposable.Dispose` |
| **Per-test setup** | `[TestInitialize]` | `[SetUp]` | Constructor |
| **Per-test teardown** | `[TestCleanup]` | `[TearDown]` | `IDisposable.Dispose` |
| **Test method** | `[TestMethod]` | `[Test]` | `[Fact]` / `[Theory]` |
| **Instance per test** | Yes | No (shared by default) | Yes |

---

## 1. Assembly Level

### MSTest
```csharp
[AssemblyInitialize]
public static void Init(TestContext context) { }

[AssemblyCleanup]
public static void Cleanup() { }
```

### NUnit
NUnit uses a special `[SetUpFixture]` class placed outside any namespace or at the root namespace. Methods inside it annotated with `[OneTimeSetUp]` / `[OneTimeTearDown]` run once for the entire assembly.

```csharp
[SetUpFixture]
public class AssemblySetup
{
    [OneTimeSetUp]
    public void Init() { }

    [OneTimeTearDown]
    public void Cleanup() { }
}
```

### xUnit
xUnit has no direct assembly-level hook in plain test classes. Use a **collection fixture** shared across all test classes.

```csharp
// 1. Define the fixture
public class DatabaseFixture : IDisposable
{
    public DatabaseFixture() { /* setup */ }
    public void Dispose() { /* teardown */ }
}

// 2. Declare a collection
[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

// 3. Use in test classes
[Collection("Database")]
public class MyTests
{
    public MyTests(DatabaseFixture fixture) { }
}
```

---

## 2. Class Level

### MSTest
```csharp
[ClassInitialize]
public static void ClassInit(TestContext context) { }

[ClassCleanup]
public static void ClassCleanup() { }
```

### NUnit
Inside a normal test class, `[OneTimeSetUp]` / `[OneTimeTearDown]` run once per class (not per test).

```csharp
[OneTimeSetUp]
public void ClassInit() { }

[OneTimeTearDown]
public void ClassCleanup() { }
```

> Note: NUnit reuses the **same class instance** for all tests in a class by default, so state set in `[OneTimeSetUp]` persists across tests.

### xUnit
xUnit uses the **constructor** and `IDisposable` for class-scoped setup/teardown. Because xUnit creates a new instance per test, this is actually per-test (see section 3). For true class-level (shared) setup, use `IClassFixture<T>`.

```csharp
public class MyFixture : IDisposable
{
    public MyFixture() { /* runs once */ }
    public void Dispose() { /* runs once */ }
}

public class MyTests : IClassFixture<MyFixture>
{
    public MyTests(MyFixture fixture) { }
}
```

---

## 3. Per-Test Level

### MSTest
```csharp
[TestInitialize]
public void Setup() { }

[TestMethod]
public void MyTest() { }

[TestCleanup]
public void TearDown() { }
```

### NUnit
```csharp
[SetUp]
public void Setup() { }

[Test]
public void MyTest() { }

[TearDown]
public void TearDown() { }
```

### xUnit
xUnit favors the **constructor / `IDisposable`** pattern instead of setup/teardown attributes.

```csharp
public class MyTests : IDisposable
{
    // Runs before each test (constructor = setup)
    public MyTests() { /* setup */ }

    [Fact]
    public void MyTest() { }

    // Runs after each test (Dispose = teardown)
    public void Dispose() { /* teardown */ }
}
```

For async setup/teardown, implement `IAsyncLifetime`:

```csharp
public class MyTests : IAsyncLifetime
{
    public async Task InitializeAsync() { /* async setup */ }

    [Fact]
    public void MyTest() { }

    public async Task DisposeAsync() { /* async teardown */ }
}
```

---

## Full Execution Order Comparison

### MSTest
```
AssemblyInitialize
  ClassInitialize
    TestInitialize → Test1 → TestCleanup
    TestInitialize → Test2 → TestCleanup
  ClassCleanup
AssemblyCleanup
```

### NUnit
```
[SetUpFixture] OneTimeSetUp          ← assembly level
  [OneTimeSetUp]                     ← class level (same instance shared)
    [SetUp] → Test1 → [TearDown]
    [SetUp] → Test2 → [TearDown]
  [OneTimeTearDown]
[SetUpFixture] OneTimeTearDown
```

### xUnit
```
CollectionFixture.ctor               ← assembly/collection level
  ClassFixture.ctor                  ← class level (shared)
    new TestClass() → Test1 → Dispose()   ← fresh instance per test
    new TestClass() → Test2 → Dispose()
  ClassFixture.Dispose()
CollectionFixture.Dispose()
```

---

## Key Differences

| Concern | MSTest | NUnit | xUnit |
|---|---|---|---|
| **Class instance** | New per test | Shared across tests | New per test |
| **Setup style** | Attributes | Attributes | Constructor / `IDisposable` |
| **Async lifecycle** | Partial support | `[SetUp]` can be async | `IAsyncLifetime` |
| **Shared state** | Class/Assembly init | `[OneTimeSetUp]` | Fixtures via DI |
| **Parallelism control** | `[DoNotParallelize]` | `[Parallelizable]` | `[Collection]` |

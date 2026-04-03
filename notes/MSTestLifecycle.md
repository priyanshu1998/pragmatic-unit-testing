# MSTest Lifecycle

This document summarizes the execution lifecycle of tests in MSTest.

---

## 1. Assembly Level (once per test run)

Runs once for the entire test assembly.

* `[AssemblyInitialize]`
* `[AssemblyCleanup]`

### Order

1. `AssemblyInitialize` → before any test runs
2. `AssemblyCleanup` → after all tests finish

### Example

```csharp
[AssemblyInitialize]
public static void Init(TestContext context) { }

[AssemblyCleanup]
public static void Cleanup() { }
```

---

## 2. Class Level (per test class)

Executed once per test class.

* `[ClassInitialize]`
* `[ClassCleanup]`

### Order

1. `ClassInitialize` → before any test methods in the class
2. `ClassCleanup` → after all test methods in the class

### Example

```csharp
[ClassInitialize]
public static void ClassInit(TestContext context) { }

[ClassCleanup]
public static void ClassCleanup() { }
```

---

## 3. Test Level (per test method)

Executed for each individual test.

* `[TestInitialize]`
* `[TestMethod]`
* `[TestCleanup]`

### Order

For each test:

1. `TestInitialize`
2. `TestMethod`
3. `TestCleanup`

### Example

```csharp
[TestInitialize]
public void Setup() { }

[TestMethod]
public void MyTest() { }

[TestCleanup]
public void TearDown() { }
```

---

## Full Execution Order

```
AssemblyInitialize

  Class1:
    ClassInitialize
      TestInitialize
      TestMethod1
      TestCleanup

      TestInitialize
      TestMethod2
      TestCleanup
    ClassCleanup

  Class2:
    ClassInitialize
      TestInitialize
      TestMethod1
      TestCleanup
    ClassCleanup

AssemblyCleanup
```

---

## Important Notes

### Instance Lifecycle

* A new instance of the test class is created per test method.
* Constructors run before each test.
* Instance fields are not shared between tests.

### Constructor vs TestInitialize

Order inside a single test:

```
Constructor → TestInitialize → TestMethod → TestCleanup
```

* Use constructor for lightweight setup.
* Use `TestInitialize` for full setup.

### Parallel Execution

* MSTest can run tests in parallel.
* Avoid shared mutable state.

### Async Support

* Lifecycle methods can be `async Task`.
* Avoid `async void`.

### Inheritance Behavior

* Base class `TestInitialize` runs before derived class.
* Base class `TestCleanup` runs after derived class.

---

## Mental Model

| Scope    | Frequency       | Use Case                  |
| -------- | --------------- | ------------------------- |
| Assembly | Once            | Global setup              |
| Class    | Per test class  | Shared fixtures           |
| Test     | Per test method | Isolation and fresh state |

---

## Common Pitfall

Putting expensive setup in the constructor leads to repeated execution per test.

Prefer using `ClassInitialize` for shared setup where possible.

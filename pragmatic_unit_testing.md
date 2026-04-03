# Patterns for Pragmatic Unit Testing

## Introduction
This document summarizes key concepts from *Patterns for Pragmatic Unit Testing*, focusing on writing **maintainable, reliable, and isolated unit tests**.

---

## What is Testing?
Testing verifies that software behaves as expected.

- Detects defects early
- Ensures confidence in code changes
- Acts as executable documentation

Good tests fail only when behavior changes—not implementation.

---

## Pragmatic Unit Testing
Core ideas:
- Test smallest unit of behavior
- Avoid external dependencies (DB, APIs)
- Use fakes/mocks
- Focus on behavior, not implementation

---

## Test Structure (AAA)
1. Arrange – Setup data
2. Act – Execute logic
3. Assert – Verify results

---

## DRY vs DAMP
- DRY: Avoid duplication
- DAMP: Prefer readability

Readable tests > overly abstract tests

---

## Test Doubles
- Fakes
- Mocks
- Stubs

Used to isolate dependencies.

---

## Infrastructure Patterns
- Abstract Test Class
- Global Fixture
- Template Test Class

---

## Parameterized Tests
Run same test with multiple inputs to improve coverage.

---

## Async Testing
Avoid flaky tests:
- Use callbacks/polling
- Avoid Thread.sleep

---

## Anti-patterns
- Global state dependency
- Overuse of mocks
- Long/complex tests
- Testing implementation

---

## Legacy Code
Legacy = code without tests

Strategies:
- Add tests first
- Use Golden Master
- Refactor safely

---

## Golden Master
Capture current behavior → refactor safely

---

## Test Pruning
Remove redundant tests

---

## Assertions
- Prefer single assertion
- Use expressive assertions

---

## Readability
- Clear naming
- Minimal logic
- Tests = documentation

---

## References
- Martin Fowler: https://martinfowler.com/testing/
- Test Doubles: https://martinfowler.com/bliki/TestDouble.html
- JUnit 5 Docs: https://junit.org/junit5/docs/current/user-guide/
- AssertJ: https://assertj.github.io/doc/
- Kent Beck TDD: https://www.amazon.com/Test-Driven-Development-Kent-Beck/dp/0321146530

# Turnstile Solution

A Test-Driven Development (TDD) solution in C# for simulating people passing through a university turnstile, following object-oriented design principles.

## Problem Description

A university has a turnstile to enter or exit a building. The goal is to determine the exact time each person will pass through the turnstile, where each person takes **one second** to pass through.

### Rules

When multiple people arrive at the turnstile simultaneously, the following priority rules apply:

1. **Idle Priority**: If the turnstile was not used in the previous second, priority is given to people **exiting**.
2. **Continuous Exit Flow**: If the previous person **exited**, the next person **exiting** goes first.
3. **Continuous Enter Flow**: If the previous person **entered**, the next person **entering** goes first.
4. **Same Direction**: If two people going in the same direction arrive at the same time, the **lower-indexed** one goes first.

> **Key Insight**: Rules 2 and 3 create a **continuous flow pattern** - once a direction starts, it continues until that queue is empty or time advances.

### Input Format

```
Line 1: n (number of people)
Line 2: n space-separated integers representing arrival times
Line 3: n space-separated integers representing directions (0 = enter, 1 = exit)
```

### Output Format

```
n space-separated integers representing the time each person passes through (indexed by person 0 to n-1)
```

### Constraints

- `1 ? n ? 10^5`
- `0 ? time[i] ? 10^9` for `0 ? i < n`
- `time[i] ? time[i+1]` for `0 ? i < n-1` (sorted by arrival time)
- `direction[i]` is either `0` (enter) or `1` (exit)

## Examples

### Example 1

**Input:**
```
4
0 0 1 5
0 1 1 0
```

**Output:**
```
2 0 1 5
```

**Explanation (by time):**
- **Time 0**: Person 0 (enter) and Person 1 (exit) arrive ? Rule 1 (idle prefers exit) ? Person 1 exits
- **Time 1**: Person 2 (exit) arrives, Person 0 still waiting ? Rule 2 (continuous exit flow) ? Person 2 exits
- **Time 2**: Person 0 (enter) still waiting ? Person 0 enters
- **Time 5**: Person 3 (enter) arrives ? Person 3 enters

### Example 2

**Input:**
```
5
0 1 1 3 3
0 1 0 0 1
```

**Output:**
```
0 2 1 4 3
```

**Explanation (by time):**
- **Time 0**: Person 0 (enter) arrives ? Person 0 enters
- **Time 1**: Person 1 (exit) and Person 2 (enter) arrive ? Rule 3 (continuous enter flow) ? Person 2 enters
- **Time 2**: Person 1 (exit) still waiting ? Person 1 exits
- **Time 3**: Person 3 (enter) and Person 4 (exit) arrive ? Rule 2 (continuous exit flow) ? Person 4 exits
- **Time 4**: Person 3 (enter) still waiting ? Person 3 enters

## Architecture & Design

This solution follows **Test-Driven Development (TDD)** and **SOLID principles** with a clean object-oriented design.

### Design Patterns

#### 1. **Strategy Pattern** (Priority Rules)

The priority rules are implemented as interchangeable strategies, allowing the simulator to select the appropriate rule based on the turnstile state.

```
IPriorityRule (interface)
??? IdlePreferExitRule (Rule 1)
??? PreviousExitPreferEnterRule (Rule 2)
??? PreviousEnterPreferExitRule (Rule 3)
```

#### 2. **Domain-Driven Design**

Clear separation of concerns with distinct layers:

- **Domain Layer**: Core entities (`Person`, `TurnstileDirection`)
- **Rules Layer**: Priority strategies (`IPriorityRule` implementations)
- **Simulation Layer**: Orchestration (`TurnstileSimulator`)
- **Application Layer**: I/O handling (`Program`)

### Key Components

#### Domain Classes

**`TurnstileDirection`** (Enum)
- Type-safe representation of directions
- `Enter = 0`, `Exit = 1`

**`Person`** (Class)
- Represents an individual with:
  - `Index`: Person identifier
  - `ArrivalTime`: When they arrive
  - `Direction`: Enter or Exit
  - `IsProcessed`: Tracking flag
  - `PassTime`: When they passed through
- Implements `IComparable<Person>` for automatic ordering (Rule 4)
- Includes validation in constructor and methods

#### Rules Layer

**`IPriorityRule`** (Interface)
- Defines the contract: `Person SelectNext(IList<Person> entering, IList<Person> exiting)`
- Allows runtime selection of priority strategy

**Concrete Rules** (Stateless)
- `IdlePreferExitRule`: Handles idle state (Rule 1)
- `PreviousExitPreferEnterRule`: Continuous exit flow (Rule 2)
- `PreviousEnterPreferExitRule`: Continuous enter flow (Rule 3)

#### Simulation Layer

**`TurnstileSimulator`**
- Main simulation engine
- Tracks current time and last direction
- Manages waiting people directly using lists
- Selects appropriate priority rule
- Coordinates the simulation loop
- Includes comprehensive input validation

### SOLID Principles Applied

? **Single Responsibility Principle**
- Each class has one clear purpose
- `Person` manages person state
- Rules encapsulate priority logic
- `TurnstileSimulator` orchestrates simulation and manages queues

? **Open/Closed Principle**
- Open for extension (can add new rules without modifying existing code)
- Closed for modification (existing rules don't change when adding new ones)

? **Liskov Substitution Principle**
- All `IPriorityRule` implementations are interchangeable
- Simulator works with any rule that implements the interface

? **Interface Segregation Principle**
- Small, focused interfaces
- `IPriorityRule` has only one method with clear responsibility

? **Dependency Inversion Principle**
- `TurnstileSimulator` depends on `IPriorityRule` abstraction
- Not coupled to concrete rule implementations

## Project Structure

```
TurnstileSolution/
??? TurnstileDirection.cs          # Enum for directions
??? Person.cs                       # Domain entity
??? IPriorityRule.cs               # Strategy interface
??? IdlePreferExitRule.cs          # Rule 1 implementation
??? PreviousExitPreferEnterRule.cs # Rule 2 implementation
??? PreviousEnterPreferExitRule.cs # Rule 3 implementation
??? TurnstileSimulator.cs          # Simulation engine
??? Program.cs                      # Console application

TurnstileSolution.Tests/
??? PersonTests.cs                  # Unit tests for Person
??? PriorityRuleTests.cs           # Unit tests for rules
??? TurnstileSimulatorTests.cs     # Integration tests

Documentation/
??? README.md                              # This file - project overview
??? DEVELOPMENT_LOG.md                     # Complete development history
??? CLEANUP_SUMMARY.md                     # Code cleanup documentation
??? DEPENDENCY_INVERSION_IMPROVEMENT.md    # SOLID improvements
??? PERFORMANCE_OPTIMIZATIONS.md           # Performance analysis
??? PERF_SUMMARY.md                        # Performance quick reference
??? COMMIT_MESSAGE.txt                     # Cleanup commit message
??? COMMIT_MESSAGE_DI.txt                  # DI fix commit message
??? COMMIT_MESSAGE_PERF.txt                # Performance commit message
```

## Development Approach

### Test-Driven Development (TDD)

This project was built following the **Red-Green-Refactor** cycle:

1. **?? RED Phase**: Wrote 20 comprehensive tests before any implementation
2. **?? GREEN Phase**: Implemented classes incrementally to make tests pass
3. **?? REFACTOR Phase**: Applied clean code and design patterns

### Test Coverage

**15 tests** covering:
- ? Unit tests for `Person` class (4 tests)
- ? Unit tests for priority rules (5 tests)
- ? Integration tests for complete simulation (6 tests)
  - Both provided examples
  - Same direction priority
  - Single person scenario
  - Time gaps (turnstile becomes idle)
  - Large time values (constraint validation)

## Building and Running

### Prerequisites

- .NET 10 SDK

### Build the Solution

```bash
dotnet build
```

### Run the Application

```bash
dotnet run --project TurnstileSolution
```

**Example input:**
```
4
0 0 1 5
0 1 1 0
```

**Expected output:**
```
2 0 1 5
```

### Run Tests

```bash
dotnet test
```

**Expected result:**
```
Passed! - Failed: 0, Passed: 15, Skipped: 0, Total: 15
```

### Test Individual Examples

```bash
# Example 1
Get-Content TurnstileSolution\example1_input.txt | dotnet run --project TurnstileSolution

# Example 2
Get-Content TurnstileSolution\example2_input.txt | dotnet run --project TurnstileSolution
```

## Key Insights

### Continuous Flow Pattern

The most important insight from this problem is that **Rules 2 and 3 create continuous flow**:

- Once exiting starts, it continues until no more people are exiting
- Once entering starts, it continues until no more people are entering
- This is **not** an alternating pattern

This design choice optimizes throughput by minimizing direction changes.

### Automatic Ordering (Rule 4)

By implementing `IComparable<Person>` and using `SortedSet<Person>`, Rule 4 (same direction, lower index first) is **automatically enforced** without explicit logic. This is an example of using the type system to encode business rules.

### Validation Strategy

The solution includes comprehensive validation:
- Input array length validation
- Duplicate index detection
- Arrival time ordering verification
- Index-position consistency checks
- Pass time validation (cannot pass before arrival)

## License

This is a demonstration project for educational purposes.

## Documentation

This repository includes comprehensive documentation covering the development process, design decisions, and optimization journey:

### ?? Core Documentation

#### **[README.md](README.md)** (This file)
Main project documentation covering:
- Problem description and rules
- Architecture and design patterns
- Building and running instructions
- Key insights and learnings

#### **[DEVELOPMENT_LOG.md](DEVELOPMENT_LOG.md)**
Complete conversation history documenting:
- Initial problem statement and clarifications
- TDD approach and planning discussions
- All implementation decisions and iterations
- Test-driven development process
- Bug fixes and rule corrections
- Full audit trail of the development session

### ?? Optimization Documentation

#### **[CLEANUP_SUMMARY.md](CLEANUP_SUMMARY.md)**
Code cleanup phase covering:
- Removal of unused `TurnstileQueue` abstraction
- Elimination of dead code and test artifacts
- Code simplification (13% reduction)
- Before/after metrics and benefits
- YAGNI and KISS principles applied

#### **[DEPENDENCY_INVERSION_IMPROVEMENT.md](DEPENDENCY_INVERSION_IMPROVEMENT.md)**
SOLID principles improvement covering:
- Fixed Dependency Inversion Principle violation
- Changed from concrete types to `IPriorityRule` interface
- Reduced coupling and improved testability
- Design pattern alignment (Strategy Pattern)
- Future extensibility opportunities

#### **[PERFORMANCE_OPTIMIZATIONS.md](PERFORMANCE_OPTIMIZATIONS.md)**
Detailed performance analysis covering:
- Identified bottlenecks (O(n² log n) complexity)
- Eliminated redundant operations (LINQ, sorting)
- Optimized algorithms (single-pass processing)
- Complexity reduction and allocation improvements
- Benchmark comparisons and trade-offs
- Code quality impact analysis

#### **[PERF_SUMMARY.md](PERF_SUMMARY.md)**
Quick performance summary covering:
- Key optimizations at a glance
- Performance metrics comparison table
- Expected real-world impact (20-30% speedup)
- Verification results
- Future optimization opportunities

### ?? Commit Messages

Pre-written comprehensive commit messages for version control:

#### **[COMMIT_MESSAGE.txt](COMMIT_MESSAGE.txt)**
Git commit message for cleanup phase:
- Removal of unused code and abstractions
- Simplification changes
- Impact metrics and benefits

#### **[COMMIT_MESSAGE_DI.txt](COMMIT_MESSAGE_DI.txt)**
Git commit message for dependency inversion fix:
- SOLID principles improvement
- Interface-based design
- Coupling reduction

#### **[COMMIT_MESSAGE_PERF.txt](COMMIT_MESSAGE_PERF.txt)**
Git commit message for performance optimizations:
- Complexity improvements
- Allocation reductions
- Benchmark data

### ?? Documentation Summary

| Document | Purpose | Audience |
|----------|---------|----------|
| **README.md** | Project overview | All users |
| **DEVELOPMENT_LOG.md** | Complete development history | Developers, reviewers |
| **CLEANUP_SUMMARY.md** | Code cleanup details | Maintainers |
| **DEPENDENCY_INVERSION_IMPROVEMENT.md** | SOLID improvements | Architects, senior devs |
| **PERFORMANCE_OPTIMIZATIONS.md** | Performance deep-dive | Performance engineers |
| **PERF_SUMMARY.md** | Quick perf overview | Tech leads, managers |
| **COMMIT_MESSAGE_*.txt** | Version control | Git users |

### ?? How to Use This Documentation

**For New Contributors:**
1. Start with **README.md** (this file) for project overview
2. Read **DEVELOPMENT_LOG.md** to understand design decisions
3. Review **CLEANUP_SUMMARY.md** to see code evolution

**For Code Reviewers:**
1. Check **COMMIT_MESSAGE_*.txt** for change context
2. Read specific optimization docs for technical details
3. Verify test coverage in **README.md**

**For Performance Analysis:**
1. Start with **PERF_SUMMARY.md** for quick overview
2. Deep-dive into **PERFORMANCE_OPTIMIZATIONS.md** for details
3. Cross-reference with code changes in commits

**For Architecture Review:**
1. Read **DEPENDENCY_INVERSION_IMPROVEMENT.md** for SOLID compliance
2. Review **CLEANUP_SUMMARY.md** for design evolution
3. Check **DEVELOPMENT_LOG.md** for decision rationale

### ? Documentation Quality

All documentation includes:
- ? Clear problem statements
- ? Before/after comparisons
- ? Metrics and measurements
- ? Code examples
- ? Benefits and trade-offs
- ? Verification results

---

**For the complete development journey from initial problem to optimized solution, start with [DEVELOPMENT_LOG.md](DEVELOPMENT_LOG.md)!**

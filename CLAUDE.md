# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Interaction Rules

This is a personal learning repository. The user is here to learn by doing — not by watching.

**NEVER write code, solutions, or implementations for the user.** Instead:

- Explain concepts, patterns, and approaches
- Ask guiding questions to lead the user toward the answer
- Point to relevant documentation, examples, or resources
- Review code the user has written and give feedback
- Hint at what's wrong when they're stuck, without giving the fix directly

This applies to all three tracks: CS fundamentals, algorithms, and AI. If the user asks you to solve a problem, guide them through the thinking process instead.

## Project Overview

Personal CS learning playground with three tracks:

- **Track A** — CS Fundamentals (IO, networking, OS, memory, data structures) in `cs-fundamentals/`
- **Track B** — Algorithm Training (LeetCode problems) in `algorithms_codes/`
- **Track C** — AI learning (frameworks, prompt engineering) in `ai/`

Documentation lives in `docs/` mirroring the same structure. Progress is tracked in `LEARNING_PLAN.md`.

## Build & Run Commands

### Algorithm Solutions (main focus)

```bash
# Build the algorithm solution
dotnet build algorithms_codes/csharp/my-csharp_leetcodes.sln

# Run the console test harness
dotnet run --project algorithms_codes/csharp/AlgorithmMain
```

### CS Fundamentals (.NET experiments)

```bash
# Build IO experiments
dotnet build cs-fundamentals/dotnet/io/IO_Learning.sln

# Build host service experiments
dotnet build cs-fundamentals/dotnet/host_service/host_service.sln

# Clean all build artifacts (Windows only)
cs-fundamentals/dotnet/clean.cmd
```

## Architecture

### Algorithm Solutions (`algorithms_codes/csharp/`)

All projects target **net8.0**. The solution has one console entry point and multiple class library projects, each organized by algorithm pattern:

- **AlgorithmMain** — Console app that references all algorithm libraries. Contains manual test methods (e.g., `SumOfTwoTest()`, `RotateTest()`). Tests are toggled by commenting/uncommenting calls in `Main()`.
- **LinearAlgorithm** — Fast-slow pointer problems (LC 26, 27, 80, 283, 189)
- **MoveZero** — Earlier implementations of move/remove problems + merge sorted array
- **SumOfTwo** — Two Sum variants (brute force, two-pass hashmap, one-pass hashmap)
- **MaxArea** (MaxArea_SlidingWindow) — Container with most water, sliding window substring problems, trapping rain water
- **TwoPointers** — Opposite-direction pointer problems
- **SortSeries** — Matrix search problems
- **ArrayAlgorithm** — Grid/matrix problems (magic squares)

**Code conventions:**

- Algorithm methods are `public static` on the class
- Primary solution named `MethodNameBase()`, alternatives use descriptive suffixes (e.g., `MoveZeroesWithTwoPointer`, `MoveZeroWithLoop`)
- Uses modern C# features: tuple swaps `(a, b) = (b, a)`, collection expressions `[1, 2, 3]`, `Span<T>`

### CS Fundamentals (`cs-fundamentals/dotnet/`)

Uses **central package management** (`Directory.Packages.props`) — algorithm projects do NOT use this. Two separate solutions:

- **IO_Learning.sln** — Anonymous pipes, named pipes, process IO redirection experiments
- **host_service.sln** — .NET Generic Host / BackgroundService learning

### No Test Framework

There are no unit test projects. MSTest packages are defined in `Directory.Packages.props` but unused. All algorithm verification is done through `AlgorithmMain/Program.cs` by running test methods manually.

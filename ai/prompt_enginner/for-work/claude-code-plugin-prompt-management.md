# Claude Code Plugin: Prompt Management & Architecture Design

> Discussion summary — Internal monorepo migration plugin (Corext + net472 → net8/net10)

---

## Table of Contents

- [1. Current State](#1-current-state)
- [2. Prompt Management Best Practices](#2-prompt-management-best-practices)
- [3. Testing Strategy for Prompts](#3-testing-strategy-for-prompts)
- [4. Tools & Frameworks](#4-tools--frameworks)
- [5. Trellis Framework Analysis](#5-trellis-framework-analysis)
- [6. Prompts vs Code: The Hybrid Approach](#6-prompts-vs-code-the-hybrid-approach)
- [7. Action Items & Next Steps](#7-action-items--next-steps)

---

## 1. Current State

### What's Built

| Component       | Details                                          |
| --------------- | ------------------------------------------------ |
| Skills          | ~20 skills for different migration patterns       |
| Agents          | ~3 agents orchestrating the workflow              |
| CLAUDE.md       | ~500 lines of base instructions                   |
| Total prompts   | ~30k words across all prompt files                |
| Output          | ~400 draft PRs generated in 3+ weeks              |
| Target          | Internal ADO repo, draft PRs for teammate review  |

### Current Execution Loop

```
Run-Loop.ps1 (7x24 autonomous)
    │
    ├── Fetch task properties from ADO work items
    ├── Gather code/project context
    ├── Call: claude -p "<this-turn-task>" ...<params>
    ├── AI generates migration + creates draft PR
    └── Mark result back to TODO.md
```

### Known Pain Points

- **Prompt review bottleneck** — Any word change in 30k prompts requires manual full review
- **No regression testing** — No way to know if a prompt edit broke the migration workflow
- **Long project chains** — Deep `<ProjectReference>` chains produce unreliable migration results
- **Easy-task bias** — Most successful PRs are simple/low-level; complex projects remain challenging

---

## 2. Prompt Management Best Practices

### 2.1 Treat Prompts as Code

Decompose monolithic prompts into composable, scoped units:

```
plugin/
├── prompts/
│   ├── claude.md                    # Base CLAUDE.md (assembled from parts)
│   ├── agents/
│   │   ├── migration-agent.md
│   │   ├── validation-agent.md
│   │   └── pr-agent.md
│   ├── skills/
│   │   ├── project-reference.md
│   │   ├── package-upgrade.md
│   │   ├── target-framework.md
│   │   └── ...
│   └── context/
│       ├── corext-patterns.md
│       ├── net472-to-net8.md
│       └── csproj-conventions.md
├── tests/
│   ├── fixtures/                    # Sample inputs (csproj, sln, etc.)
│   ├── snapshots/                   # Expected outputs (golden files)
│   └── eval/                        # Evaluation criteria
└── scripts/
    ├── assemble.ts                  # Compose final prompts from parts
    └── validate.ts                  # Lint/check prompts
```

### 2.2 Key Principles

- **Version prompts alongside plugin code** — Same repo, same branching, same review process
- **Add "why" comments to rules** — Makes it clear when a rule can be safely removed
- **Track rule usage** — If a rule hasn't been relevant in 50+ PRs, consider removing it
- **Layered structure** — Separate stable base rules from migration-phase rules that will be sunset

---

## 3. Testing Strategy for Prompts

### Three-Layer Testing Pyramid

```
         /\
        /  \        Layer 3: LLM-as-Judge Eval
       / $$ \       (selective, for critical skills)
      /──────\
     /        \     Layer 2: Snapshot / Golden-File Tests
    /  medium  \    (run on affected skills per PR)
   /────────────\
  /              \  Layer 1: Static Validation
 /    cheap       \ (run on every PR)
/──────────────────\
```

### Layer 1: Static Validation (Every PR, Fast)

- **Prompt linting** — Check for contradictory rules, undefined references, length limits
- **Diff-based review** — When a prompt file changes, CI flags which skills/agents are affected
- **Schema validation** — If skills have structured input/output, validate the schema still holds
- **Cross-reference check** — No broken skill or context references

### Layer 2: Snapshot Tests (Affected Skills, Medium Cost)

- Take real sample inputs (a net472 csproj with known patterns)
- Run the prompt + skill against it
- Compare output to a known-good "golden file" snapshot
- If the output changes, the test fails — developer must review and approve the new snapshot
- **Most practical approach** — catches regressions without full eval cost

### Layer 3: LLM-as-Judge Evaluation (Selective, Higher Cost)

- For complex migrations, have a second LLM evaluate the output against criteria
- Example assertions: "Does this PR correctly update TargetFramework?", "Are all ProjectReferences preserved?"
- Tools like `promptfoo` or `braintrust` can automate this
- Run on nightly/weekly schedule rather than every PR

### Recommended PR Pipeline

```
PR with prompt changes
  │
  ├── Static checks (fast, seconds)
  │   ├── Prompt diff summary → which skills affected?
  │   ├── Length / structure validation
  │   └── Cross-reference check (no broken references)
  │
  ├── Snapshot tests (medium, minutes)
  │   ├── Run each changed skill against fixture inputs
  │   ├── Compare to golden outputs
  │   └── Fail if unexpected diff
  │
  └── Eval tests (selective, expensive)
      ├── LLM-as-judge on migration quality
      └── Run on subset of real projects
```

---

## 4. Tools & Frameworks

### Comparison

| Tool | What It Does | Fit for This Use Case |
| --- | --- | --- |
| **[promptfoo](https://github.com/promptfoo/promptfoo)** | Prompt testing & eval framework | Define test cases per skill, run evals on PR, detect regressions |
| **[Trellis](https://docs.trytrellis.app)** | Spec/context management, multi-platform AI framework | Organize specs, context injection, agent pipeline |
| **[Braintrust](https://www.braintrust.dev/)** | LLM eval & observability | Logging, scoring, tracking prompt quality over time |
| **Custom snapshot tests** | Golden-file comparison | Most practical for migration output validation |
| **ESLint-style rules** | Static analysis on prompt files | Catch structural issues before runtime |

### Recommended Combination

- **Trellis** → Prompt organization + context injection + agent pipeline
- **promptfoo** (or custom) → Testing + regression detection as PR gate
- **Custom orchestration** → Your `Run-Loop.ps1` + deterministic pre-processing

---

## 5. Trellis Framework Analysis

> Source: [docs.trytrellis.app](https://docs.trytrellis.app)

### Architecture

```
.trellis/
├── spec/           # Specifications & coding standards
│   ├── shared/     #   Cross-cutting standards
│   ├── backend/    #   Backend-specific guides
│   └── frontend/   #   Frontend-specific guides
├── workspace/      # Session history & journals
├── tasks/          # Active task directories
│   └── {task}/
│       ├── task.json        # Metadata & status
│       ├── prd.md           # Requirements
│       ├── info.md          # Design notes
│       ├── implement.jsonl  # Spec routing for Implement Agent
│       ├── check.jsonl      # Spec routing for Check Agent
│       └── debug.jsonl      # Spec routing for Debug Agent
└── scripts/        # Automation hooks
    ├── session-start.py
    ├── inject-subagent-context.py
    └── ralph-loop.py
```

### What Trellis Solves

| Need | Trellis Solution |
| --- | --- |
| Prompt organization | `.trellis/spec/` with domain-scoped files |
| Context injection | Hooks + JSONL routing tables per agent |
| Agent pipeline | 6 built-in agents (dispatch → plan → implement → check) |
| Output verification | Ralph Loop — runs `dotnet build` etc., retries up to 5x |
| Session continuity | Workspace journals for cross-session memory |

### What Trellis Does NOT Solve

| Need | Gap |
| --- | --- |
| Prompt regression testing | No built-in eval/testing framework |
| CI gate for prompt changes | No snapshot tests or diff-based checks |
| "Did this edit break the workflow?" | Ralph Loop validates code output, not prompt correctness |

### JSONL Routing — Key Concept

Each agent gets only the specs it needs, configured per task:

```jsonl
{"file": ".trellis/spec/migration/net472-to-net8.md", "reason": "Migration rules"}
{"file": ".trellis/spec/migration/csproj-patterns.md", "reason": "Project file conventions"}
{"file": "src/legacy/", "type": "directory", "reason": "Existing code patterns"}
```

The `reason` field doubles as a completion marker: `"TypeCheck"` → `TYPECHECK_FINISH`.

### Ralph Loop — Programmatic Verification

```
Check Agent completes work
    │
    ├── ralph-loop.py reads worktree.yaml verify commands
    ├── Runs: dotnet build, dotnet test, lint, etc.
    │
    ├── All pass? → Allow agent to stop
    └── Any fail? → Block stop, send agent back to fix
                    (max 5 iterations)
```

---

## 6. Prompts vs Code: The Hybrid Approach

### The Spectrum

```
Pure Prompts ◄─────────────────────────────► Pure Code
(current state)          ▲                   (Semantic Kernel /
 30k words              │                    LangChain)
                    RECOMMENDED
                     (Hybrid)
```

### Why Not Stay Pure Prompts?

- 30k words is too much to review/test as text
- Deterministic logic (dependency analysis, XML parsing) is unreliable in prompts
- Branching logic ("if >5 references, do X") is hard to test in prompt form

### Why Not Go Full Framework?

- Would reimplement what Claude Code already does (file ops, git, code gen)
- Loses the flexibility that made 400 PRs possible
- Over-engineering for the use case

### The Hybrid Architecture

```
┌───────────────────────────────────────────────────┐
│  Orchestration Layer (Code — testable)             │
│  ├── Run-Loop.ps1 (existing)                       │
│  ├── Task planner (code, not prompt)               │
│  │   ├── Parse ADO work item                       │
│  │   ├── Analyze dependency graph (dotnet CLI)     │
│  │   ├── Determine migration type                  │
│  │   └── Select which skills to invoke             │
│  └── Result validator (code, not prompt)            │
│      ├── dotnet build check                        │
│      └── csproj schema validation                  │
├───────────────────────────────────────────────────┤
│  AI Layer (Prompts — much smaller, ~10-15k words)  │
│  ├── Skills: focused, single-purpose               │
│  │   "Migrate this csproj from net472 → net8       │
│  │    following these specific rules..."           │
│  └── Context: injected by orchestration            │
│      (not hardcoded in prompts)                    │
├───────────────────────────────────────────────────┤
│  Execution Engine: claude -p (unchanged)           │
└───────────────────────────────────────────────────┘
```

### What Moves to Code vs. Stays in Prompts

| Logic | Move to code? | Reasoning |
| --- | --- | --- |
| Analyze dependency graph | **Yes** | Deterministic — `dotnet` CLI gives exact data |
| "If project has >5 refs, do X" | **Yes** | Branching logic is testable in code |
| Read csproj, identify patterns | **Probably** | XML parsing in C# is more reliable than LLM |
| Generate the migrated csproj | **No — keep as prompt** | LLM creativity helps here |
| Write the PR description | **No — keep as prompt** | Natural language generation |

### MCP as the Bridge Layer

Custom MCP tools serve as the testable code layer Claude Code calls into:

```
Claude Code (prompt-driven)
    │
    ├── calls MCP tool: analyze_project("path.csproj")
    │   └── Returns structured data (deterministic, unit-testable)
    │
    ├── calls MCP tool: get_migration_rules(project_type)
    │   └── Returns applicable rules (deterministic, unit-testable)
    │
    └── LLM generates the actual migration
        (creative, prompt-driven, eval-testable)
```

### Before vs. After

**Before (all in prompts):**

```powershell
claude -p "Migrate project X. First analyze its dependencies,
then determine the migration strategy, then update the csproj,
then verify it builds... (500 more words of instructions)"
```

**After (hybrid):**

```powershell
# Code: deterministic analysis (testable)
$project = Get-ProjectInfo "path/to/project.csproj"
$deps = Get-DependencyChain $project
$strategy = Select-MigrationStrategy $project $deps
$context = Build-MigrationContext $project $deps       # structured JSON

# AI: focused generation (smaller, testable prompt)
claude -p "Migrate this csproj using strategy '$strategy'.
Context: $context
Rules: (2k words instead of 30k)"
```

---

## 7. Action Items & Next Steps

### Phase 1: Organize (Low Effort, High Impact)

- [ ] Decompose 30k-word prompts into scoped files (per skill, per agent, per context)
- [ ] Evaluate adopting Trellis for spec organization + context injection
- [ ] Add "why" comments to each major rule for future maintainability

### Phase 2: Test (Medium Effort, Highest Impact)

- [ ] Create fixture inputs — sample net472 csprojs covering common migration patterns
- [ ] Build snapshot/golden-file tests for top 5 most-used skills
- [ ] Set up ADO pipeline gate: prompt change → detect affected skills → run tests
- [ ] Evaluate `promptfoo` for eval framework or build custom solution

### Phase 3: Extract Deterministic Logic (Medium Effort, Long-term Win)

- [ ] Identify deterministic logic currently in prompts (dependency analysis, branching, XML parsing)
- [ ] Extract into MCP tools or PowerShell functions with unit tests
- [ ] Shrink prompts from ~30k to ~10-15k words focused on what LLMs actually need to do
- [ ] The ~15k words of deterministic logic becomes testable C#/PowerShell code

### Phase 4: Handle Complex Projects

- [ ] Bottom-up migration ordering — migrate leaf projects first, work upward
- [ ] Dependency context injection — summary of which deps are already migrated vs. still net472
- [ ] Confidence scoring — auto-flag "low confidence" PRs for harder projects
- [ ] Consider breaking complex migrations into multiple smaller PRs

---

## Key Takeaways

1. **Your current approach works** — 400 PRs in 3 weeks validates the architecture
2. **The biggest gap is testing** — invest in snapshot tests + PR gates for prompt changes
3. **Go hybrid** — move deterministic logic to code, keep creative generation in prompts
4. **Trellis helps with organization** but you still need a custom testing layer on top
5. **Incremental migration** — don't rewrite; extract the most brittle parts first

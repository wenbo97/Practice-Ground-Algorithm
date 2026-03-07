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

## 4. Tools & Frameworks for Prompt Management

### 4.1 Category Overview

Prompt tooling falls into three categories. Most tools span multiple, but each has a primary strength:

```
┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐
│   Organization   │  │    Testing &     │  │  Observability   │
│   & Versioning   │  │    Evaluation    │  │  & Monitoring    │
│                  │  │                  │  │                  │
│  PromptHub       │  │  promptfoo ★     │  │  LangSmith       │
│  Trellis ★       │  │  Agenta          │  │  Braintrust      │
│  PromptLayer     │  │  Braintrust      │  │  Agenta          │
│  Pezzo           │  │  LangSmith       │  │  Pezzo           │
│                  │  │  PromptHub       │  │                  │
└─────────────────┘  └─────────────────┘  └─────────────────┘
                    ★ = recommended for your use case
```

### 4.2 Detailed Comparison

#### promptfoo — Testing & Eval (Best Fit for Regression Testing)

> [promptfoo.dev](https://www.promptfoo.dev/) | Open Source | CLI + Library

| Feature | Details |
| --- | --- |
| **Core** | Test-driven prompt development with declarative test cases |
| **Assertions** | Configurable — exact match, contains, regex, LLM-as-judge, custom functions |
| **CI/CD** | GitHub Actions integration; runs entirely locally |
| **Comparison** | Side-by-side matrix view across prompts, models, and inputs |
| **Red teaming** | Automated vulnerability scanning for prompt injection, jailbreaks |
| **Providers** | OpenAI, Anthropic, Azure, Google, HuggingFace, custom APIs |
| **Cost** | Free, open source, no data leaves your machine |
| **Scale** | Battle-tested serving 10M+ users in production |

**Why it fits your case**: Define test cases per migration skill in YAML, run as ADO pipeline gate, detect regressions when any prompt changes. Supports snapshot-style testing out of the box.

```yaml
# Example: test a migration skill
prompts:
  - file://skills/project-reference.md
tests:
  - vars:
      input_csproj: file://fixtures/net472-with-refs.csproj
    assert:
      - type: contains
        value: "<TargetFramework>net8.0</TargetFramework>"
      - type: contains
        value: "<ProjectReference"
      - type: not-contains
        value: "Corext"
```

---

#### Trellis — Organization & Context Injection (Best Fit for Spec Management)

> [docs.trytrellis.app](https://docs.trytrellis.app/) | Open Source (AGPL-3.0)

| Feature | Details |
| --- | --- |
| **Core** | Structured spec management with auto-injection into AI agents |
| **Specs** | Domain-scoped files in `.trellis/spec/`, routed via JSONL per agent |
| **Hooks** | SessionStart, PreToolUse, SubagentStop — automate context loading |
| **Agents** | 6 built-in (dispatch, plan, research, implement, check, debug) |
| **Verification** | Ralph Loop — programmatic build/lint/test verification with retry |
| **Platforms** | Claude Code, Cursor, OpenCode, Codex, Kilo, Kiro, Gemini CLI |
| **Session memory** | Workspace journals for cross-session continuity |

**Why it fits your case**: Replaces your monolithic CLAUDE.md with scoped specs. Each skill only gets the context it needs via JSONL routing. (See [Section 5](#5-trellis-framework-analysis) for deep dive.)

---

#### PromptHub — Versioning & Collaboration (Best for Team Prompt Management)

> [prompthub.us](https://www.prompthub.us/) | SaaS

| Feature | Details |
| --- | --- |
| **Core** | Git-style prompt versioning with branches, diffs, merge requests |
| **Testing** | Playground, batch testing, side-by-side model comparison |
| **CI/CD Pipelines** | Runs evals on every commit/merge — detects secret leaks, regressions |
| **Collaboration** | Activity streams, fork/star system, team workspaces |
| **Deploy** | API to retrieve latest prompt from any branch at runtime |
| **Providers** | OpenAI, Anthropic, Azure, Google, Meta, AWS Bedrock, Mistral |

**Why it could fit**: If your team wants a visual dashboard for prompt editing with built-in PR-style review flow. The pipeline feature (eval on every commit) directly addresses your regression concern.

---

#### Agenta — Eval Platform with Prompt Management

> [github.com/Agenta-AI/agenta](https://github.com/Agenta-AI/agenta) | Open Source (MIT)

| Feature | Details |
| --- | --- |
| **Core** | LLMOps platform — prompt playground + evaluation + observability |
| **Eval** | 20+ built-in evaluators, LLM-as-judge, custom evaluators, human feedback |
| **Playground** | Side-by-side prompt comparison against test cases |
| **Versioning** | Full versioning with branching and environment management |
| **Observability** | OpenTelemetry-based tracing, cost/latency monitoring |
| **Deploy** | Self-hosted (Docker) or cloud |

**Why it could fit**: Strong eval capabilities. Good if you want both prompt management and structured evaluation in one tool. MIT licensed.

---

#### LangSmith — Full Lifecycle Platform

> [docs.langchain.com/langsmith](https://docs.langchain.com/langsmith) | SaaS (free tier available)

| Feature | Details |
| --- | --- |
| **Core** | Framework-agnostic platform for developing, debugging, deploying LLM apps |
| **Tracing** | Step-by-step visibility into every LLM call and agent action |
| **Eval** | Systematic evaluation with quality tracking over time |
| **Prompts** | Built-in versioning and collaboration |
| **Deploy** | Agent Server deployment for production |
| **Compliance** | HIPAA, SOC 2 Type 2, GDPR |

**Why it could fit**: Best if you're also building LangChain-based applications. Overkill if you only need prompt management for Claude Code skills.

---

#### Pezzo — Self-Hosted LLMOps

> [github.com/pezzolabs/pezzo](https://github.com/pezzolabs/pezzo) | Open Source (Apache 2.0)

| Feature | Details |
| --- | --- |
| **Core** | Cloud-native LLMOps — prompt management + observability + caching |
| **Prompt mgmt** | Centralized editing, instant delivery without redeployment |
| **Observability** | Real-time monitoring, request logging, analytics |
| **Caching** | Built-in caching for cost reduction (claims up to 90% savings) |
| **Deploy** | Self-hosted via Docker Compose (PostgreSQL + ClickHouse + Redis) |

**Why it could fit**: Good if you want a fully self-hosted solution within your internal network. Less focused on testing/eval than promptfoo.

---

#### Braintrust — Eval & Observability

> [braintrust.dev](https://www.braintrust.dev/) | SaaS + Open Source components

| Feature | Details |
| --- | --- |
| **Core** | LLM eval, logging, and scoring platform |
| **Eval** | Structured experiments with scoring functions |
| **Logging** | Production request logging with quality tracking |
| **Prompts** | Prompt playground and management |
| **CI** | Integrates into CI pipelines for automated eval |

**Why it could fit**: Strong eval + observability combo. Good for tracking prompt quality trends over time.

---

### 4.3 Decision Matrix for Your Use Case

| Need | Best Tool | Runner-Up |
| --- | --- | --- |
| **Prompt regression testing (PR gate)** | promptfoo | PromptHub (pipelines) |
| **Spec organization & context injection** | Trellis | Custom file structure |
| **Git-style prompt versioning with UI** | PromptHub | Agenta |
| **LLM-as-judge evaluation** | promptfoo | Agenta |
| **Production observability** | LangSmith | Braintrust |
| **Self-hosted, internal network** | Pezzo / Agenta | Custom solution |
| **Side-by-side prompt comparison** | Agenta | PromptHub |
| **Cost tracking** | LangSmith / Pezzo | Braintrust |

### 4.4 Recommended Stack for Your Plugin

```
┌─────────────────────────────────────────────────┐
│  Layer 1: Prompt Organization                    │
│  Trellis (.trellis/spec/ + JSONL routing)        │
│  — or custom file structure with assembler —     │
├─────────────────────────────────────────────────┤
│  Layer 2: Testing & Regression Detection         │
│  promptfoo (YAML test cases, CI gate in ADO)     │
├─────────────────────────────────────────────────┤
│  Layer 3: Orchestration                          │
│  Run-Loop.ps1 + MCP tools (deterministic logic)  │
├─────────────────────────────────────────────────┤
│  Layer 4: Execution                              │
│  claude -p (unchanged)                           │
└─────────────────────────────────────────────────┘
```

**Why this combination**:

- **Trellis** handles what you struggle with most today — organizing 30k words into scoped, auto-injected specs
- **promptfoo** directly solves the "did this change break something?" problem with CI-integrated eval
- **Keep your Run-Loop.ps1** — it works, and the hybrid approach (Section 6) incrementally improves it
- **No need for heavy platforms** like LangSmith or Pezzo unless you later need production observability

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

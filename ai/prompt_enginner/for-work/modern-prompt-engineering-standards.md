# Modern Prompt Engineering — Standards & Practices

> A comprehensive guide to prompt engineering as a professional discipline.
> Covers techniques, evaluation, lifecycle, and tooling.

---

## Table of Contents

- [1. The Standard Lifecycle](#1-the-standard-lifecycle)
- [2. Foundation: Define Success First](#2-foundation-define-success-first)
- [3. Core Techniques](#3-core-techniques)
- [4. Evaluation — The Engineering Core](#4-evaluation--the-engineering-core)
- [5. Team Maturity Levels](#5-team-maturity-levels)
- [6. Tools Landscape](#6-tools-landscape)
- [7. Key Takeaways](#7-key-takeaways)

---

## 1. The Standard Lifecycle

Prompt engineering has matured from "write better instructions" into a proper engineering
discipline with standards, lifecycle, and tooling.

```
Define Success ──► Write Prompt ──► Test/Eval ──► Iterate ──► Deploy ──► Monitor
     │                  │               │            │           │          │
  Criteria           Techniques      Assertions    Refine     Version    Observe
  Metrics            Patterns        Grading       A/B test   Gate       Trace
  Edge cases         Structure       Benchmark     Feedback   Promote    Alert
```

Each stage feeds into the next. Skipping "Define Success" is the most common mistake —
without measurable criteria, prompt engineering is just guessing.

---

## 2. Foundation: Define Success First

> Anthropic's official guidance: define success criteria and build evaluations
> **before** writing prompts.

### What Good Success Criteria Look Like

| Quality | Bad | Good |
| --- | --- | --- |
| **Specific** | "Good performance" | "Accurate sentiment classification" |
| **Measurable** | "Safe outputs" | "<0.1% of outputs flagged for toxicity in 10k trials" |
| **Achievable** | "100% accuracy" | "F1 score >= 0.85 (5% improvement over baseline)" |
| **Relevant** | "High citation accuracy" (for a chatbot) | "High citation accuracy" (for a medical app) |

### Common Criteria Dimensions

Most use cases need **multidimensional** evaluation:

- **Task fidelity** — How well does it perform the core task? Edge case handling?
- **Consistency** — Same question twice → semantically similar answers?
- **Relevance & coherence** — Directly addresses the question? Logically structured?
- **Tone & style** — Matches expectations for the audience?
- **Privacy preservation** — Handles sensitive info correctly?
- **Context utilization** — Builds on conversation history effectively?
- **Latency** — Acceptable response time?
- **Cost** — Within budget per API call?

### Before You Write a Single Prompt

1. Define measurable success criteria (from the dimensions above)
2. Build a test set with edge cases
3. Establish a baseline to improve against

---

## 3. Core Techniques

These are the established, documented techniques from Anthropic's official
prompting best practices — not opinions.

### 3.1 Be Clear and Direct

The #1 rule. Claude responds best to explicit instructions.

> **Golden rule:** Show your prompt to a colleague with minimal context.
> If they'd be confused, Claude will be too.

```
Less effective:
  "Create an analytics dashboard"

More effective:
  "Create an analytics dashboard. Include as many relevant features
   and interactions as possible. Go beyond the basics to create a
   fully-featured implementation."
```

- Be specific about desired output format and constraints
- Use numbered lists or bullet points when order/completeness matters

### 3.2 Add Context and Motivation

Explain **why**, not just **what**. Claude generalizes from the reasoning.

```
Less effective:
  "NEVER use ellipses"

More effective:
  "Your response will be read aloud by a text-to-speech engine,
   so never use ellipses since the TTS engine won't know how to
   pronounce them."
```

### 3.3 Few-Shot Examples

3-5 representative input/output pairs. One of the most reliable ways to steer
format, tone, and structure.

Best practices:

- **Relevant** — Mirror your actual use case
- **Diverse** — Cover edge cases; avoid unintended pattern pickup
- **Structured** — Wrap in `<example>` / `<examples>` tags

```xml
<examples>
  <example>
    <input>Customer email: "I want a refund immediately!"</input>
    <output>Classification: urgent, negative, refund_request</output>
  </example>
  <example>
    <input>Customer email: "Thanks, the issue is resolved."</input>
    <output>Classification: low_priority, positive, resolved</output>
  </example>
</examples>
```

### 3.4 XML Structuring

Use XML tags to separate instructions, context, examples, and inputs.
Reduces misinterpretation in complex prompts.

```xml
<instructions>
  Analyze the document and extract key findings.
</instructions>

<context>
  This is a quarterly financial report for Q3 2025.
</context>

<document>
  {{DOCUMENT_CONTENT}}
</document>
```

Best practices:

- Consistent, descriptive tag names across prompts
- Nest tags when content has natural hierarchy
- Use `<document index="n">` for multiple documents

### 3.5 Role Prompting

Set a role in the system prompt to focus behavior and tone.

```python
system = "You are a senior .NET architect specializing in
          migrating legacy Corext projects to net8.0."
```

Even a single sentence makes a meaningful difference.

### 3.6 Chain of Thought / Thinking

For math, logic, and multi-step reasoning.

**With extended thinking enabled:**
```
After receiving tool results, carefully reflect on their quality
and determine optimal next steps before proceeding.
```

**Without extended thinking (manual CoT):**
```
Think through this problem step by step.
Put your reasoning in <thinking> tags and your final answer
in <answer> tags.
```

Tips:

- "Think thoroughly" often produces better reasoning than prescriptive step-by-step plans
- Few-shot examples can include `<thinking>` tags to show the reasoning pattern
- "Before you finish, verify your answer against [criteria]" catches errors reliably

### 3.7 Prompt Chaining

Break a task into sequential API calls when you need to inspect or branch
at intermediate steps.

```
Step 1: Generate draft     → inspect output
Step 2: Review against criteria → inspect review
Step 3: Refine based on review → final output
```

Most common pattern: **self-correction** — generate → review → refine.

With modern adaptive thinking, Claude handles most multi-step reasoning internally.
Chain only when you need pipeline control.

### 3.8 Long Context Patterns (20k+ tokens)

- **Documents at the top**, query at the bottom (up to 30% quality improvement)
- **XML structure** for multi-document inputs with metadata
- **Quote-first**: Ask Claude to extract relevant quotes before answering

```xml
<documents>
  <document index="1">
    <source>annual_report_2023.pdf</source>
    <document_content>{{ANNUAL_REPORT}}</document_content>
  </document>
</documents>

Find relevant quotes first, place them in <quotes> tags,
then analyze based on those quotes.
```

### 3.9 Structured Output

When downstream code parses the output:

- Use the Structured Outputs feature (JSON schema enforcement)
- Or use tool calling with enum fields for classification
- Or ask explicitly: "Output valid JSON matching this schema: ..."
- Fallback: Direct the model to output within XML tags

### Quick Reference Table

| Technique | When to Use |
| --- | --- |
| Be clear and direct | Always — the #1 rule |
| Add context/motivation | When behavior needs to generalize |
| Few-shot examples (3-5) | When format/tone precision matters |
| XML structuring | Complex prompts with mixed content types |
| Role prompting | Domain-specific tasks |
| Chain of thought | Math, logic, multi-step reasoning |
| Prompt chaining | When you need to inspect/branch at intermediate steps |
| Long context patterns | 20k+ token inputs |
| Structured output | When downstream code parses the response |

---

## 4. Evaluation — The Engineering Core

This is where prompt engineering becomes **engineering**.

### 4.1 Three Grading Methods

In order of preference (fastest/most reliable first):

| Method | Speed | Reliability | Scalability | Use When |
| --- | --- | --- | --- | --- |
| **Code-based** | Fastest | Highest | Excellent | Exact match, contains, regex, schema |
| **LLM-as-judge** | Fast | High (with rubrics) | Good | Tone, quality, coherence — anything subjective |
| **Human grading** | Slow | Highest | Poor | Last resort — calibrate your LLM judge against this |

### 4.2 Standard Eval Patterns

#### Exact Match

Best for: Classification, extraction, categorical outputs.

```python
def evaluate_exact_match(model_output, correct_answer):
    return model_output.strip().lower() == correct_answer.lower()
```

#### String Contains / Regex

Best for: Structural checks ("does the output include X?").

```python
assert "<TargetFramework>net8.0</TargetFramework>" in output
assert "ProjectReference" in output
assert "Corext" not in output
```

#### Cosine Similarity (Embeddings)

Best for: Consistency — do paraphrased inputs produce semantically similar outputs?

```python
from sentence_transformers import SentenceTransformer
import numpy as np

model = SentenceTransformer("all-MiniLM-L6-v2")
embeddings = [model.encode(output) for output in outputs]
similarity = np.dot(embeddings[0], embeddings[1]) / (
    np.linalg.norm(embeddings[0]) * np.linalg.norm(embeddings[1])
)
```

#### ROUGE-L

Best for: Summary quality against a reference.

```python
from rouge import Rouge
rouge = Rouge()
scores = rouge.get_scores(model_output, reference_summary)
rouge_l_f1 = scores[0]["rouge-l"]["f"]
```

#### LLM-as-Judge (Likert Scale)

Best for: Subjective quality — tone, empathy, professionalism.

```python
rubric = """Rate this response on a scale of 1-5 for being professional:
<response>{output}</response>
1: Not at all professional
5: Perfectly professional
Think through your reasoning in <thinking> tags,
then output only the number in <result> tags."""
```

Tips for LLM-based grading:

- **Detailed, clear rubrics** — Specific criteria, not vague guidance
- **Empirical output** — "Output only 'correct' or 'incorrect'" or "Rate 1-5"
- **Encourage reasoning first** — "Think before scoring, then discard reasoning"
- **Use a different model** to grade than the one that generated the output

#### Binary Classification via LLM

Best for: Yes/no checks — "Does this contain PII?", "Is this safe?"

```python
prompt = """Does this response contain Personal Health Information?
<response>{output}</response>
Output only 'yes' or 'no'."""
```

### 4.3 Building an Eval Suite

1. **Start with edge cases** — These are where prompts fail
   - Irrelevant or nonexistent input
   - Overly long input
   - Ambiguous cases where even humans disagree
   - Adversarial or harmful input

2. **Prioritize volume over quality** — More test cases with automated grading
   beats fewer cases with human grading

3. **Use Claude to generate test cases** — Start with 10 hand-written examples,
   ask Claude to generate 100 more covering edge cases

4. **Design task-specific evals** — Mirror your real-world task distribution

---

## 5. Team Maturity Levels

### Level 1: Ad-hoc (Most Teams Start Here)

```
Developer writes prompt → eyeball test → ship it
```

Problems: No regression detection, no reproducibility, no confidence in changes.

### Level 2: Eval-Driven (The Target for Most Teams)

```
Developer writes/edits prompt
  → runs eval suite (promptfoo / custom)
  → assertions pass? → PR review → merge → deploy
  → assertions fail? → iterate on prompt
```

Key investment: Test fixtures + assertion definitions per skill/prompt.

### Level 3: Full Lifecycle (Enterprise Scale)

```
Prompt as code in repo
  → CI runs static checks + eval suite
  → Prompt versioned + deployed via API
  → Production traces logged (Phoenix / LangSmith)
  → Quality monitored, alerts on regression
  → Feedback loop → update evals → update prompt
```

Key investments: Observability platform, prompt registry, CI/CD pipeline.

### Moving Between Levels

| From → To | Highest Impact Action |
| --- | --- |
| Level 1 → 2 | Write 10 test cases per prompt + automate grading |
| Level 2 → 3 | Add production tracing + feedback-to-eval pipeline |

---

## 6. Tools Landscape

### By Category

```
┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
│  Writing &        │  │  Testing &        │  │  Monitoring &     │
│  Organizing       │  │  Evaluation       │  │  Observability    │
│                   │  │                   │  │                   │
│  Trellis          │  │  promptfoo        │  │  LangSmith        │
│  PromptHub        │  │  Agenta           │  │  Phoenix (Arize)  │
│  PromptLayer      │  │  Braintrust       │  │  Braintrust       │
│  Prompeteer (MCP) │  │  LangSmith        │  │  Pezzo            │
│                   │  │  PromptHub        │  │  Logfire           │
└──────────────────┘  └──────────────────┘  └──────────────────┘
```

### Quick Selection Guide

| Need | Best Tool |
| --- | --- |
| Organize prompts as specs with auto-injection | Trellis |
| Regression testing as PR gate | promptfoo |
| Git-style prompt versioning with UI | PromptHub |
| Self-hosted observability | Phoenix (Arize) |
| LLM-as-judge evaluation platform | Agenta |
| Production tracing + debugging | LangSmith |
| Prompt quality scoring via MCP | Prompeteer |

> For detailed tool comparisons, see
> [claude-code-plugin-prompt-management.md](./claude-code-plugin-prompt-management.md)
> Section 4.

---

## 7. Key Takeaways

### What Modern Prompt Engineering IS

1. **Define** what good looks like (success criteria with metrics)
2. **Write** the prompt using established techniques
3. **Test** against fixtures with automated grading
4. **Iterate** based on eval results, not vibes
5. **Deploy** with version control and gating
6. **Monitor** and feed failures back into evals

### What Modern Prompt Engineering IS NOT

- Knowing "magic words" or secret incantations
- Writing longer, more detailed instructions (sometimes shorter is better)
- One-shot prompt writing without testing
- Manual review as the primary quality gate

### The Single Most Important Insight

> The techniques are table stakes.
> **The eval loop** is what separates production prompt engineering
> from ad-hoc experimentation.

---

## References

- [Anthropic — Prompting Best Practices](https://docs.anthropic.com/en/docs/build-with-claude/prompt-engineering/claude-prompting-best-practices)
- [Anthropic — Define Success Criteria & Build Evals](https://docs.anthropic.com/en/test-and-evaluate/develop-tests)
- [Anthropic — Prompt Engineering Interactive Tutorial (GitHub)](https://github.com/anthropics/prompt-eng-interactive-tutorial)
- [promptfoo — Open Source Eval Framework](https://www.promptfoo.dev/)
- [Trellis — AI Coding Framework](https://docs.trytrellis.app/)

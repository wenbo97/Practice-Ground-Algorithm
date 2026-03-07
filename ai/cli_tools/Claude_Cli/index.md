# Claude CLI Learning and Use Cases

## How to use Claude -p with 1M Context

> call claude -p "/context" --model=opus[1m] --allow-dangerously-skip-permissions

```log
D:\A_Trainings\Practice-Ground-Algorithm>call claude -p "/context" --model=opus[1m] --allow-dangerously-skip-permissions                
## Context Usage

**Model:** claude-opus-4-6[1m]  
**Tokens:** 84 / 1000k (0%)

### Estimated usage by category

| Category | Tokens | Percentage |
|----------|--------|------------|
| System prompt | 9 | 0.0% |
| Skills | 74 | 0.0% |
| Messages | 1 | 0.0% |
| Free space | 966.9k | 96.7% |
| Autocompact buffer | 33k | 3.3% |

### Skills

| Skill | Source | Tokens |
|-------|--------|--------|
| keybindings-help | undefined | 61 |
| code-review:code-review | Plugin | 13 |
```

## Claude Code CLI Reference
- [SubAgent](https://code.claude.com/docs/en/sub-agents)
- [AgentTeam]()
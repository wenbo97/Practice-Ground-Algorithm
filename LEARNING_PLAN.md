# CS Learning System - Personal Roadmap

> Goal: Systematically build CS knowledge to power coding skills, with structured notes and spaced repetition for retention.

## Learning Philosophy

1. **Problem-Driven** - Learn from real problems, then connect to CS fundamentals
2. **Linked Knowledge** - Every concept connects to related topics
3. **Spaced Repetition** - Regular review prevents knowledge decay
4. **Theory + Code** - Every concept has working code examples
5. **Micro-Projects** - Small, targeted experiments (30min-2hr) to solidify understanding

---

## Micro-Projects Philosophy

> **Why micro-projects instead of big projects?**

| Approach | Pros | Cons |
|----------|------|------|
| Big Project (e.g., "Build a shell") | Impressive result | Scope creep, lose focus on CS concepts |
| Micro-Experiments (30min-2hr) | Targeted learning, quick feedback | Less "portfolio-worthy" |

**The Strategy:**
1. **Start with micro-experiments** - Each one tests ONE concept
2. **Combine into mini-project IF useful** - e.g., After doing "single command executor" + "pipe two processes", you basically have a simple shell
3. **Document the learning** - What did you observe? What surprised you?

**Example Path (IO → Simple Shell):**
```
Experiment: Single command executor (1hr)
    ↓ learned: Process.Start, RedirectStandardOutput
Experiment: Pipe two processes (1.5hr)  
    ↓ learned: Connect stdout→stdin between processes
Experiment: Handle stderr separately (30min)
    ↓ learned: Three streams, error handling
    
Result: You now have 90% of a simple shell, built incrementally!
```

**Code Location:** `dotnet/experiments/` (create as you go)

---

## Current Focus Areas

### Track A: CS Fundamentals

Status: IO & Networking (In Progress) → OS & Kernel (Next)

| Module | Status | Notes Location |
|--------|--------|----------------|
| IO & Networking | 🟡 In Progress | `docs/My Basic of CS/IO & Networking/` |
| OS & Kernel | ⚪ Not Started | `docs/My Basic of CS/The CPU & OS Kernel/` |
| Memory Management | ⚪ Not Started | `docs/My Basic of CS/Memory Hierarchy/` |
| Data Structures | ⚪ Not Started | `docs/My Basic of CS/Data Structure/` |

### Track B: Algorithm Training

Status: Phase 1 - Linear Structures (In Progress)

| Pattern | Progress | Review Date |
|---------|----------|-------------|
| Fast-Slow Pointers | 4/5 | - |
| Two Pointers (Opposite) | 0/6 | - |
| Sliding Window | 0/5 | - |
| Prefix Sum | 0/4 | - |

---

## Detailed Roadmap

### CS Fundamentals Roadmap

#### Module 1: IO & Networking (Current)

Core Question: *How does data flow between processes and machines?*

**Topics:**
- [x] Standard IO (stdin/stdout/stderr, file descriptors)
- [x] Pipes (Anonymous, Named, kernel buffer)
- [x] C# Process I/O (Console, Process.Start, RedirectStandard*)
- [x] IPC basics (Pipes, Shared Memory, Sync Primitives)
- [ ] Blocking vs Non-blocking IO
- [ ] Buffering (fully/line/unbuffered)
- [ ] Socket basics

**Completed Projects:**
- [x] AnonymousPipe Server/Client (`dotnet/io/AnonymousPipeServer/`)
- [x] NamedPipe with Process redirection (`dotnet/io/IO.Processor.Server.Learning/`)

**Micro-Experiments (30min-2hr each):**

| Experiment | Concept | Time | Status |
|------------|---------|------|--------|
| Blocking stdin read | What happens when no input? Program hangs - understand blocking | 30min | ⚪ |
| Add timeout to pipe read | Non-blocking with CancellationToken | 1hr | ⚪ |
| Intentional pipe deadlock | Create deadlock by filling buffer, then fix it | 1hr | ⚪ |
| Buffer size experiment | Write large data, observe chunking behavior | 30min | ⚪ |
| Simple stdio echo server | Read stdin, process, write stdout (MCP-style) | 1hr | ⚪ |
| Compare sync vs async pipe | Measure timing difference | 1hr | ⚪ |

**Key Connections:**
- Pipes → Process (fork, file descriptor inheritance)
- Named Pipes → IPC → Network Sockets (similar API patterns)
- Async IO → Thread scheduling (OS Kernel topic)

---

#### Module 2: OS & Kernel (Next)

Core Question: *How does code become execution? Who manages resources?*

**Topics:**
- [ ] Process Lifecycle (fork, exec, wait, exit)
- [ ] System Calls (user mode vs kernel mode)
- [ ] File Descriptor Table (deep dive)
- [ ] Thread vs Process (shared memory, thread pools)
- [ ] CPU Scheduling (Round Robin, Priority, CFS)
- [ ] Context Switching (what gets saved/restored)
- [ ] Synchronization (Mutex, Semaphore, Deadlock)

**Mini-Projects:**
- [ ] Simple shell in C# (execute commands, handle pipes)
- [ ] Task scheduler simulator
- [ ] Producer-Consumer with multiple threads

> Note: Mini-projects are built FROM micro-experiments. Complete experiments first, then combine into larger project if desired.

**Key Connections:**
- Process → Pipes (3 default file descriptors: stdin/stdout/stderr)
- System Calls → IO operations (read/write go through kernel)
- Thread Sync → IPC Sync Primitives (Mutex, Semaphore)

**Micro-Experiments (30min-2hr each):**

| Experiment | Concept | Time | Status |
|------------|---------|------|--------|
| Single command executor | Process.Start + capture output (minimal shell) | 1hr | ⚪ |
| Pipe two processes | `A.stdout → B.stdin` like `ls \| grep` | 1.5hr | ⚪ |
| Thread vs Task comparison | Same work, different approaches, measure | 1hr | ⚪ |
| Intentional deadlock | Two threads, two locks, observe hang | 30min | ⚪ |
| Fix deadlock with ordering | Apply lock ordering solution | 30min | ⚪ |
| Producer-Consumer queue | BlockingCollection with multiple threads | 1.5hr | ⚪ |
| Context switch observer | Measure time when many threads compete | 1hr | ⚪ |

---

#### Module 3: Memory Management (Future)

Core Question: *Where does data live? How is memory organized?*

**Topics:**
- [ ] Virtual Address Space (process memory layout)
- [ ] Stack vs Heap (allocation patterns)
- [ ] Page Tables & TLB
- [ ] Garbage Collection algorithms (Mark-Sweep, Generational)
- [ ] CPU Cache (L1/L2/L3, cache coherence)

**Key Connections:**
- Virtual Memory → Process isolation
- GC → C# runtime, memory pressure
- Cache → Algorithm optimization (cache-friendly code)

**Micro-Experiments (30min-2hr each):**

| Experiment | Concept | Time | Status |
|------------|---------|------|--------|
| Stack overflow demo | Deep recursion until crash, observe limit | 30min | ⚪ |
| Heap allocation patterns | Allocate objects, observe GC with diagnostics | 1hr | ⚪ |
| GC generations observer | Create objects, force GC, watch gen0/1/2 | 1hr | ⚪ |
| Large object heap | Allocate >85KB objects, observe LOH behavior | 30min | ⚪ |
| Memory-mapped file | Share data between two processes | 1.5hr | ⚪ |
| Cache-friendly vs unfriendly | Row-major vs column-major array access timing | 1hr | ⚪ |

---

#### Module 4: Data Structures (Foundation)

Core Question: *How to organize data for efficient access?*

**Topics:**
- [ ] Array (contiguous memory, O(1) access)
- [ ] Linked List (pointer-based, O(1) insert)
- [ ] Hash Table (hash functions, collision resolution)
- [ ] Tree structures (BST, balanced trees)
- [ ] Graph representations

**Key Connections:**
- Array → Memory layout, cache efficiency
- Hash Table → Collision → Linked List
- Tree → Recursion → Stack (memory)

**Micro-Experiments (30min-2hr each):**

| Experiment | Concept | Time | Status |
|------------|---------|------|--------|
| Implement dynamic array | ArrayList-like with resize logic | 1hr | ⚪ |
| Linked list from scratch | Node class, insert/delete/traverse | 1hr | ⚪ |
| Hash table with chaining | Handle collisions with linked list | 1.5hr | ⚪ |
| Hash table with probing | Linear/quadratic probing comparison | 1hr | ⚪ |
| Binary tree traversals | Implement all three + level order | 1.5hr | ⚪ |

---

### Algorithm Roadmap

#### Phase 1: Linear Structures (Current)

##### 1.1 Fast-Slow Pointers
Pattern: Two pointers moving at different speeds or conditions

- [x] LC 27. Remove Element `Easy`
- [x] LC 26. Remove Duplicates from Sorted Array `Easy`
- [x] LC 80. Remove Duplicates from Sorted Array II `Medium`
- [x] LC 283. Move Zeroes `Easy`
- [ ] LC 189. Rotate Array `Medium` ⬅️ **Next**

##### 1.2 Two Pointers (Opposite Direction)
Pattern: Pointers starting from both ends, moving toward center

- [ ] LC 344. Reverse String `Easy`
- [ ] LC 125. Valid Palindrome `Easy`
- [ ] LC 167. Two Sum II `Medium`
- [ ] LC 11. Container With Most Water `Medium`
- [ ] LC 15. 3Sum `Medium`
- [ ] LC 16. 3Sum Closest `Medium`

##### 1.3 Sliding Window
Pattern: Maintain a window that slides through the array

- [ ] LC 643. Maximum Average Subarray I `Easy`
- [ ] LC 209. Minimum Size Subarray Sum `Medium`
- [ ] LC 3. Longest Substring Without Repeating Characters `Medium`
- [ ] LC 1004. Max Consecutive Ones III `Medium`
- [ ] LC 76. Minimum Window Substring `Hard`

##### 1.4 Prefix Sum
Pattern: Precompute cumulative sums for O(1) range queries

- [ ] LC 724. Find Pivot Index `Easy`
- [ ] LC 303. Range Sum Query - Immutable `Easy`
- [ ] LC 560. Subarray Sum Equals K `Medium`
- [ ] LC 523. Continuous Subarray Sum `Medium`

---

#### Phase 2: Sorting & Searching (Future)

**Sorting Algorithms:**
- [ ] Merge Sort (divide & conquer, stable)
- [ ] Quick Sort (partition, in-place)
- [ ] Heap Sort (heap data structure)

**Binary Search Patterns:**
- [ ] Basic binary search
- [ ] Search insert position
- [ ] Search in rotated array
- [ ] Find first/last occurrence

---

#### Phase 3: Non-Linear Structures (Future)

**Linked List:**
- [ ] Reverse linked list
- [ ] Detect cycle
- [ ] Merge sorted lists

**Stack & Queue:**
- [ ] Valid parentheses
- [ ] Monotonic stack patterns
- [ ] BFS with queue

**Hash Table:**
- [ ] Two Sum (hash map)
- [ ] Group anagrams
- [ ] LRU Cache

---

#### Phase 4: Trees & Graphs (Future)

**Tree Traversals:**
- [ ] Inorder, Preorder, Postorder
- [ ] Level order (BFS)
- [ ] DFS patterns

**Graph Algorithms:**
- [ ] DFS & BFS
- [ ] Topological sort
- [ ] Shortest path (Dijkstra)

---

## Review Schedule

| Content Type | Review Interval | Method |
|--------------|-----------------|--------|
| New LC problem | 1 day → 3 days → 7 days | Re-solve without looking at solution |
| CS concept note | 3 days → 1 week → 2 weeks | Re-read, add connections |
| Mini-project | Monthly | Revisit code, extend features |

---

## Progress Log

### Current Week

**Date:** 2026-01-25

**Focus:**
- Created learning plan structure
- Reviewed existing notes and code

**Next Actions:**
- [ ] Finish LC 189 Rotate Array
- [ ] Complete IO module TODOs (Blocking vs Non-blocking)
- [ ] Start OS & Kernel module planning

---

## Quick Links

**CS Notes:**
- [IO & Networking](docs/My%20Basic%20of%20CS/IO%20&%20Networking/Index.md)
- [OS & Kernel](docs/My%20Basic%20of%20CS/The%20CPU%20&%20OS%20Kernel/Index.md)
- [Memory](docs/My%20Basic%20of%20CS/Memory%20Hierarchy/Index.md)

**Algorithm Notes:**
- [Algorithm Index](docs/Algorithm/Index.md)
- [LeetCode Top 100](docs/Algorithm/Leetcode%20Top%20100/0%20Index.md)

**Code:**
- [IO Examples](dotnet/io/)
- [Algorithm Solutions](LinearAlgorithm/)

---

## Resources

**Books:**
- [Operating Systems: Three Easy Pieces](https://pages.cs.wisc.edu/~remzi/OSTEP/)
- [High Performance Browser Networking](https://hpbn.co/)
- [Crafting Interpreters](https://craftinginterpreters.com/)

**Practice:**
- [LeetCode](https://leetcode.cn/)
- [NeetCode Roadmap](https://neetcode.io/roadmap)

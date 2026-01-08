> To clarify the concepts, we divide the IO system into three layers
## IO vs Stream vs Implementation

1. **IO (Input/Output)*
	- **What** - The **behavior** of data exchange.
	- **Context** - It is a subsystem in computer architecture (CPU, Memory, IO).
2. **Stream**
	- **How** - The **abstract model** of the IO behavior provided by programming languages and the OS.
	- **Concept** - Regardless of whether the underlying entity is a file, socket, or keyboard, a Stream treats all of them as an **ordered sequence of bytes**.
	- **Key** - A Stream is an **interface**, not the final physical implementation.
	- *Example* - Csharp's `System.IO.Stream` (abstract class), Go's `io.Reader` (interface).

3. **Underlying Mechanism**
	- **Implementation** - The actual carrier that supports the Stream.
	- **Components:**
		- **File Descriptor (fd)** - The kernel's index pointing to a specific Stream.
			- What is [File Descriptor](https://stackoverflow.com/a/5256705)
		- **Kernel Buffer** - The data transfer station (core implementation of pipes and streams).

## Concept Hierarchy

> We could think of the IO system as a three-layer structure

- **Level 1 - IO (Input / Output) -> The Domain (Problem Domain/Behavior)**
	- **Definition** - Refers to the process of moving data between memory (Memory) and the external world (Disk, Network, Terminal).
	- **Essence** - `I want to move data.`

- **Level 2 Stream -> The Abstraction (Abstract Mode)**
	- **Definition** - A logical model created by the operating system and programming languages to simplify IO operations. It treats data as an **ordered sequence of bytes**, rather than as physical blocks scattered across disk sectors.
	- **Essence** - `I treat data as a sequence of bytes.`

- **Level 3 File Descriptor & Buffer -> The Implementation (Low-Level Implementation)**
	- **Definition** - the mechanism used by the operating system kernel to actually manage IO.
	- **FD (File Descriptor)** - Track the ID of the stream.
	- **Buffer** - A memory block that temporarily holds the data.
	- **Essence** - `How the OS actually moves the bits.`

## Water System Analogy

> IO (behavior) - The act of moving water (from reservoir to your house.)

> Stream (Model) - The water flow. We do not need to know that the water in the reservoir is fetched bucket by bucket; to use, the water coming out of the faucet is a continuous "flow of water".

> Implementation - Pipes, pumps, and valves. These are the low-level components actually doing the work (corresponding to the kernel's Buffer and Driver)
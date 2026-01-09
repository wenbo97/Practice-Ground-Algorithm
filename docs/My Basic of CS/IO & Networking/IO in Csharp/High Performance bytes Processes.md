## Zero Allocation

`ReadOnlySpan<T>`, `ReadOnlyMemory<T>`

- **ReadOnlySpan**
	- **Limitation** - It is a `ref` struct and can only live one stack.
	- **Concequence** - It cannot be used after an await in an async method, not can it be stored as a field in class.
	- **Usage** - Intended for data parsing and processing within synchronous methods.

- **ReadonlyMemory**
	- **Characteristic** - It does not have the same restrictions as Span and can live on the heap.
	- **Usage** - Designed specifically for `async/await` scenarios.

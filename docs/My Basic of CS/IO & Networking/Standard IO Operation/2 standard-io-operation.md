# Standard IO & Pipeline Mechanism

## Core Mental Model

> *In One Sentence* - IO is a stream of bytes, and the pipe is the bridge connecting the streams.

- **Everything is a File**
	- Whether it is a regular file, hardware device, or network socket, the `OS` views them all as bytes streams.
	- Core operations: `Read()` and `Write()`.

- **Standard Streams**
	- Three "files" that are automatically opened when each process starts:
		- `stdin(0)` - Input source
		- `stdout(1)` - Output destination
		- `stderr(2)` - Error log
- **Pipe**
	- Essence: A **buffer in the kernel (Kernel Buffer)**
	- Function: Connects the `stdout(1)` of the previous process to the `stdin(0)` of the next process.
## How Pipeline Works
#fork #file-descriptor #buffer

> Lifecycle of pipe creation

1. **Create** - The parent process calls `pipe()` and gets two file descriptors (read end `r_fd`, write end `w_fd`).
2. **Fork** - The parent process `fork()` creates a child process, which inherits these two descriptors.
3. **Redirect**:
	- **Writer process** - Closes the read end and replaces `stdout(1)` with `w_fd` (using `dup2`).
	- **Reader process** - Closes the write end and replaces `stdin(0)` with `r_fd` (using `dup2`).
4. **Transfer** - Data flows into the kernel buffer, and the reader reads from the buffer.

## Key System Calls
* `pipe()` - Creates a pipe.
* `fork()` - Creates a process.
* `dup2() / dup()` - Copy/redirect file descriptors **(core concepts!)**.
* `read() / write()` - Read and write data.

### TODO
* [ ] **Buffering** - Differences between fully buffered, line-buffered, and unbuffered (Standard IO Library vs System Call).
* [ ] **Blocking vs Non-blocking** - What happens when the pipe is full or empty? (Blocking/Non-blocking).
* [ ] **Code Example** - Simulate the implementation of `ls | grep` using `C\Cpp` or `python`.
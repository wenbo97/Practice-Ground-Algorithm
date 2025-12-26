- "Once I use async/await, the program will run faster because it uses multiple threads." - This is a misconception.
- Core Truth:
	- Async **DOES NOT** mean multi-threading.
	- The primary purpose of async is to **avoid blocking** (non-blocking) and to **improve throughput**(scalability). Not to make a single calculation faster.

- Synchronous
	- Mock a synchronous without await/async, a thread was a task, if any I/O operations, data connections, the thread will do nothing but wait for the operations end. This thread is **blocked**

- Asynchronous
	- Codes are executing on a thread, when this thread meets any I/O operations, it will immediately return to the caller. A callback function will notify the thread or the other free threads to back to work. This thread is **released**, and fewer threads do more task.
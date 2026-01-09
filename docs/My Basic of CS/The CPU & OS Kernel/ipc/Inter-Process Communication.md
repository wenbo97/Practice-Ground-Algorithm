> High performance - intra-machine and OS kernel-level communication mechanism

1. **Pipe**
	- `AnonymousPipe`, `NamedPipe`
	- *Positioning* - The most basic form of stream-based transmission.

2. **Shared Memory**
	- `MemoryMappedFiles`
	- *Positioning* - The king of local machine performance, zero-copy data transfer.

3. **Synchronization Primitives**
	- `Mutex`, `Semaphore`, `EventWaitHandle`, `AutoResetEvent`
	- *Positioning* - Since this is IPC, in addition to transferring data, we must also learn to "transfer control signals" (e.g., A tells B to "stop").

4. **Unix Domain Sockets (UDS)**
	- `Socket`, `AddressFamily.Unix`
	- *Positioning* - Similar to network sockets but without going through the network interface card, a standard for inter-container communication in Docker/k8s.
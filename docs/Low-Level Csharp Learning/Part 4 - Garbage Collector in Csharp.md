Csharp introduce the GC(Garbage Collector) - We just need to "throw away the garbage", and it takes care of "collecting the garbage".

But at a low level, you need to understand that memory is divided into two distinct worlds: **Stack** and **Heap**.

## Stack
#Fast #Automatic #Temporary

What is it? It's like a stack of plates that you can only add to from the top.

Who resides here?
* **Value types**: `int`, `bool`, `double`, `struct`
* **Reference pointers**: The `remote control` that points to objects in the heap.
* **Characteristics**: **Extremely fast!** When a method finishes (i.e., when it hits the closing`}`), the data in the stack is automatically popped and discarded. The GC does not need to intervene.

## Heap
#Huge #Messy #Persistent

What is it? It's like a big, chaotic warehouse.

Who resides here?
* **Reference types**: All `class` instances (e.g., `string`, `List<T>`, custom classes).
* **Characteristics**: **Slower allocation**, **slower cleanup**. The heap is where GC spends most of its time.

As long as we use `new` keyword, the actual data resides here.

### What is "Garbage"

The GC periodically checks the **heap**. Its algorithm is simple: `Us anyone still referring to this object?` if no variable in the stack points to a particular object in the heap (e.g., you set the variable to `null` or it goes out of scope), the object becomes "garbage" and awaits GC cleanup.
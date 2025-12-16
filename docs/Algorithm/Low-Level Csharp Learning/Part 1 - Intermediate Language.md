## The Compilation Process - From Source Code to IL

- Many beginners think - The csharp compiler (Roslyn) compiles code directly into instructions that the CPU can run - that is incorrect.
- The truth is: The csharp compiler only does half of the work. It translates your csharp code into an intermediate language called IL (Intermediate Language), also known as MSIL.
	- Csharp is for humans to read.
	- Machine code is for CPU to read.
	- IL is for the .NET runtime(CLR) to read.

- Code segement:
	```csharp

  
public class Program  
{  
    public static int Add(int a, int b)  
    {        
        int result = a + b;  
        return result;  
    }  
    public static void Main(string[] args)  
    {        
        Console.WriteLine("Hello, World!");  
    }
}
```

- We will see "alien text" or "magic symbols":
```IL
.class public auto ansi beforefieldinit  
  ExampleMain.Program  
    extends [System.Runtime]System.Object  
{  
  
  .method public hidebysig static int32  
    Add(  
      int32 a,  
      int32 b  
    ) cil managed  
  {  
    .maxstack 2  
    .locals init (  
      [0] int32 result,  
      [1] int32 V_1  
    )  
  
    // [6 5 - 6 6]  
    IL_0000: nop  
  
    // [7 9 - 7 28]  
    IL_0001: ldarg.0      // a  
    IL_0002: ldarg.1      // b  
    IL_0003: add  
    IL_0004: stloc.0      // result  
  
    // [8 9 - 8 23]    IL_0005: ldloc.0      // result  
    IL_0006: stloc.1      // V_1  
    IL_0007: br.s         IL_0009  
  
    // [9 5 - 9 6]  
    IL_0009: ldloc.1      // V_1  
    IL_000a: ret  
  
  } // end of method Program::Add  
  
  .method public hidebysig specialname rtspecialname instance void    .ctor() cil managed  {    .maxstack 8  
    IL_0000: ldarg.0      // this    IL_0001: call         instance void [System.Runtime]System.Object::.ctor()    IL_0006: nop    IL_0007: ret  
  } // end of method Program::.ctor  
} // end of class ExampleMain.Program
```
- Above is the real form of the csharp code inside a .dll or .exe file.
- ***Stack-based machine***: IL instructions do not work on CPU registers like assembly code. Instead, they work on a ***Evaluation Stack***.
	- `ldarg` - load argument: pushes a value onto the top of the stack.
	- `add` - takes two values from the top of the stack, adds them together, and pushes the result back onto the stack.
	- `stloc` - store local: takes the value from the top of the stack and stores it in a local variable.
- It is platform-independent:
	- This IL code is exactly the same whether it runs on an Intel x64 PC or on an ARM-based mobile device. - "Compile once, run anywhere".
- Update code and check the extra IL code parts.
- ```csharp
namespace ExampleMain;  
  
public class Program  
{  
    public static int Add(int a, int b)  
    {        int result = (a + b) * 10;  
        return result;  
    }
}
```
- IL:
- ```IL
  .method public hidebysig static int32  
  Add(    int32 a,    int32 b  ) cil managed{  
  .maxstack 2  .locals init (    [0] int32 result,    [1] int32 V_1  )  
  // [6 5 - 6 6]  
  IL_0000: nop
  
  // [7 9 - 7 35]  
  IL_0001: ldarg.0      // a  
  IL_0002: ldarg.1      // b  
  IL_0003: add  IL_0004: ldc.i4.s     10 // 0x0a  
  IL_0006: mul  IL_0007: stloc.0      // result
  ```
- `ldc.i4.s  10` - Load Constant int 4 bytes Short form: push value `10` onto the top of the stack.
	- Now there are two values on the stack:
		- The result of `(a + b)`
		- `10`
- `mul` - Pops these two values, multiply them, push the result back onto the stack.
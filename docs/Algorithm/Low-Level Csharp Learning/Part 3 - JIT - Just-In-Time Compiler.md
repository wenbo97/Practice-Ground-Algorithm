https://sharplab.io/

- Check `JIT ASM` in above online lib
- ```asm
  Program..ctor()
    L0000: push ebp
    L0001: mov ebp, esp
    L0003: push edi
    L0004: push eax
    L0005: mov [ebp-8], ecx
    L0008: cmp dword ptr [0x287cc140], 0
    L000f: je short L0016
    L0011: call 0x736510c0
    L0016: mov ecx, [ebp-8]
    L0019: call dword ptr [0xbfa6388]
    L001f: nop
    L0020: nop
    L0021: pop ecx
    L0022: pop edi
    L0023: pop ebp
    L0024: ret
    Program.Add(Int32, Int32)
    L0000: push ebp
    L0001: mov ebp, esp
    L0003: sub esp, 0x10
    L0006: xor eax, eax
    L0008: mov [ebp-0xc], eax
    L000b: mov [ebp-0x10], eax
    L000e: mov [ebp-4], ecx
    L0011: mov [ebp-8], edx
    L0014: cmp dword ptr [0x287cc140], 0
    L001b: je short L0022
    L001d: call 0x736510c0
    L0022: nop
    L0023: mov eax, [ebp-4]
    L0026: add eax, [ebp-8]
    L0029: mov [ebp-0xc], eax
    L002c: mov eax, [ebp-0xc]
    L002f: mov [ebp-0x10], eax
    L0032: nop
    L0033: mov eax, [ebp-0x10]
    L0036: mov esp, ebp
    L0038: pop ebp
    L0039: ret
  ```
- IL: `ldarg.0`, `ldc.i4.s 10`, `mul` these are stack-based logic. It is *abstract*.
- JIT Assembly: `lea`, `imul`, `eax` these are register-based, physical execution. These are the instructions that actually run on the *CPU hardware*.
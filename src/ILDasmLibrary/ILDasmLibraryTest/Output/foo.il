
//  Copyright (c) Microsoft Corporation.  All rights reserved.



.assembly extern /* 23000001 */ 'mscorlib'
{
  .publickeytoken = ( B7 7A 5C 56 19 34 E0 89 )
  .ver 4:0:0:0
}
.assembly 'Demo1'
{
  .custom instance void [mscorlib]'System.Runtime.CompilerServices'.'CompilationRelaxationsAttribute'::.ctor(int32) = (01 00 08 00 00 00 00 00)
  .custom instance void [mscorlib]'System.Runtime.CompilerServices'.'RuntimeCompatibilityAttribute'::.ctor() = (01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78 63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01)
  .custom instance void [mscorlib]'System.Diagnostics'.'DebuggableAttribute'::.ctor(valuetype [mscorlib]'System.Diagnostics'.'DebuggableAttribute'/'DebuggingModes') = (01 00 07 01 00 00 00 00)
  .custom instance void [mscorlib]'System.Reflection'.'AssemblyTitleAttribute'::.ctor(string) = (01 00 05 44 65 6D 6F 31 00 00)
  .custom instance void [mscorlib]'System.Reflection'.'AssemblyDescriptionAttribute'::.ctor(string) = (01 00 00 00 00)
  .custom instance void [mscorlib]'System.Reflection'.'AssemblyConfigurationAttribute'::.ctor(string) = (01 00 00 00 00)
  .custom instance void [mscorlib]'System.Reflection'.'AssemblyCompanyAttribute'::.ctor(string) = (01 00 00 00 00)
  .custom instance void [mscorlib]'System.Reflection'.'AssemblyProductAttribute'::.ctor(string) = (01 00 05 44 65 6D 6F 31 00 00)
  .custom instance void [mscorlib]'System.Reflection'.'AssemblyCopyrightAttribute'::.ctor(string) = (01 00 12 43 6F 70 79 72 69 67 68 74 20 C2 A9 20 20 32 30 31 35 00 00)
  .custom instance void [mscorlib]'System.Reflection'.'AssemblyTrademarkAttribute'::.ctor(string) = (01 00 00 00 00)
  .custom instance void [mscorlib]'System.Runtime.InteropServices'.'ComVisibleAttribute'::.ctor(bool) = (01 00 00 00 00)
  .custom instance void [mscorlib]'System.Runtime.InteropServices'.'GuidAttribute'::.ctor(string) = (01 00 24 66 64 35 39 65 35 63 63 2D 31 64 31 36 2D 34 38 31 38 2D 62 63 63 63 2D 34 39 66 39 61 61 62 30 36 38 31 63 00 00)
  .custom instance void [mscorlib]'System.Reflection'.'AssemblyFileVersionAttribute'::.ctor(string) = (01 00 07 31 2E 30 2E 30 2E 30 00 00)
  .custom instance void [mscorlib]'System.Runtime.Versioning'.'TargetFrameworkAttribute'::.ctor(string) = (01 00 1A 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B 2C 56 65 72 73 69 6F 6E 3D 76 34 2E 35 01 00 54 0E 14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79 4E 61 6D 65 12 2E 4E 45 54 20 46 72 61 6D 65 77 6F 72 6B 20 34 2E 35)


  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module Demo1.exe
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003
.corflags 0x00020003

// =============== CLASS MEMBERS DECLARATION ===================

.class /* 02000002 */ private auto ansi beforefieldinit 'Demo1'.'Program' extends [mscorlib]'System'.'Object'
{
  .class /* 02000004 */ nested private auto ansi sealed serializable beforefieldinit '<>c' extends [mscorlib]'System'.'Object'
  {
    .custom instance void [mscorlib]'System.Runtime.CompilerServices'.'CompilerGeneratedAttribute'::.ctor() = (01 00 00 00)
    .field /* 04000005 */ public static initonly class 'Demo1'.'Program'/'<>c' '<>9'
    .field /* 04000006 */ public static class [mscorlib]'System'.'Comparison`1'<int32> '<>9__4_0'
    .method /* 0600000A */ private hidebysig static specialname rtspecialname void .cctor() cil managed
    {
      // code size       11 (0xb)
      .maxstack  8
      IL_0000:  /* 73   | (06)00000B       */ newobj       instance void 'Demo1'.'Program'/'<>c'::.ctor()
      IL_0005:  /* 80   | (04)000005       */ stsfld       class 'Demo1'.'Program'/'<>c' 'Demo1'.'Program'/'<>c'::'<>9'
      IL_000a:  /* 2A   |                  */ ret
    } // end of method .cctor

    .method /* 0600000B */ public hidebysig specialname rtspecialname instance void .ctor() cil managed
    {
      // code size        8 (0x8)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)00001F       */ call         instance void [mscorlib]'System'.'Object'::.ctor()
      IL_0006:  /* 00   |                  */ nop
      IL_0007:  /* 2A   |                  */ ret
    } // end of method .ctor

    .method /* 0600000C */ assembly hidebysig instance int32 '<Main>b__4_0'(int32 a, int32 b) cil managed
    {
      // code size       14 (0xe)
      .maxstack  2
      .locals init([0] int32 V_0)
      IL_0000:  /* 00   |                  */ nop
      IL_0001:  /* 0F   | 01               */ ldarga.s   a
      IL_0003:  /* 04   |                  */ ldarg.2
      IL_0004:  /* 28   | (0A)000029       */ call         instance int32 [mscorlib]'System'.'Int32'::'CompareTo'(int32)
      IL_0009:  /* 0A   |                  */ stloc.0
      IL_000a:  /* 2B   | 00               */ br.s       IL_000c
      IL_000c:  /* 06   |                  */ ldloc.0
      IL_000d:  /* 2A   |                  */ ret
    } // end of method '<Main>b__4_0'

  } // end of class 'Demo1'.'Program'/'<>c'

  .field /* 04000001 */ public static valuetype [mscorlib]'System'.'DateTime' 'date'
  .field /* 04000002 */ private static valuetype 'Demo1'.'UseReflection' '_reflection'
  .method /* 06000001 */ public hidebysig static specialname valuetype 'Demo1'.'UseReflection' 'get_Reflection'() cil managed
  {
    // code size       11 (0xb)
    .maxstack  1
    .locals init([0] valuetype 'Demo1'.'UseReflection' V_0)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 7E   | (04)000002       */ ldsfld       valuetype 'Demo1'.'UseReflection' 'Demo1'.'Program'::'_reflection'
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009
    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method 'get_Reflection'

  .method /* 06000002 */ private hidebysig static void 'Main'(string[] args) cil managed
  {
    .entrypoint
    // code size      322 (0x142)
    .maxstack  3
    .locals init([0] class [mscorlib]'System.Collections.Generic'.'List`1'<int32> V_0,
                 [1] class [mscorlib]'System.Collections.Generic'.'List`1'<int32> V_1,
                 [2] valuetype 'Demo1'.'UseReflection' V_2)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 73   | (0A)000010       */ newobj       instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::.ctor()
    IL_0006:  /* 0B   |                  */ stloc.1
    IL_0007:  /* 07   |                  */ ldloc.1
    IL_0008:  /* 17   |                  */ ldc.i4.1
    IL_0009:  /* 6F   | (0A)000011       */ callvirt     instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'Add'(!0)
    IL_000e:  /* 00   |                  */ nop
    IL_000f:  /* 07   |                  */ ldloc.1
    IL_0010:  /* 1C   |                  */ ldc.i4.6
    IL_0011:  /* 6F   | (0A)000011       */ callvirt     instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'Add'(!0)
    IL_0016:  /* 00   |                  */ nop
    IL_0017:  /* 07   |                  */ ldloc.1
    IL_0018:  /* 19   |                  */ ldc.i4.3
    IL_0019:  /* 6F   | (0A)000011       */ callvirt     instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'Add'(!0)
    IL_001e:  /* 00   |                  */ nop
    IL_001f:  /* 07   |                  */ ldloc.1
    IL_0020:  /* 1E   |                  */ ldc.i4.8
    IL_0021:  /* 6F   | (0A)000011       */ callvirt     instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'Add'(!0)
    IL_0026:  /* 00   |                  */ nop
    IL_0027:  /* 07   |                  */ ldloc.1
    IL_0028:  /* 1A   |                  */ ldc.i4.4
    IL_0029:  /* 6F   | (0A)000011       */ callvirt     instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'Add'(!0)
    IL_002e:  /* 00   |                  */ nop
    IL_002f:  /* 07   |                  */ ldloc.1
    IL_0030:  /* 18   |                  */ ldc.i4.2
    IL_0031:  /* 6F   | (0A)000011       */ callvirt     instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'Add'(!0)
    IL_0036:  /* 00   |                  */ nop
    IL_0037:  /* 07   |                  */ ldloc.1
    IL_0038:  /* 1B   |                  */ ldc.i4.5
    IL_0039:  /* 6F   | (0A)000011       */ callvirt     instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'Add'(!0)
    IL_003e:  /* 00   |                  */ nop
    IL_003f:  /* 07   |                  */ ldloc.1
    IL_0040:  /* 1F   | FFFFFFFF         */ ldc.i4.s   9
    IL_0042:  /* 6F   | (0A)000011       */ callvirt     instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'Add'(!0)
    IL_0047:  /* 00   |                  */ nop
    IL_0048:  /* 07   |                  */ ldloc.1
    IL_0049:  /* 1D   |                  */ ldc.i4.7
    IL_004a:  /* 6F   | (0A)000011       */ callvirt     instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'Add'(!0)
    IL_004f:  /* 00   |                  */ nop
    IL_0050:  /* 07   |                  */ ldloc.1
    IL_0051:  /* 1F   | FFFFFFFF         */ ldc.i4.s   10
    IL_0053:  /* 6F   | (0A)000011       */ callvirt     instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'Add'(!0)
    IL_0058:  /* 00   |                  */ nop
    IL_0059:  /* 07   |                  */ ldloc.1
    IL_005a:  /* 0A   |                  */ stloc.0
    IL_005b:  /* 7F   | (04)000001       */ ldsflda      valuetype [mscorlib]'System'.'DateTime' 'Demo1'.'Program'::'date'
    IL_0060:  /* 28   | (0A)000012       */ call         instance string [mscorlib]'System'.'DateTime'::'ToShortDateString'()
    IL_0065:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_006a:  /* 00   |                  */ nop
    IL_006b:  /* 28   | (0A)000014       */ call         void [mscorlib]'System'.'Console'::'WriteLine'()
    IL_0070:  /* 00   |                  */ nop
    IL_0071:  /* 72   | (70)000001       */ ldstr        "Welcome to ILDASM Demo"
    IL_0076:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_007b:  /* 00   |                  */ nop
    IL_007c:  /* 28   | (0A)000014       */ call         void [mscorlib]'System'.'Console'::'WriteLine'()
    IL_0081:  /* 00   |                  */ nop
    IL_0082:  /* 72   | (70)00002F       */ ldstr        "We are going to print a List<int>: "
    IL_0087:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_008c:  /* 00   |                  */ nop
    IL_008d:  /* 06   |                  */ ldloc.0
    IL_008e:  /* 28   | (06)000003       */ call         void 'Demo1'.'Program'::'PrintList'(class [mscorlib]'System.Collections.Generic'.'List`1'<int32>)
    IL_0093:  /* 00   |                  */ nop
    IL_0094:  /* 28   | (0A)000014       */ call         void [mscorlib]'System'.'Console'::'WriteLine'()
    IL_0099:  /* 00   |                  */ nop
    IL_009a:  /* 28   | (0A)000014       */ call         void [mscorlib]'System'.'Console'::'WriteLine'()
    IL_009f:  /* 00   |                  */ nop
    IL_00a0:  /* 72   | (70)000077       */ ldstr        "Now let's sort it with a lambda function and print it: "
    IL_00a5:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_00aa:  /* 00   |                  */ nop
    IL_00ab:  /* 06   |                  */ ldloc.0
    IL_00ac:  /* 7E   | (04)000006       */ ldsfld       class [mscorlib]'System'.'Comparison`1'<int32> 'Demo1'.'Program'/'<>c'::'<>9__4_0'
    IL_00b1:  /* 25   |                  */ dup
    IL_00b2:  /* 2D   | 17               */ brtrue.s   IL_00cb
    IL_00b4:  /* 26   |                  */ pop
    IL_00b5:  /* 7E   | (04)000005       */ ldsfld       class 'Demo1'.'Program'/'<>c' 'Demo1'.'Program'/'<>c'::'<>9'
    IL_00ba:  /* FE06 | (06)00000C       */ ldftn        instance int32 'Demo1'.'Program'/'<>c'::'<Main>b__4_0'(int32,int32)
    IL_00c0:  /* 73   | (0A)000015       */ newobj       instance void class [mscorlib]'System'.'Comparison`1'<int32>::.ctor(object,native int)
    IL_00c5:  /* 25   |                  */ dup
    IL_00c6:  /* 80   | (04)000006       */ stsfld       class [mscorlib]'System'.'Comparison`1'<int32> 'Demo1'.'Program'/'<>c'::'<>9__4_0'
    IL_00cb:  /* 6F   | (0A)000016       */ callvirt     instance void class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'Sort'(class [mscorlib]'System'.'Comparison`1'<!0>)
    IL_00d0:  /* 00   |                  */ nop
    IL_00d1:  /* 06   |                  */ ldloc.0
    IL_00d2:  /* 28   | (06)000003       */ call         void 'Demo1'.'Program'::'PrintList'(class [mscorlib]'System.Collections.Generic'.'List`1'<int32>)
    IL_00d7:  /* 00   |                  */ nop
    IL_00d8:  /* 72   | (70)0000E7       */ ldstr        "What about we do some recursion and call fibonacci(10)?"
    IL_00dd:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_00e2:  /* 00   |                  */ nop
    IL_00e3:  /* 72   | (70)000157       */ ldstr        "fibonacci(10) = "
    IL_00e8:  /* 1F   | FFFFFFFF         */ ldc.i4.s   10
    IL_00ea:  /* 28   | (06)000004       */ call         int32 'Demo1'.'Program'::'fibonacci'(int32)
    IL_00ef:  /* 8C   | (01)00001B       */ box          [mscorlib]'System'.'Int32'
    IL_00f4:  /* 28   | (0A)000017       */ call         string [mscorlib]'System'.'String'::'Concat'(object,object)
    IL_00f9:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_00fe:  /* 00   |                  */ nop
    IL_00ff:  /* 28   | (0A)000014       */ call         void [mscorlib]'System'.'Console'::'WriteLine'()
    IL_0104:  /* 00   |                  */ nop
    IL_0105:  /* 72   | (70)000179       */ ldstr        "Now let's use some reflection to get all the InlineNone OpCodes and print them..."
    IL_010a:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_010f:  /* 00   |                  */ nop
    IL_0110:  /* 28   | (0A)000014       */ call         void [mscorlib]'System'.'Console'::'WriteLine'()
    IL_0115:  /* 00   |                  */ nop
    IL_0116:  /* 28   | (0A)000014       */ call         void [mscorlib]'System'.'Console'::'WriteLine'()
    IL_011b:  /* 00   |                  */ nop
    IL_011c:  /* 28   | (06)000001       */ call         valuetype 'Demo1'.'UseReflection' 'Demo1'.'Program'::'get_Reflection'()
    IL_0121:  /* 0C   |                  */ stloc.2
    IL_0122:  /* 12   | 02               */ ldloca.s   V_2
    IL_0124:  /* 28   | (06)000009       */ call         instance void 'Demo1'.'UseReflection'::'ReflectSomeOpcodes'()
    IL_0129:  /* 00   |                  */ nop
    IL_012a:  /* 28   | (0A)000014       */ call         void [mscorlib]'System'.'Console'::'WriteLine'()
    IL_012f:  /* 00   |                  */ nop
    IL_0130:  /* 72   | (70)00021E       */ ldstr        "Now let's throw an exception, catch it and display the message: "
    IL_0135:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_013a:  /* 00   |                  */ nop
    IL_013b:  /* 28   | (06)000005       */ call         void 'Demo1'.'Program'::'ThrowException'()
    IL_0140:  /* 00   |                  */ nop
    IL_0141:  /* 2A   |                  */ ret
  } // end of method 'Main'

  .method /* 06000003 */ public hidebysig static void 'PrintList'(class [mscorlib]'System.Collections.Generic'.'List`1'<int32> list) cil managed
  {
    // code size       61 (0x3d)
    .maxstack  2
    .locals init([0] int32 V_0,
                 [1] bool V_1,
                 [2] int32 V_2,
                 [3] bool V_3)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 0A   |                  */ stloc.0
    IL_0003:  /* 2B   | 2A               */ br.s       IL_002f
    IL_0005:  /* 00   |                  */ nop
    IL_0006:  /* 06   |                  */ ldloc.0
    IL_0007:  /* 16   |                  */ ldc.i4.0
    IL_0008:  /* FE02 |                  */ cgt
    IL_000a:  /* 0B   |                  */ stloc.1
    IL_000b:  /* 07   |                  */ ldloc.1
    IL_000c:  /* 2C   | 0D               */ brfalse.s  IL_001b
    IL_000e:  /* 00   |                  */ nop
    IL_000f:  /* 72   | (70)0002A1       */ ldstr        ", "
    IL_0014:  /* 28   | (0A)000018       */ call         void [mscorlib]'System'.'Console'::'Write'(string)
    IL_0019:  /* 00   |                  */ nop
    IL_001a:  /* 00   |                  */ nop
    IL_001b:  /* 02   |                  */ ldarg.0
    IL_001c:  /* 06   |                  */ ldloc.0
    IL_001d:  /* 6F   | (0A)000019       */ callvirt     instance !0 class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'get_Item'(int32)
    IL_0022:  /* 28   | (0A)00001A       */ call         void [mscorlib]'System'.'Console'::'Write'(int32)
    IL_0027:  /* 00   |                  */ nop
    IL_0028:  /* 00   |                  */ nop
    IL_0029:  /* 06   |                  */ ldloc.0
    IL_002a:  /* 0C   |                  */ stloc.2
    IL_002b:  /* 08   |                  */ ldloc.2
    IL_002c:  /* 17   |                  */ ldc.i4.1
    IL_002d:  /* 58   |                  */ add
    IL_002e:  /* 0A   |                  */ stloc.0
    IL_002f:  /* 06   |                  */ ldloc.0
    IL_0030:  /* 02   |                  */ ldarg.0
    IL_0031:  /* 6F   | (0A)00001B       */ callvirt     instance int32 class [mscorlib]'System.Collections.Generic'.'List`1'<int32>::'get_Count'()
    IL_0036:  /* FE04 |                  */ clt
    IL_0038:  /* 0D   |                  */ stloc.3
    IL_0039:  /* 09   |                  */ ldloc.3
    IL_003a:  /* 2D   | C9               */ brtrue.s   IL_0005
    IL_003c:  /* 2A   |                  */ ret
  } // end of method 'PrintList'

  .method /* 06000004 */ public hidebysig static int32 'fibonacci'(int32 n) cil managed
  {
    // code size       47 (0x2f)
    .maxstack  3
    .locals init([0] bool V_0,
                 [1] int32 V_1,
                 [2] bool V_2)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* FE01 |                  */ ceq
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 06   |                  */ ldloc.0
    IL_0007:  /* 2C   | 04               */ brfalse.s  IL_000d
    IL_0009:  /* 16   |                  */ ldc.i4.0
    IL_000a:  /* 0B   |                  */ stloc.1
    IL_000b:  /* 2B   | 20               */ br.s       IL_002d
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 17   |                  */ ldc.i4.1
    IL_000f:  /* FE01 |                  */ ceq
    IL_0011:  /* 0C   |                  */ stloc.2
    IL_0012:  /* 08   |                  */ ldloc.2
    IL_0013:  /* 2C   | 04               */ brfalse.s  IL_0019
    IL_0015:  /* 17   |                  */ ldc.i4.1
    IL_0016:  /* 0B   |                  */ stloc.1
    IL_0017:  /* 2B   | 14               */ br.s       IL_002d
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 17   |                  */ ldc.i4.1
    IL_001b:  /* 59   |                  */ sub
    IL_001c:  /* 28   | (06)000004       */ call         int32 'Demo1'.'Program'::'fibonacci'(int32)
    IL_0021:  /* 02   |                  */ ldarg.0
    IL_0022:  /* 18   |                  */ ldc.i4.2
    IL_0023:  /* 59   |                  */ sub
    IL_0024:  /* 28   | (06)000004       */ call         int32 'Demo1'.'Program'::'fibonacci'(int32)
    IL_0029:  /* 58   |                  */ add
    IL_002a:  /* 0B   |                  */ stloc.1
    IL_002b:  /* 2B   | 00               */ br.s       IL_002d
    IL_002d:  /* 07   |                  */ ldloc.1
    IL_002e:  /* 2A   |                  */ ret
  } // end of method 'fibonacci'

  .method /* 06000005 */ public hidebysig static void 'ThrowException'() cil managed
  {
    // code size       74 (0x4a)
    .maxstack  2
    .locals init([0] class [mscorlib]'System'.'SystemException' V_0)
    IL_0000:  /* 00   |                  */ nop
    .try
    {
      .try
      {
        IL_0001:  /* 00   |                  */ nop
        IL_0002:  /* 28   | (0A)000014       */ call         void [mscorlib]'System'.'Console'::'WriteLine'()
        IL_0007:  /* 00   |                  */ nop
        IL_0008:  /* 72   | (70)0002A7       */ ldstr        "Throwing exception with message."
        IL_000d:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
        IL_0012:  /* 00   |                  */ nop
        IL_0013:  /* 72   | (70)0002E9       */ ldstr        "ILDasm is awesome"
        IL_0018:  /* 73   | (0A)00001C       */ newobj       instance void [mscorlib]'System'.'SystemException'::.ctor(string)
        IL_001d:  /* 7A   |                  */ throw
      } // end .try
      catch [mscorlib]'System'.'SystemException'
      {
        IL_001e:  /* 0A   |                  */ stloc.0
        IL_001f:  /* 00   |                  */ nop
        IL_0020:  /* 72   | (70)00030D       */ ldstr        "Just cached an exception: "
        IL_0025:  /* 06   |                  */ ldloc.0
        IL_0026:  /* 6F   | (0A)00001D       */ callvirt     instance string [mscorlib]'System'.'Exception'::'get_Message'()
        IL_002b:  /* 28   | (0A)00001E       */ call         string [mscorlib]'System'.'String'::'Concat'(string,string)
        IL_0030:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
        IL_0035:  /* 00   |                  */ nop
        IL_0036:  /* 00   |                  */ nop
        IL_0037:  /* DE   | 00               */ leave.s    IL_0039
      } // end handler
      IL_0039:  /* DE   | 0E               */ leave.s    IL_0049
    } // end .try
    finally
    {
      IL_003b:  /* 00   |                  */ nop
      IL_003c:  /* 72   | (70)000343       */ ldstr        "Finally: Thank you for comming to this awesome demo."
      IL_0041:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
      IL_0046:  /* 00   |                  */ nop
      IL_0047:  /* 00   |                  */ nop
      IL_0048:  /* DC   |                  */ endfinally
    } // end handler
    IL_0049:  /* 2A   |                  */ ret
  } // end of method 'ThrowException'

  .method /* 06000006 */ public hidebysig specialname rtspecialname instance void .ctor() cil managed
  {
    // code size        8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call         instance void [mscorlib]'System'.'Object'::.ctor()
    IL_0006:  /* 00   |                  */ nop
    IL_0007:  /* 2A   |                  */ ret
  } // end of method .ctor

  .method /* 06000007 */ private hidebysig static specialname rtspecialname void .cctor() cil managed
  {
    // code size       21 (0x15)
    .maxstack  8
    IL_0000:  /* 28   | (0A)000020       */ call         valuetype [mscorlib]'System'.'DateTime' [mscorlib]'System'.'DateTime'::'get_Today'()
    IL_0005:  /* 80   | (04)000001       */ stsfld       valuetype [mscorlib]'System'.'DateTime' 'Demo1'.'Program'::'date'
    IL_000a:  /* 28   | (06)000008       */ call         valuetype 'Demo1'.'UseReflection' 'Demo1'.'UseReflection'::'Create'()
    IL_000f:  /* 80   | (04)000002       */ stsfld       valuetype 'Demo1'.'UseReflection' 'Demo1'.'Program'::'_reflection'
    IL_0014:  /* 2A   |                  */ ret
  } // end of method .cctor

  .property /* 17000001 */ valuetype 'Demo1'.'UseReflection' 'Reflection'()
  {
    .get valuetype 'Demo1'.'UseReflection' 'Demo1'.'Program'::'get_Reflection'()
  }

} // end of class 'Demo1'.'Program'

.class /* 02000003 */ public sequential ansi sealed beforefieldinit 'Demo1'.'UseReflection' extends [mscorlib]'System'.'ValueType'
{
  .field /* 04000003 */ private  valuetype [mscorlib]'System.Reflection.Emit'.'OpCode'[] 'oneByteOpCodes'
  .field /* 04000004 */ private  valuetype [mscorlib]'System.Reflection.Emit'.'OpCode'[] 'twoByteOpCodes'
  .method /* 06000008 */ public hidebysig static valuetype 'Demo1'.'UseReflection' 'Create'() cil managed
  {
    // code size       49 (0x31)
    .maxstack  2
    .locals init([0] valuetype 'Demo1'.'UseReflection' V_0,
                 [1] valuetype 'Demo1'.'UseReflection' V_1)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 12   | 00               */ ldloca.s   V_0
    IL_0003:  /* FE15 | (02)000003       */ initobj      'Demo1'.'UseReflection'
    IL_0009:  /* 12   | 00               */ ldloca.s   V_0
    IL_000b:  /* 20   | FFFFFFFF         */ ldc.i4     256
    IL_0010:  /* 8D   | (01)000017       */ newarr       [mscorlib]'System.Reflection.Emit'.'OpCode'
    IL_0015:  /* 7D   | (04)000003       */ stfld        valuetype [mscorlib]'System.Reflection.Emit'.'OpCode'[] 'Demo1'.'UseReflection'::'oneByteOpCodes'
    IL_001a:  /* 12   | 00               */ ldloca.s   V_0
    IL_001c:  /* 20   | FFFFFFFF         */ ldc.i4     256
    IL_0021:  /* 8D   | (01)000017       */ newarr       [mscorlib]'System.Reflection.Emit'.'OpCode'
    IL_0026:  /* 7D   | (04)000004       */ stfld        valuetype [mscorlib]'System.Reflection.Emit'.'OpCode'[] 'Demo1'.'UseReflection'::'twoByteOpCodes'
    IL_002b:  /* 06   |                  */ ldloc.0
    IL_002c:  /* 0B   |                  */ stloc.1
    IL_002d:  /* 2B   | 00               */ br.s       IL_002f
    IL_002f:  /* 07   |                  */ ldloc.1
    IL_0030:  /* 2A   |                  */ ret
  } // end of method 'Create'

  .method /* 06000009 */ public hidebysig instance void 'ReflectSomeOpcodes'() cil managed
  {
    // code size      357 (0x165)
    .maxstack  3
    .locals init([0] class [mscorlib]'System'.'Type' V_0,
                 [1] class [mscorlib]'System.Reflection'.'FieldInfo'[] V_1,
                 [2] class [mscorlib]'System.Reflection'.'FieldInfo'[] V_2,
                 [3] int32 V_3,
                 [4] class [mscorlib]'System.Reflection'.'FieldInfo' V_4,
                 [5] valuetype [mscorlib]'System.Reflection.Emit'.'OpCode' V_5,
                 [6] uint16 V_6,
                 [7] bool V_7,
                 [8] bool V_8,
                 [9] bool V_9,
                 [10] valuetype [mscorlib]'System.Reflection.Emit'.'OpCode'[] V_10,
                 [11] int32 V_11,
                 [12] valuetype [mscorlib]'System.Reflection.Emit'.'OpCode' V_12,
                 [13] bool V_13,
                 [14] valuetype [mscorlib]'System.Reflection.Emit'.'OpCode'[] V_14,
                 [15] int32 V_15,
                 [16] valuetype [mscorlib]'System.Reflection.Emit'.'OpCode' V_16,
                 [17] bool V_17)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* D0   | (01)000017       */ ldtoken      [mscorlib]'System.Reflection.Emit'.'OpCode'
    IL_0006:  /* 28   | (0A)000021       */ call         class [mscorlib]'System'.'Type' [mscorlib]'System'.'Type'::'GetTypeFromHandle'(valuetype [mscorlib]'System'.'RuntimeTypeHandle')
    IL_000b:  /* 0A   |                  */ stloc.0
    IL_000c:  /* D0   | (01)00001F       */ ldtoken      [mscorlib]'System.Reflection.Emit'.'OpCodes'
    IL_0011:  /* 28   | (0A)000021       */ call         class [mscorlib]'System'.'Type' [mscorlib]'System'.'Type'::'GetTypeFromHandle'(valuetype [mscorlib]'System'.'RuntimeTypeHandle')
    IL_0016:  /* 1F   | FFFFFFFF         */ ldc.i4.s   24
    IL_0018:  /* 6F   | (0A)000022       */ callvirt     instance class [mscorlib]'System.Reflection'.'FieldInfo'[] [mscorlib]'System'.'Type'::'GetFields'(valuetype [mscorlib]'System.Reflection'.'BindingFlags')
    IL_001d:  /* 0B   |                  */ stloc.1
    IL_001e:  /* 00   |                  */ nop
    IL_001f:  /* 07   |                  */ ldloc.1
    IL_0020:  /* 0C   |                  */ stloc.2
    IL_0021:  /* 16   |                  */ ldc.i4.0
    IL_0022:  /* 0D   |                  */ stloc.3
    IL_0023:  /* 38   | 0087             */ br         IL_00af
    IL_0028:  /* 08   |                  */ ldloc.2
    IL_0029:  /* 09   |                  */ ldloc.3
    IL_002a:  /* 9A   |                  */ ldelem.ref
    IL_002b:  /* 13   | 04               */ stloc.s    V_4
    IL_002d:  /* 00   |                  */ nop
    IL_002e:  /* 11   | 04               */ ldloc.s    V_4
    IL_0030:  /* 6F   | (0A)000023       */ callvirt     instance class [mscorlib]'System'.'Type' [mscorlib]'System.Reflection'.'FieldInfo'::'get_FieldType'()
    IL_0035:  /* 06   |                  */ ldloc.0
    IL_0036:  /* 28   | (0A)000024       */ call         bool [mscorlib]'System'.'Type'::'op_Inequality'(class [mscorlib]'System'.'Type',class [mscorlib]'System'.'Type')
    IL_003b:  /* 13   | 07               */ stloc.s    V_7
    IL_003d:  /* 11   | 07               */ ldloc.s    V_7
    IL_003f:  /* 2C   | 02               */ brfalse.s  IL_0043
    IL_0041:  /* 2B   | 68               */ br.s       IL_00ab
    IL_0043:  /* 11   | 04               */ ldloc.s    V_4
    IL_0045:  /* 14   |                  */ ldnull
    IL_0046:  /* 6F   | (0A)000025       */ callvirt     instance object [mscorlib]'System.Reflection'.'FieldInfo'::'GetValue'(object)
    IL_004b:  /* A5   | (01)000017       */ unbox.any    [mscorlib]'System.Reflection.Emit'.'OpCode'
    IL_0050:  /* 13   | 05               */ stloc.s    V_5
    IL_0052:  /* 12   | 05               */ ldloca.s   V_5
    IL_0054:  /* 28   | (0A)000026       */ call         instance int16 [mscorlib]'System.Reflection.Emit'.'OpCode'::'get_Value'()
    IL_0059:  /* D1   |                  */ conv.u2
    IL_005a:  /* 13   | 06               */ stloc.s    V_6
    IL_005c:  /* 11   | 06               */ ldloc.s    V_6
    IL_005e:  /* 20   | FFFFFFFF         */ ldc.i4     256
    IL_0063:  /* FE04 |                  */ clt
    IL_0065:  /* 13   | 08               */ stloc.s    V_8
    IL_0067:  /* 11   | 08               */ ldloc.s    V_8
    IL_0069:  /* 2C   | 13               */ brfalse.s  IL_007e
    IL_006b:  /* 00   |                  */ nop
    IL_006c:  /* 02   |                  */ ldarg.0
    IL_006d:  /* 7B   | (04)000003       */ ldfld        valuetype [mscorlib]'System.Reflection.Emit'.'OpCode'[] 'Demo1'.'UseReflection'::'oneByteOpCodes'
    IL_0072:  /* 11   | 06               */ ldloc.s    V_6
    IL_0074:  /* 11   | 05               */ ldloc.s    V_5
    IL_0076:  /* A4   | (01)000017       */ stelem       [mscorlib]'System.Reflection.Emit'.'OpCode'
    IL_007b:  /* 00   |                  */ nop
    IL_007c:  /* 2B   | 2C               */ br.s       IL_00aa
    IL_007e:  /* 11   | 06               */ ldloc.s    V_6
    IL_0080:  /* 20   | FFFFFFFF         */ ldc.i4     65280
    IL_0085:  /* 5F   |                  */ and
    IL_0086:  /* 20   | FFFFFFFF         */ ldc.i4     65024
    IL_008b:  /* FE01 |                  */ ceq
    IL_008d:  /* 13   | 09               */ stloc.s    V_9
    IL_008f:  /* 11   | 09               */ ldloc.s    V_9
    IL_0091:  /* 2C   | 17               */ brfalse.s  IL_00aa
    IL_0093:  /* 00   |                  */ nop
    IL_0094:  /* 02   |                  */ ldarg.0
    IL_0095:  /* 7B   | (04)000004       */ ldfld        valuetype [mscorlib]'System.Reflection.Emit'.'OpCode'[] 'Demo1'.'UseReflection'::'twoByteOpCodes'
    IL_009a:  /* 11   | 06               */ ldloc.s    V_6
    IL_009c:  /* 20   | FFFFFFFF         */ ldc.i4     255
    IL_00a1:  /* 5F   |                  */ and
    IL_00a2:  /* 11   | 05               */ ldloc.s    V_5
    IL_00a4:  /* A4   | (01)000017       */ stelem       [mscorlib]'System.Reflection.Emit'.'OpCode'
    IL_00a9:  /* 00   |                  */ nop
    IL_00aa:  /* 00   |                  */ nop
    IL_00ab:  /* 09   |                  */ ldloc.3
    IL_00ac:  /* 17   |                  */ ldc.i4.1
    IL_00ad:  /* 58   |                  */ add
    IL_00ae:  /* 0D   |                  */ stloc.3
    IL_00af:  /* 09   |                  */ ldloc.3
    IL_00b0:  /* 08   |                  */ ldloc.2
    IL_00b1:  /* 8E   |                  */ ldlen
    IL_00b2:  /* 69   |                  */ conv.i4
    IL_00b3:  /* 3F   | FFFFFF70         */ blt        IL_0028
    IL_00b8:  /* 72   | (70)0003AD       */ ldstr        "One byte opcodes----------------:"
    IL_00bd:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_00c2:  /* 00   |                  */ nop
    IL_00c3:  /* 00   |                  */ nop
    IL_00c4:  /* 02   |                  */ ldarg.0
    IL_00c5:  /* 7B   | (04)000003       */ ldfld        valuetype [mscorlib]'System.Reflection.Emit'.'OpCode'[] 'Demo1'.'UseReflection'::'oneByteOpCodes'
    IL_00ca:  /* 13   | 0A               */ stloc.s    V_10
    IL_00cc:  /* 16   |                  */ ldc.i4.0
    IL_00cd:  /* 13   | 0B               */ stloc.s    V_11
    IL_00cf:  /* 2B   | 32               */ br.s       IL_0103
    IL_00d1:  /* 11   | 0A               */ ldloc.s    V_10
    IL_00d3:  /* 11   | 0B               */ ldloc.s    V_11
    IL_00d5:  /* A3   | (01)000017       */ ldelem       [mscorlib]'System.Reflection.Emit'.'OpCode'
    IL_00da:  /* 13   | 0C               */ stloc.s    V_12
    IL_00dc:  /* 00   |                  */ nop
    IL_00dd:  /* 12   | 0C               */ ldloca.s   V_12
    IL_00df:  /* 28   | (0A)000027       */ call         instance valuetype [mscorlib]'System.Reflection.Emit'.'OperandType' [mscorlib]'System.Reflection.Emit'.'OpCode'::'get_OperandType'()
    IL_00e4:  /* 1B   |                  */ ldc.i4.5
    IL_00e5:  /* FE01 |                  */ ceq
    IL_00e7:  /* 13   | 0D               */ stloc.s    V_13
    IL_00e9:  /* 11   | 0D               */ ldloc.s    V_13
    IL_00eb:  /* 2C   | 0F               */ brfalse.s  IL_00fc
    IL_00ed:  /* 00   |                  */ nop
    IL_00ee:  /* 12   | 0C               */ ldloca.s   V_12
    IL_00f0:  /* 28   | (0A)000028       */ call         instance string [mscorlib]'System.Reflection.Emit'.'OpCode'::'get_Name'()
    IL_00f5:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_00fa:  /* 00   |                  */ nop
    IL_00fb:  /* 00   |                  */ nop
    IL_00fc:  /* 00   |                  */ nop
    IL_00fd:  /* 11   | 0B               */ ldloc.s    V_11
    IL_00ff:  /* 17   |                  */ ldc.i4.1
    IL_0100:  /* 58   |                  */ add
    IL_0101:  /* 13   | 0B               */ stloc.s    V_11
    IL_0103:  /* 11   | 0B               */ ldloc.s    V_11
    IL_0105:  /* 11   | 0A               */ ldloc.s    V_10
    IL_0107:  /* 8E   |                  */ ldlen
    IL_0108:  /* 69   |                  */ conv.i4
    IL_0109:  /* 32   | C6               */ blt.s      IL_00d1
    IL_010b:  /* 28   | (0A)000014       */ call         void [mscorlib]'System'.'Console'::'WriteLine'()
    IL_0110:  /* 00   |                  */ nop
    IL_0111:  /* 72   | (70)0003F1       */ ldstr        "Two byte opcodes----------------:"
    IL_0116:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_011b:  /* 00   |                  */ nop
    IL_011c:  /* 00   |                  */ nop
    IL_011d:  /* 02   |                  */ ldarg.0
    IL_011e:  /* 7B   | (04)000004       */ ldfld        valuetype [mscorlib]'System.Reflection.Emit'.'OpCode'[] 'Demo1'.'UseReflection'::'twoByteOpCodes'
    IL_0123:  /* 13   | 0E               */ stloc.s    V_14
    IL_0125:  /* 16   |                  */ ldc.i4.0
    IL_0126:  /* 13   | 0F               */ stloc.s    V_15
    IL_0128:  /* 2B   | 32               */ br.s       IL_015c
    IL_012a:  /* 11   | 0E               */ ldloc.s    V_14
    IL_012c:  /* 11   | 0F               */ ldloc.s    V_15
    IL_012e:  /* A3   | (01)000017       */ ldelem       [mscorlib]'System.Reflection.Emit'.'OpCode'
    IL_0133:  /* 13   | 10               */ stloc.s    V_16
    IL_0135:  /* 00   |                  */ nop
    IL_0136:  /* 12   | 10               */ ldloca.s   V_16
    IL_0138:  /* 28   | (0A)000027       */ call         instance valuetype [mscorlib]'System.Reflection.Emit'.'OperandType' [mscorlib]'System.Reflection.Emit'.'OpCode'::'get_OperandType'()
    IL_013d:  /* 1B   |                  */ ldc.i4.5
    IL_013e:  /* FE01 |                  */ ceq
    IL_0140:  /* 13   | 11               */ stloc.s    V_17
    IL_0142:  /* 11   | 11               */ ldloc.s    V_17
    IL_0144:  /* 2C   | 0F               */ brfalse.s  IL_0155
    IL_0146:  /* 00   |                  */ nop
    IL_0147:  /* 12   | 10               */ ldloca.s   V_16
    IL_0149:  /* 28   | (0A)000028       */ call         instance string [mscorlib]'System.Reflection.Emit'.'OpCode'::'get_Name'()
    IL_014e:  /* 28   | (0A)000013       */ call         void [mscorlib]'System'.'Console'::'WriteLine'(string)
    IL_0153:  /* 00   |                  */ nop
    IL_0154:  /* 00   |                  */ nop
    IL_0155:  /* 00   |                  */ nop
    IL_0156:  /* 11   | 0F               */ ldloc.s    V_15
    IL_0158:  /* 17   |                  */ ldc.i4.1
    IL_0159:  /* 58   |                  */ add
    IL_015a:  /* 13   | 0F               */ stloc.s    V_15
    IL_015c:  /* 11   | 0F               */ ldloc.s    V_15
    IL_015e:  /* 11   | 0E               */ ldloc.s    V_14
    IL_0160:  /* 8E   |                  */ ldlen
    IL_0161:  /* 69   |                  */ conv.i4
    IL_0162:  /* 32   | C6               */ blt.s      IL_012a
    IL_0164:  /* 2A   |                  */ ret
  } // end of method 'ReflectSomeOpcodes'

} // end of class 'Demo1'.'UseReflection'

//Time elapsed: 00:00:00.1255394

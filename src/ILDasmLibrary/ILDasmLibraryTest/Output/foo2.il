
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.6.81.0
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v4.0.30319
.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000002*/ System.Core
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly /*20000001*/ Demo1
{
  .custom /*0C000002:0A000001*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000003:0A000002*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000004:0A000003*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 07 01 00 00 00 00 ) 

  .custom /*0C000005:0A000004*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 05 44 65 6D 6F 31 00 00 )                   // ...Demo1..
  .custom /*0C000006:0A000005*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000007:0A000006*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000008:0A000007*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000009:0A000008*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 05 44 65 6D 6F 31 00 00 )                   // ...Demo1..
  .custom /*0C00000A:0A000009*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 12 43 6F 70 79 72 69 67 68 74 20 C2 A9 20   // ...Copyright .. 
                                                                                                                                                               20 32 30 31 35 00 00 )                            //  2015..
  .custom /*0C00000B:0A00000A*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTrademarkAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 00 00 00 ) 
  .custom /*0C00000C:0A00000B*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*0100000C*/::.ctor(bool) /* 0A00000B */ = ( 01 00 00 00 00 ) 
  .custom /*0C00000D:0A00000C*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.GuidAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 24 66 64 35 39 65 35 63 63 2D 31 64 31 36   // ..$fd59e5cc-1d16
                                                                                                                                                               2D 34 38 31 38 2D 62 63 63 63 2D 34 39 66 39 61   // -4818-bccc-49f9a
                                                                                                                                                               61 62 30 36 38 31 63 00 00 )                      // ab0681c..
  .custom /*0C00000E:0A00000D*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 07 31 2E 30 2E 30 2E 30 00 00 )             // ...1.0.0.0..
  .custom /*0C00000F:0A00000E*/ instance void [mscorlib/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 1A 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                     2C 56 65 72 73 69 6F 6E 3D 76 34 2E 35 01 00 54   // ,Version=v4.5..T
                                                                                                                                                                     0E 14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C   // ..FrameworkDispl
                                                                                                                                                                     61 79 4E 61 6D 65 12 2E 4E 45 54 20 46 72 61 6D   // ayName..NET Fram
                                                                                                                                                                     65 77 6F 72 6B 20 34 2E 35 )                      // ework 4.5
  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module Demo1.exe
// MVID: {73D610B5-8A5E-4AB0-888F-C6A6B3B2E7A4}
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00020003    //  ILONLY 32BITPREFERRED
// Image base: 0x01780000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi beforefieldinit Demo1.Program
       extends [mscorlib/*23000001*/]System.Object/*01000010*/
{
  .field /*04000001*/ public static string str
  .field /*04000002*/ public static valuetype [mscorlib/*23000001*/]System.DateTime/*01000011*/ dt
  .field /*04000003*/ public static valuetype Demo1.StructTest/*02000003*/ st
  .field /*04000004*/ private static string '<Str>k__BackingField'
  .custom /*0C000011:0A00000F*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .custom /*0C000012:0A000010*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggerBrowsableAttribute/*01000014*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggerBrowsableState/*01000013*/) /* 0A000010 */ = ( 01 00 00 00 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname static 
          string  get_Str() cil managed
  // SIG: 00 00 0E
  {
    .custom /*0C000001:0A00000F*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2050
    // Code size       10 (0xa)
    .maxstack  1
    .locals /*11000001*/ init (string V_0)
    IL_0000:  /* 7E   | (04)000004       */ ldsfld     string Demo1.Program/*02000002*/::'<Str>k__BackingField' /* 04000004 */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 2B   | 00               */ br.s       IL_0008

    IL_0008:  /* 06   |                  */ ldloc.0
    IL_0009:  /* 2A   |                  */ ret
  } // end of method Program::get_Str

  .method /*06000002*/ public hidebysig specialname static 
          void  set_Str(string 'value') cil managed
  // SIG: 00 01 01 0E
  {
    .custom /*0C000010:0A00000F*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2066
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 80   | (04)000004       */ stsfld     string Demo1.Program/*02000002*/::'<Str>k__BackingField' /* 04000004 */
    IL_0006:  /* 2B   | 00               */ br.s       IL_0008

    IL_0008:  /* 2A   |                  */ ret
  } // end of method Program::set_Str

  .method /*06000003*/ private hidebysig static 
          void  Main(string[] args) cil managed
  // SIG: 00 01 01 1D 0E
  {
    .entrypoint
    // Method begins at RVA 0x2070
    // Code size       184 (0xb8)
    .maxstack  6
    .locals /*11000002*/ init (float64 V_0,
             int64 V_1,
             int32 V_2,
             float32 V_3,
             bool V_4)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 23   | 0000000000002440 */ ldc.r8     10.
    IL_000a:  /* 0A   |                  */ stloc.0
    IL_000b:  /* 21   | C701BDDE19000000 */ ldc.i8     0x19debd01c7
    IL_0014:  /* 0B   |                  */ stloc.1
    IL_0015:  /* 1F   | 19               */ ldc.i4.s   25
    IL_0017:  /* 0C   |                  */ stloc.2
    IL_0018:  /* 22   | BA493A41         */ ldc.r4     11.643
    IL_001d:  /* 0D   |                  */ stloc.3
    IL_001e:  /* 7F   | (04)000002       */ ldsflda    valuetype [mscorlib/*23000001*/]System.DateTime/*01000011*/ Demo1.Program/*02000002*/::dt /* 04000002 */
    IL_0023:  /* 28   | (0A)000011       */ call       instance int32 [mscorlib/*23000001*/]System.DateTime/*01000011*/::get_Day() /* 0A000011 */
    IL_0028:  /* 28   | (0A)000012       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(int32) /* 0A000012 */
    IL_002d:  /* 00   |                  */ nop
    IL_002e:  /* 7F   | (04)000003       */ ldsflda    valuetype Demo1.StructTest/*02000003*/ Demo1.Program/*02000002*/::st /* 04000003 */
    IL_0033:  /* 28   | (06)00000A       */ call       instance string Demo1.StructTest/*02000003*/::get_foo() /* 0600000A */
    IL_0038:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_003d:  /* 00   |                  */ nop
    IL_003e:  /* 28   | (06)000001       */ call       string Demo1.Program/*02000002*/::get_Str() /* 06000001 */
    IL_0043:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_0048:  /* 00   |                  */ nop
    IL_0049:  /* 72   | (70)000001       */ ldstr      "World" /* 70000001 */
    IL_004e:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_0053:  /* 00   |                  */ nop
    IL_0054:  /* 72   | (70)00000D       */ ldstr      "Goodbye" /* 7000000D */
    IL_0059:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_005e:  /* 00   |                  */ nop
    IL_005f:  /* 12   | 01               */ ldloca.s   V_1
    IL_0061:  /* 28   | (0A)000014       */ call       instance string [mscorlib/*23000001*/]System.Int64/*0100001E*/::ToString() /* 0A000014 */
    IL_0066:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_006b:  /* 00   |                  */ nop
    IL_006c:  /* 08   |                  */ ldloc.2
    IL_006d:  /* 1F   | 14               */ ldc.i4.s   20
    IL_006f:  /* FE02 |                  */ cgt
    IL_0071:  /* 13   | 04               */ stloc.s    V_4
    IL_0073:  /* 11   | 04               */ ldloc.s    V_4
    IL_0075:  /* 2C   | 11               */ brfalse.s  IL_0088

    IL_0077:  /* 00   |                  */ nop
    IL_0078:  /* 12   | 00               */ ldloca.s   V_0
    IL_007a:  /* 28   | (0A)000015       */ call       instance string [mscorlib/*23000001*/]System.Double/*0100001F*/::ToString() /* 0A000015 */
    IL_007f:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_0084:  /* 00   |                  */ nop
    IL_0085:  /* 00   |                  */ nop
    IL_0086:  /* 2B   | 0D               */ br.s       IL_0095

    IL_0088:  /* 12   | 02               */ ldloca.s   V_2
    IL_008a:  /* 28   | (0A)000016       */ call       instance string [mscorlib/*23000001*/]System.Int32/*01000020*/::ToString() /* 0A000016 */
    IL_008f:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_0094:  /* 00   |                  */ nop
    IL_0095:  /* 12   | 03               */ ldloca.s   V_3
    IL_0097:  /* 28   | (0A)000017       */ call       instance string [mscorlib/*23000001*/]System.Single/*01000021*/::ToString() /* 0A000017 */
    IL_009c:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_00a1:  /* 00   |                  */ nop
    IL_00a2:  /* 72   | (70)00001D       */ ldstr      "Test" /* 7000001D */
    IL_00a7:  /* 16   |                  */ ldc.i4.0
    IL_00a8:  /* 16   |                  */ ldc.i4.0
    IL_00a9:  /* 16   |                  */ ldc.i4.0
    IL_00aa:  /* 1F   | 20               */ ldc.i4.s   32
    IL_00ac:  /* 73   | (0A)000018       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000022*/<string>/*1B000001*/::.ctor() /* 0A000018 */
    IL_00b1:  /* 28   | (06)000004       */ call       void Demo1.Program/*02000002*/::Hola(string,
                                                                                            int32,
                                                                                            int32,
                                                                                            int32,
                                                                                            int32,
                                                                                            class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000015*/<string>) /* 06000004 */
    IL_00b6:  /* 00   |                  */ nop
    IL_00b7:  /* 2A   |                  */ ret
  } // end of method Program::Main

  .method /*06000004*/ public hidebysig static 
          void  Hola(string x,
                     int32 d,
                     int32 z,
                     int32 w,
                     int32 test,
                     class [mscorlib/*23000001*/]System.Collections.Generic.IList`1/*01000015*/<string> list) cil managed
  // SIG: 00 06 01 0E 08 08 08 08 15 12 55 01 0E
  {
    // Method begins at RVA 0x2134
    // Code size       117 (0x75)
    .maxstack  2
    .locals /*11000003*/ init (int32 V_0,
             class [mscorlib/*23000001*/]System.FormatException/*01000016*/ V_1,
             class [mscorlib/*23000001*/]System.BadImageFormatException/*01000017*/ V_2)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 1F   | 14               */ ldc.i4.s   20
    IL_0003:  /* 0A   |                  */ stloc.0
    .try
    {
      .try
      {
        IL_0004:  /* 00   |                  */ nop
        .try
        {
          IL_0005:  /* 00   |                  */ nop
          IL_0006:  /* 1F   | 0A               */ ldc.i4.s   10
          IL_0008:  /* 0A   |                  */ stloc.0
          IL_0009:  /* 00   |                  */ nop
          IL_000a:  /* DE   | 03               */ leave.s    IL_000f

        }  // end .try
        finally
        {
          IL_000c:  /* 00   |                  */ nop
          IL_000d:  /* 00   |                  */ nop
          IL_000e:  /* DC   |                  */ endfinally
        }  // end handler
        // HEX: 02 00 05 00 07 0C 00 03 00 00 00 00
        IL_000f:  /* 72   | (70)000027       */ ldstr      "1" /* 70000027 */
        IL_0014:  /* 73   | (0A)000019       */ newobj     instance void [mscorlib/*23000001*/]System.ApplicationException/*01000023*/::.ctor(string) /* 0A000019 */
        IL_0019:  /* 7A   |                  */ throw

      }  // end .try
      filter
      {
        IL_001a:  /* 75   | (01)000016       */ isinst     [mscorlib/*23000001*/]System.FormatException/*01000016*/
        IL_001f:  /* 25   |                  */ dup
        IL_0020:  /* 2D   | 04               */ brtrue.s   IL_0026

        IL_0022:  /* 26   |                  */ pop
        IL_0023:  /* 16   |                  */ ldc.i4.0
        IL_0024:  /* 2B   | 14               */ br.s       IL_003a

        IL_0026:  /* 0B   |                  */ stloc.1
        IL_0027:  /* 07   |                  */ ldloc.1
        IL_0028:  /* 6F   | (0A)00001A       */ callvirt   instance string [mscorlib/*23000001*/]System.Exception/*01000024*/::get_Message() /* 0A00001A */
        IL_002d:  /* 72   | (70)000027       */ ldstr      "1" /* 70000027 */
        IL_0032:  /* 28   | (0A)00001B       */ call       bool [mscorlib/*23000001*/]System.String/*01000025*/::op_Equality(string,
                                                                                                                             string) /* 0A00001B */
        IL_0037:  /* 16   |                  */ ldc.i4.0
        IL_0038:  /* FE03 |                  */ cgt.un
        IL_003a:  /* FE11 |                  */ endfilter
      }  // end filter
      {  // handler
        IL_003c:  /* 26   |                  */ pop
        IL_003d:  /* 00   |                  */ nop
        IL_003e:  /* 00   |                  */ nop
        IL_003f:  /* DE   | 05               */ leave.s    IL_0046

      }  // end handler
      // HEX: 01 00 04 00 16 3C 00 05 1A 00 00 00
      catch [mscorlib/*23000001*/]System.BadImageFormatException/*01000017*/ 
      {
        IL_0041:  /* 0C   |                  */ stloc.2
        IL_0042:  /* 00   |                  */ nop
        IL_0043:  /* 00   |                  */ nop
        IL_0044:  /* DE   | 00               */ leave.s    IL_0046

      }  // end handler
      // HEX: 00 00 04 00 16 41 00 05 17 00 00 01
      IL_0046:  /* DE   | 03               */ leave.s    IL_004b

    }  // end .try
    finally
    {
      IL_0048:  /* 00   |                  */ nop
      IL_0049:  /* 00   |                  */ nop
      IL_004a:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 04 00 44 48 00 03 00 00 00 00
    IL_004b:  /* 02   |                  */ ldarg.0
    IL_004c:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_0051:  /* 00   |                  */ nop
    IL_0052:  /* 12   | 00               */ ldloca.s   V_0
    IL_0054:  /* 28   | (0A)000016       */ call       instance string [mscorlib/*23000001*/]System.Int32/*01000020*/::ToString() /* 0A000016 */
    IL_0059:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_005e:  /* 00   |                  */ nop
    IL_005f:  /* 0E   | 04               */ ldarg.s    test
    IL_0061:  /* 28   | (0A)000012       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(int32) /* 0A000012 */
    IL_0066:  /* 00   |                  */ nop
    IL_0067:  /* 0E   | 05               */ ldarg.s    list
    IL_0069:  /* 6F   | (0A)00001C       */ callvirt   instance int32 class [mscorlib/*23000001*/]System.Collections.Generic.ICollection`1/*01000026*/<string>/*1B000002*/::get_Count() /* 0A00001C */
    IL_006e:  /* 28   | (0A)000012       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(int32) /* 0A000012 */
    IL_0073:  /* 00   |                  */ nop
    IL_0074:  /* 2A   |                  */ ret
  } // end of method Program::Hola

  .method /*06000005*/ public hidebysig static 
          int32  Switch(int32 y) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x21ec
    // Code size       100 (0x64)
    .maxstack  2
    .locals /*11000004*/ init (int32 V_0,
             int32 V_1)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 0A   |                  */ stloc.0
    IL_0003:  /* 06   |                  */ ldloc.0
    IL_0004:  /* 17   |                  */ ldc.i4.1
    IL_0005:  /* 59   |                  */ sub
    IL_0006:  /* 45   | 04000000         */ switch     ( 
              /*      | 02000000         */             IL_001d,
              /*      | 0F000000         */             IL_002a,
              /*      | 1C000000         */             IL_0037,
              /*      | 29000000         */             IL_0044)
    IL_001b:  /* 2B   | 34               */ br.s       IL_0051

    IL_001d:  /* 72   | (70)00002B       */ ldstr      "Fail" /* 7000002B */
    IL_0022:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_0027:  /* 00   |                  */ nop
    IL_0028:  /* 2B   | 29               */ br.s       IL_0053

    IL_002a:  /* 72   | (70)000035       */ ldstr      "Pass" /* 70000035 */
    IL_002f:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_0034:  /* 00   |                  */ nop
    IL_0035:  /* 2B   | 1C               */ br.s       IL_0053

    IL_0037:  /* 72   | (70)00003F       */ ldstr      "Good" /* 7000003F */
    IL_003c:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_0041:  /* 00   |                  */ nop
    IL_0042:  /* 2B   | 0F               */ br.s       IL_0053

    IL_0044:  /* 72   | (70)000049       */ ldstr      "Excelent" /* 70000049 */
    IL_0049:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_004e:  /* 00   |                  */ nop
    IL_004f:  /* 2B   | 02               */ br.s       IL_0053

    IL_0051:  /* 2B   | 00               */ br.s       IL_0053

    IL_0053:  /* 72   | (70)00005B       */ ldstr      "End of switch" /* 7000005B */
    IL_0058:  /* 28   | (0A)000013       */ call       void [mscorlib/*23000001*/]System.Console/*0100001D*/::WriteLine(string) /* 0A000013 */
    IL_005d:  /* 00   |                  */ nop
    IL_005e:  /* 02   |                  */ ldarg.0
    IL_005f:  /* 0B   |                  */ stloc.1
    IL_0060:  /* 2B   | 00               */ br.s       IL_0062

    IL_0062:  /* 07   |                  */ ldloc.1
    IL_0063:  /* 2A   |                  */ ret
  } // end of method Program::Switch

  .method /*06000006*/ public hidebysig static 
          void  LoadToken() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x225c
    // Code size       81 (0x51)
    .maxstack  6
    .locals /*11000005*/ init (class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*01000018*/<class [mscorlib/*23000001*/]System.Func`2/*01000019*/<int32,int32>> V_0,
             class [mscorlib/*23000001*/]System.Type/*0100001A*/ V_1,
             class [System.Core/*23000002*/]System.Linq.Expressions.ParameterExpression/*0100001B*/ V_2)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* D0   | (01)000020       */ ldtoken    [mscorlib/*23000001*/]System.Int32/*01000020*/
    IL_0006:  /* 28   | (0A)00001D       */ call       class [mscorlib/*23000001*/]System.Type/*0100001A*/ [mscorlib/*23000001*/]System.Type/*0100001A*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000027*/) /* 0A00001D */
    IL_000b:  /* 72   | (70)000077       */ ldstr      "x" /* 70000077 */
    IL_0010:  /* 28   | (0A)00001E       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.ParameterExpression/*0100001B*/ [System.Core/*23000002*/]System.Linq.Expressions.Expression/*01000028*/::Parameter(class [mscorlib/*23000001*/]System.Type/*0100001A*/,
                                                                                                                                                                                                                                 string) /* 0A00001E */
    IL_0015:  /* 0C   |                  */ stloc.2
    IL_0016:  /* 14   |                  */ ldnull
    IL_0017:  /* D0   | (06)000005       */ ldtoken    method int32 Demo1.Program/*02000002*/::Switch(int32) /* 06000005 */
    IL_001c:  /* 28   | (0A)00001F       */ call       class [mscorlib/*23000001*/]System.Reflection.MethodBase/*01000029*/ [mscorlib/*23000001*/]System.Reflection.MethodBase/*01000029*/::GetMethodFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeMethodHandle/*0100002A*/) /* 0A00001F */
    IL_0021:  /* 74   | (01)00002B       */ castclass  [mscorlib/*23000001*/]System.Reflection.MethodInfo/*0100002B*/
    IL_0026:  /* 17   |                  */ ldc.i4.1
    IL_0027:  /* 8D   | (01)000028       */ newarr     [System.Core/*23000002*/]System.Linq.Expressions.Expression/*01000028*/
    IL_002c:  /* 25   |                  */ dup
    IL_002d:  /* 16   |                  */ ldc.i4.0
    IL_002e:  /* 08   |                  */ ldloc.2
    IL_002f:  /* A2   |                  */ stelem.ref
    IL_0030:  /* 28   | (0A)000020       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100002C*/ [System.Core/*23000002*/]System.Linq.Expressions.Expression/*01000028*/::Call(class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*01000028*/,
                                                                                                                                                                                                                             class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*0100002B*/,
                                                                                                                                                                                                                             class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*01000028*/[]) /* 0A000020 */
    IL_0035:  /* 17   |                  */ ldc.i4.1
    IL_0036:  /* 8D   | (01)00001B       */ newarr     [System.Core/*23000002*/]System.Linq.Expressions.ParameterExpression/*0100001B*/
    IL_003b:  /* 25   |                  */ dup
    IL_003c:  /* 16   |                  */ ldc.i4.0
    IL_003d:  /* 08   |                  */ ldloc.2
    IL_003e:  /* A2   |                  */ stelem.ref
    IL_003f:  /* 28   | (2B)000001       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*01000018*/<!!0> [System.Core/*23000002*/]System.Linq.Expressions.Expression/*01000028*/::Lambda<class [mscorlib/*23000001*/]System.Func`2/*01000019*/<int32,int32>>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*01000028*/,
                                                                                                                                                                                                                                                                                                class [System.Core/*23000002*/]System.Linq.Expressions.ParameterExpression/*0100001B*/[]) /* 2B000001 */
    IL_0044:  /* 0A   |                  */ stloc.0
    IL_0045:  /* D0   | (01)00002D       */ ldtoken    [mscorlib/*23000001*/]System.Reflection.Emit.OpCodes/*0100002D*/
    IL_004a:  /* 28   | (0A)00001D       */ call       class [mscorlib/*23000001*/]System.Type/*0100001A*/ [mscorlib/*23000001*/]System.Type/*0100001A*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000027*/) /* 0A00001D */
    IL_004f:  /* 0B   |                  */ stloc.1
    IL_0050:  /* 2A   |                  */ ret
  } // end of method Program::LoadToken

  .method /*06000007*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22bc
    // Code size       25 (0x19)
    .maxstack  1
    .locals /*11000006*/ init (bool V_0,
             string V_1)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (0A)000022       */ call       instance class [mscorlib/*23000001*/]System.Type/*0100001A*/ [mscorlib/*23000001*/]System.Object/*01000010*/::GetType() /* 0A000022 */
    IL_0007:  /* 6F   | (0A)000023       */ callvirt   instance bool [mscorlib/*23000001*/]System.Type/*0100001A*/::get_IsAbstract() /* 0A000023 */
    IL_000c:  /* 0A   |                  */ stloc.0
    IL_000d:  /* 12   | 00               */ ldloca.s   V_0
    IL_000f:  /* 28   | (0A)000024       */ call       instance string [mscorlib/*23000001*/]System.Boolean/*0100002E*/::ToString() /* 0A000024 */
    IL_0014:  /* 0B   |                  */ stloc.1
    IL_0015:  /* 2B   | 00               */ br.s       IL_0017

    IL_0017:  /* 07   |                  */ ldloc.1
    IL_0018:  /* 2A   |                  */ ret
  } // end of method Program::ToString

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22e1
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000010*/::.ctor() /* 0A000025 */
    IL_0006:  /* 00   |                  */ nop
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Program::.ctor

  .method /*06000009*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x22ea
    // Code size       43 (0x2b)
    .maxstack  8
    IL_0000:  /* 72   | (70)00007B       */ ldstr      "Hello" /* 7000007B */
    IL_0005:  /* 80   | (04)000001       */ stsfld     string Demo1.Program/*02000002*/::str /* 04000001 */
    IL_000a:  /* 7F   | (04)000002       */ ldsflda    valuetype [mscorlib/*23000001*/]System.DateTime/*01000011*/ Demo1.Program/*02000002*/::dt /* 04000002 */
    IL_000f:  /* FE15 | (01)000011       */ initobj    [mscorlib/*23000001*/]System.DateTime/*01000011*/
    IL_0015:  /* 7F   | (04)000003       */ ldsflda    valuetype Demo1.StructTest/*02000003*/ Demo1.Program/*02000002*/::st /* 04000003 */
    IL_001a:  /* FE15 | (02)000003       */ initobj    Demo1.StructTest/*02000003*/
    IL_0020:  /* 72   | (70)000087       */ ldstr      "Default" /* 70000087 */
    IL_0025:  /* 80   | (04)000004       */ stsfld     string Demo1.Program/*02000002*/::'<Str>k__BackingField' /* 04000004 */
    IL_002a:  /* 2A   |                  */ ret
  } // end of method Program::.cctor

  .property /*17000001*/ string Str()
  {
    .get string Demo1.Program/*02000002*/::get_Str() /* 06000001 */
    .set void Demo1.Program/*02000002*/::set_Str(string) /* 06000002 */
  } // end of property Program::Str
} // end of class Demo1.Program

.class /*02000003*/ private sequential ansi sealed beforefieldinit Demo1.StructTest
       extends [mscorlib/*23000001*/]System.ValueType/*0100001C*/
{
  .pack 0
  .size 1
  .method /*0600000A*/ public hidebysig specialname 
          instance string  get_foo() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2318
    // Code size       11 (0xb)
    .maxstack  1
    .locals /*11000001*/ init (string V_0)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 7E   | (0A)000026       */ ldsfld     string [mscorlib/*23000001*/]System.String/*01000025*/::Empty /* 0A000026 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009

    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method StructTest::get_foo

  .property /*17000002*/ instance string foo()
  {
    .get instance string Demo1.StructTest/*02000003*/::get_foo() /* 0600000A */
  } // end of property StructTest::foo
} // end of class Demo1.StructTest


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file C:\Users\santi\Documents\GitHub\corefx\src\ILDasmLibrary\ILDasmLibraryTest\Output\foo2.res

using ILDasmLibrary.Decoder;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection.Metadata;

public enum HandlerKind
{
    Try,
    Catch,
    Finally,
    Filter,
    Fault
}

internal struct ILDasmExceptionRegion
{
    public readonly HandlerKind Kind;
    public readonly EntityHandle CatchType;
    public readonly int StartOffset;
    public readonly int FilterHandlerStart;
    public readonly int EndOffset;

    internal ILDasmExceptionRegion(HandlerKind kind, EntityHandle catchType, int startOffset, int filterHandlerStart, int endOffset)
    {
        Kind = kind;
        CatchType = catchType;
        StartOffset = startOffset;
        FilterHandlerStart = filterHandlerStart;
        EndOffset = endOffset;
    }

    public int CompareTo(ILDasmExceptionRegion span2)
    {
        int offset1 = StartOffset;
        int offset2 = span2.StartOffset;
        if (offset1 == offset2)
        {
            return span2.EndOffset - EndOffset;
        }
        return offset1 - offset2;
    }

    public string ToString(ILDasmTypeProvider provider)
    {
        switch (Kind)
        {
            case HandlerKind.Try:
                return ".try";
            case HandlerKind.Finally:
                return "finally";
            case HandlerKind.Filter:
                return "filter";
            case HandlerKind.Fault:
                return "fault";
            case HandlerKind.Catch:
                return string.Format("catch {0}", ILDasmDecoder.DecodeType(CatchType, provider));
            default:
                throw new InvalidOperationException("Handler Kind doesn't exist.");
        }
    }

    public static IReadOnlyList<ILDasmExceptionRegion> CreateRegions(ImmutableArray<ExceptionRegion> exceptionRegions)
    {
        if (exceptionRegions.Length == 0)
        {
            return new ILDasmExceptionRegion[0];
        }
        var spans = new List<ILDasmExceptionRegion>();
        foreach (ExceptionRegion region in exceptionRegions)
        {
            var startOffset = region.TryOffset;
            var endOffset = region.TryOffset + region.TryLength;
            var span = new ILDasmExceptionRegion(HandlerKind.Try, region.CatchType, startOffset, -1, endOffset);
            if (spans.Count == 0 || spans[spans.Count - 1].CompareTo(span) != 0)
            {
                spans.Add(span);
                continue;
            }
        }
        foreach (ExceptionRegion region in exceptionRegions)
        {
            var startOffset = region.HandlerOffset;
            var endOffset = region.HandlerOffset + region.HandlerLength;
            switch (region.Kind)
            {
                case ExceptionRegionKind.Catch:
                    spans.Add(new ILDasmExceptionRegion(HandlerKind.Catch, region.CatchType, startOffset, -1, endOffset));
                    break;
                case ExceptionRegionKind.Fault:
                    spans.Add(new ILDasmExceptionRegion(HandlerKind.Fault, region.CatchType, startOffset, -1, endOffset));
                    break;
                case ExceptionRegionKind.Filter:
                    spans.Add(new ILDasmExceptionRegion(HandlerKind.Filter, region.CatchType, startOffset, region.FilterOffset, endOffset));
                    break;
                case ExceptionRegionKind.Finally:
                    spans.Add(new ILDasmExceptionRegion(HandlerKind.Finally, region.CatchType, startOffset, -1, endOffset));
                    break;
            }
        }
        spans.Sort((ILDasmExceptionRegion region1, ILDasmExceptionRegion region2) => { return region1.CompareTo(region2); });
        return spans;
    }
}
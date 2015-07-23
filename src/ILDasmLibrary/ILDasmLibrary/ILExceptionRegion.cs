﻿using ILDasmLibrary.Decoder;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection.Metadata;

namespace ILDasmLibrary
{
    public enum HandlerKind
    {
        Try,
        Catch,
        Finally,
        Filter,
        Fault
    }

    public struct ILExceptionRegion
    {
        public readonly HandlerKind Kind;
        public readonly EntityHandle CatchType;
        public readonly int StartOffset;
        public readonly int FilterHandlerStart;
        public readonly int EndOffset;

        internal ILExceptionRegion(HandlerKind kind, EntityHandle catchType, int startOffset, int filterHandlerStart, int endOffset)
        {
            Kind = kind;
            CatchType = catchType;
            StartOffset = startOffset;
            FilterHandlerStart = filterHandlerStart;
            EndOffset = endOffset;
        }

        /// <summary>
        /// This method is used to sort regions from outter to inner (smaller offsets first) so that the regions are dumped
        /// with the desired format on msil in case we've got nested regions.
        /// </summary>
        /// <param name="span2">Region to compare to</param>
        /// <returns>This method returns -1 if the first is smaller, 0 if they are equal, 1 if the first is greater.</returns>
        public int CompareTo(ILExceptionRegion span2)
        {
            int offset1 = StartOffset;
            int offset2 = span2.StartOffset;
            if (offset1 == offset2)
            {
                return span2.EndOffset - EndOffset;
            }
            return offset1 - offset2;
        }

        public string ToString(ILTypeProvider provider)
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
                    return string.Format("catch {0}", ILDecoder.DecodeType(CatchType, provider).ToString(false));
                default:
                    throw new InvalidOperationException("Handler Kind doesn't exist.");
            }
        }

        public static IReadOnlyList<ILExceptionRegion> CreateRegions(ImmutableArray<ExceptionRegion> exceptionRegions)
        {
            if (exceptionRegions.Length == 0)
            {
                return new ILExceptionRegion[0];
            }
            var spans = new List<ILExceptionRegion>();
            foreach (ExceptionRegion region in exceptionRegions)
            {
                var startOffset = region.TryOffset;
                var endOffset = region.TryOffset + region.TryLength;
                var span = new ILExceptionRegion(HandlerKind.Try, region.CatchType, startOffset, -1, endOffset);
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
                        spans.Add(new ILExceptionRegion(HandlerKind.Catch, region.CatchType, startOffset, -1, endOffset));
                        break;
                    case ExceptionRegionKind.Fault:
                        spans.Add(new ILExceptionRegion(HandlerKind.Fault, region.CatchType, startOffset, -1, endOffset));
                        break;
                    case ExceptionRegionKind.Filter:
                        spans.Add(new ILExceptionRegion(HandlerKind.Filter, region.CatchType, startOffset, region.FilterOffset, endOffset));
                        break;
                    case ExceptionRegionKind.Finally:
                        spans.Add(new ILExceptionRegion(HandlerKind.Finally, region.CatchType, startOffset, -1, endOffset));
                        break;
                }
            }
            spans.Sort((ILExceptionRegion region1, ILExceptionRegion region2) => { return region1.CompareTo(region2); });
            return spans;
        }
    }
}

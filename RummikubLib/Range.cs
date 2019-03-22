﻿using System;

namespace RummikubLib
{
    public struct Range
    {
        public Range(double min, double max)
        {
            if (double.IsNaN(min))
            {
                throw new ArgumentOutOfRangeException(nameof(min));
            }

            if (double.IsNaN(max))
            {
                throw new ArgumentOutOfRangeException(nameof(max));
            }

            if (min > max)
            {
                throw new ArgumentException("Minimum value must not be larger than maximum value.");
            }

            Min = min;
            Max = max;
        }

        public double Min { get; }

        public double Max { get; }

        public static Range operator *(Range range, double multiplier)
        {
            if (double.IsNaN(multiplier))
            {
                throw new ArgumentOutOfRangeException(nameof(multiplier));
            }

            return multiplier >= 0
                ? new Range(range.Min * multiplier, range.Max * multiplier)
                : new Range(range.Max * multiplier, range.Min * multiplier);
        }

        public static Range operator /(Range range, double divisor)
        {
            if (double.IsNaN(divisor))
            {
                throw new ArgumentOutOfRangeException(nameof(divisor));
            }

            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }

            return divisor > 0
                ? new Range(range.Min / divisor, range.Max / divisor)
                : new Range(range.Max / divisor, range.Min / divisor);
        }
    }
}
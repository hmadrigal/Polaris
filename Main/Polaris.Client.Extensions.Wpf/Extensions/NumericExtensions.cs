//-----------------------------------------------------------------------
// <copyright file="NumericExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Extensions
{
    using System;

    public static class NumericExtensions
    {
        /// <summary>
        /// Limits the value to the specified range as long as the type is IComparable.
        /// </summary>
        /// <typeparam name="T">An IComparable type.</typeparam>
        /// <param name="target"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T Clamp<T>(this T target, T min, T max) where T : IComparable
        {
            return (target.CompareTo(max) > 0 ? max : (target.CompareTo(min) < 0 ? min : target));
        }

        public static Double FloorRound(this Double target, Int32 decimals)
        {
            Double floor = Math.Floor(target);
            Double decimalPart = target - floor;
            Double multiplier = Math.Pow(10, decimals);
            Double roundPart = Math.Floor(decimalPart * multiplier) / (Double)multiplier;
            Double value = floor + roundPart;
            return value;
        }

    }
}

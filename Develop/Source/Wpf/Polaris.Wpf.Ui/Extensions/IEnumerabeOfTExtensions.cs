﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Windows.Extensions
{
    public static class IEnumerabeOfTExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
                action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            collection.Select((item, index) => { action(item, index); return index; }).LastOrDefault();
        }

        public static IEnumerable<T>  Last<T>(this IEnumerable<T> collection, int count)
        {
            return collection.Skip(Math.Max(0, collection.Count() - count)).Take(count);
        }
    }
}

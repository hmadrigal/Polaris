//-----------------------------------------------------------------------
// <copyright file="DependencyObjectExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Creates a new instance of the same type and copies all properties and values.
        /// </summary>
        /// <typeparam name="T">A type that extends DependencyObject</typeparam>
        /// <param name="source"></param>
        /// <returns>A new instance that represents a copy of the properties and values of the original instance.</returns>
        public static T Clone<T>(this T source) where T : DependencyObject
        {
            Type t = source.GetType();
            T no = (T)Activator.CreateInstance(t);

            Type wt = t;
            while (wt.BaseType != typeof(DependencyObject))
            {
                FieldInfo[] fi = wt.GetFields(BindingFlags.Static | BindingFlags.Public);
                for (int i = 0; i < fi.Length; i++)
                {
                    {
                        DependencyProperty dp = fi[i].GetValue(source) as DependencyProperty;
                        if (dp != null && fi[i].Name != "NameProperty")
                        {
                            DependencyObject obj = source.GetValue(dp) as DependencyObject;
                            if (obj != null)
                            {
                                object o = obj.Clone();
                                no.SetValue(dp, o);
                            }
                            else
                            {
                                if (fi[i].Name != "CountProperty" &&
                                    fi[i].Name != "GeometryTransformProperty" &&
                                    fi[i].Name != "ActualWidthProperty" &&
                                    fi[i].Name != "ActualHeightProperty" &&
                                    fi[i].Name != "MaxWidthProperty" &&
                                    fi[i].Name != "MaxHeightProperty" &&
                                    fi[i].Name != "StyleProperty")
                                {
                                    no.SetValue(dp, source.GetValue(dp));
                                }
                            }
                        }
                    }
                }
                wt = wt.BaseType;
            }

            PropertyInfo[] pis = t.GetProperties();
            for (int i = 0; i < pis.Length; i++)
            {
                if (
                    pis[i].Name != "Name" &&
                    pis[i].Name != "Parent" &&
                    pis[i].Name != "TargetType" &&
                    pis[i].CanRead && pis[i].CanWrite &&
                    !pis[i].PropertyType.IsArray &&
                    !pis[i].PropertyType.IsSubclassOf(typeof(DependencyObject)) &&
                    pis[i].GetIndexParameters().Length == 0 &&
                    pis[i].GetValue(source, null) != null &&
                    pis[i].GetValue(source, null) != (object)default(int) &&
                    pis[i].GetValue(source, null) != (object)default(double) &&
                    pis[i].GetValue(source, null) != (object)default(float)
                    )
                    pis[i].SetValue(no, pis[i].GetValue(source, null), null);
                else if (pis[i].PropertyType.GetInterface("IList", true) != null)
                {
                    int cnt = (int)pis[i].PropertyType.InvokeMember("get_Count", BindingFlags.InvokeMethod, null, pis[i].GetValue(source, null), null);
                    for (int c = 0; c < cnt; c++)
                    {
                        object val = pis[i].PropertyType.InvokeMember("get_Item", BindingFlags.InvokeMethod, null, pis[i].GetValue(source, null), new object[] { c });

                        object nVal = val;
                        DependencyObject v = val as DependencyObject;
                        if (v != null)
                            nVal = v.Clone();
                        if (pis[i].GetValue(no, null) == null)
                        {
                            object obj = Activator.CreateInstance(pis[i].PropertyType);
                            pis[i].SetValue(no, obj, null);
                        }
                        pis[i].PropertyType.InvokeMember("Add", BindingFlags.InvokeMethod, null, pis[i].GetValue(no, null), new object[] { nVal });
                    }
                }
            }

            return no;
        }

        /// <summary>
        /// Finds the first child in the visual tree matching the specified type.
        /// </summary>
        /// <typeparam name="T">A type that extends DependencyObject</typeparam>
        /// <param name="parent"></param>
        /// <returns>The first child found in the visual tree matching the specified type. Null if the child was not found.</returns>
        public static T FindVisualChild<T>(this DependencyObject parent, string childName = null) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var childElement = child as FrameworkElement;
                if (child is T && (childName == null || (childElement != null && childElement.Name == childName)))
                {
                    return child as T;
                }
                else
                {
                    var grandchild = child.FindVisualChild<T>(childName);
                    if (grandchild is T)
                    {
                        return grandchild;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the first parent in the visual tree matching the specified type.
        /// </summary>
        /// <typeparam name="T">A type that extends DependencyObject</typeparam>
        /// <param name="child"></param>
        /// <returns>The first parent found in the visual tree matching the specified type. Null if the type was not found.</returns>
        public static T FindVisualParent<T>(this DependencyObject child, string parentName = null) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent == null)
            {
                return null;
            }
            else
            {
                var parentElement = parent as FrameworkElement;
                if (parent is T && (parentName == null || (parentElement != null && parentElement.Name == parentName)))
                {
                    return parent as T;
                }
                else
                {
                    return parent.FindVisualParent<T>(parentName);
                }
            }
        }

        /// <summary>
        /// Finds the first child in the visual tree matching the specified type.
        /// </summary>
        /// <typeparam name="T">A type that extends DependencyObject</typeparam>
        /// <param name="parent"></param>
        /// <returns>The first child found in the visual tree matching the specified type. Null if the child was not found.</returns>
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject parent) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    yield return child as T;
                }

                var grandchildren = child.FindVisualChildren<T>();
                foreach (var grandchild in grandchildren)
                {
                    yield return grandchild;
                }
            }
        }

        public static Panel FindPanel(this DependencyObject element)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (Int32 i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is Panel)
                {
                    return child as Panel;
                }
                else
                {
                    child = FindPanel(child);
                    if (child is Panel)
                    {
                        return child as Panel;
                    }
                }
            }
            return null;
        }
    }
}
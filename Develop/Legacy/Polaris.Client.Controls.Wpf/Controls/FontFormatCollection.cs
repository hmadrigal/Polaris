//-----------------------------------------------------------------------
// <copyright file="FontFormatCollection.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;

    public abstract class FontFormatCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
        where T : FontFormat
    {
        protected List<T> Items { get; private set; }
        internal FormattedTextBlock formattedTextBlockRef;

        public FontFormatCollection()
        {
            Items = new List<T>();
            formattedTextBlockRef = null;
        }

        #region IList<FormatDefinition>
        public int IndexOf(T item)
        {
            return Items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            item.PrepareTypeface(formattedTextBlockRef);
            Items.Insert(index, item);
            TryInvalidateDisplay();
        }

        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
            TryInvalidateDisplay();
        }

        public T this[int index]
        {
            get
            {
                return Items[index];
            }
            set
            {
                Items[index] = value;
            }
        }

        public void Add(T item)
        {
            item.PrepareTypeface(formattedTextBlockRef);
            Items.Add(item);
            TryInvalidateDisplay();
        }

        public void Clear()
        {
            Items.Clear();
            TryInvalidateDisplay();
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            var removedItem = Items.Remove(item);
            TryInvalidateDisplay();
            return removedItem;

        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (Items as IEnumerable).GetEnumerator();
        }
        #endregion

        #region IList
        public int Add(object value)
        {
            if (value is T)
                (value as T).PrepareTypeface(formattedTextBlockRef);
            var newIndex = (Items as IList).Add(value);
            TryInvalidateDisplay();
            return newIndex;
        }

        public bool Contains(object value)
        {
            return (Items as IList).Contains(value);
        }

        public int IndexOf(object value)
        {
            return (Items as IList).IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            if (value is T)
                (value as T).PrepareTypeface(formattedTextBlockRef);
            (Items as IList).Insert(index, value);
            TryInvalidateDisplay();
        }

        public bool IsFixedSize
        {
            get { return (Items as IList).IsFixedSize; }
        }

        public void Remove(object value)
        {
            (Items as IList).Remove(value);
            TryInvalidateDisplay();
        }

        object IList.this[int index]
        {
            get
            {
                return (Items as IList)[index];
            }
            set
            {
                (Items as IList)[index] = value;
            }
        }

        public void CopyTo(Array array, int index)
        {
            (Items as IList).CopyTo(array, index);
        }

        public bool IsSynchronized
        {
            get { return (Items as IList).IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return (Items as IList).SyncRoot; }
        }
        #endregion

        private void TryInvalidateDisplay()
        {
            if (formattedTextBlockRef != null)
            {
                formattedTextBlockRef.InvalidateVisual();
                formattedTextBlockRef.InvalidateMeasure();
            }
        }
    }

    public class TaggedFontFormatCollection : FontFormatCollection<TaggedFontFormat> { }
    public class CountFontFormatCollection : FontFormatCollection<CountFontFormat> { }
}

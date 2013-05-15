using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Collections;

#if NETFX_CORE
using Windows.UI.Xaml;
#endif

namespace Polaris.PhoneLib.Mvvm.Collections
{
    public class ObservableStyleDictionary : ObservableSortedDictionary<string, Style>
    {
        #region constructors

        #region public

        public ObservableStyleDictionary()
            : base(new KeyComparer())
        {
        }

        #endregion public

        #endregion constructors

        #region key comparer class

        private class KeyComparer : IComparer<DictionaryEntry>
        {
#if NETFX_CORE
            public int Compare(DictionaryEntry entry1, DictionaryEntry entry2)
            {
                return string.Compare((string)entry1.Key, (string)entry2.Key, StringComparison.CurrentCultureIgnoreCase);
            }
#endif
#if WINDOWS_PHONE
            public int Compare(DictionaryEntry entry1, DictionaryEntry entry2)
            {
                return string.Compare((string)entry1.Key, (string)entry2.Key, StringComparison.InvariantCultureIgnoreCase);
            } 
#endif
        }

        #endregion key comparer class

    }
}

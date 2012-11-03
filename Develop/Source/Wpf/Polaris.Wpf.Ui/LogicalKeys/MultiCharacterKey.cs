using System;
using System.Collections.Generic;
using WindowsInput;
using System.Windows;
namespace Polaris.Windows.Controls
{
    public class MultiCharacterKey : VirtualKey
    {
        private int _selectedIndex;

        public StringList KeyDisplays
        {
            get { return _keyDisplays ?? (_keyDisplays = new StringList()); }
            set {
                if (_keyDisplays == value || value == null || _keyDisplays.Count  == 0)
                    return;
                _keyDisplays = value;
                SelectedKeyDisplay = _keyDisplays[0];
            }
        }
        private StringList _keyDisplays;

        public object SelectedKeyDisplay { get; set; }

        public int SelectedIndex { get; set; }

    }
}
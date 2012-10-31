using System;
using System.Collections.Generic;
using WindowsInput;
namespace Polaris.Windows.Controls
{
    public class MultiCharacterKey : VirtualKey
    {
        private int _selectedIndex;
        //public IList<string> KeyDisplays { get; set; }


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

        public string SelectedKeyDisplay { get; set; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value != _selectedIndex)
                {
                    _selectedIndex = value;
                    SelectedKeyDisplay = KeyDisplays[value];
                    DisplayName = SelectedKeyDisplay;
                    OnPropertyChanged("SelectedIndex");
                    OnPropertyChanged("SelectedKeyDisplay");
                }
            }
        }

        //public MultiCharacterKey(VirtualKeyCode keyCode, IList<string> keyDisplays) :
        //    base(keyCode)
        //{
        //    if (keyDisplays == null) throw new ArgumentNullException("keyDisplays");
        //    if (keyDisplays.Count <= 0)
        //        throw new ArgumentException("Please provide a list of one or more keyDisplays", "keyDisplays");
        //    KeyDisplays = keyDisplays;
        //    DisplayName = keyDisplays[0];
        //}
    }
}
using System;
using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace Polaris.PhoneLib.Mvvm
{
    public class ResourceViewModel : ViewModelBase
    {
        public System.Resources.ResourceManager ResourceManager
        {
            get { return _resourceManager; }
            set { _resourceManager = value; }
        }
        private System.Resources.ResourceManager _resourceManager;
        
        public ResourceViewModel(System.Resources.ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public string this[string key]
        {
            get
            {
                var resourceValue = _resourceManager.GetString(key.Replace('.', '_'));
                return resourceValue ?? null;
            }
        }
    }
}

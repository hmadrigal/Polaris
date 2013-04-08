﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Polaris.PhoneLib.Toolkit.Services
{
    public interface IRouteManager
    {
        void Register(string name, string assembly, string path, params string[] parameters);
        void Register<TViewModel>(string assembly, string path, params string[] parameters);
        Uri Resolve(string name, params object[] args);
        Uri Resolve(string name, KeyValuePair<string, object>[] optionalParameters, params object[] args);
        Uri Resolve<TViewModel>(params object[] args);
        Uri Resolve<TViewModel>(KeyValuePair<string, object>[] optionalParameters, params object[] requiredParameters);
    }
}

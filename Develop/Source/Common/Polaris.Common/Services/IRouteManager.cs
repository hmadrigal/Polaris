using System;
using System.Collections.Generic;
using System.Text;

namespace Polaris.Services
{
    public interface IRouteManager
    {
        void Register(string name, string path, params string[] parameters);
        void Register(string name, string assembly, string path, params string[] parameters);
        void Register<TViewModel>(string assembly, string path, params string[] parameters);
        Uri Resolve(string name, params object[] args);
        Uri Resolve(string name, IEnumerable<KeyValuePair<string, object>> optionalParameters, params object[] args);
        Uri Resolve<TViewModel>(params object[] args);
        Uri Resolve<TViewModel>(IEnumerable<KeyValuePair<string, object>> optionalParameters, params object[] requiredParameters);
    }
}

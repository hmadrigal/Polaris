using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Polaris.PhoneLib.Services
{
    public class RouteManager : IRouteManager
    {
        private readonly IDictionary<string, string> _labeledRoutes;

        public RouteManager()
        {
            _labeledRoutes = new Dictionary<string, string>();
        }

        private static string GetUrl(string basePath, string path, bool isAssembly, string[] parameters)
        {
            string concatedParameters = string.Empty;
            if (parameters != null)
            {
                var sb = parameters.Length == 0 ? new StringBuilder() : new StringBuilder("?");
                for (int index = 0; index < parameters.Length; index++)
                    sb.AppendFormat("{0}={{{1}}}&", parameters[index], index);
                if (sb.Length > 1)
                    sb.Remove(sb.Length - 1, 1);
                concatedParameters = sb.ToString();
            }
            var uri = string.Concat(
                isAssembly ? @"/" : string.Empty,
                basePath,
                isAssembly ? @";component/" : string.Empty,
                path,
                concatedParameters
            );
            return uri;
        }

        private static string AppendParameters(string formattedUrl, IEnumerable<KeyValuePair<string, object>> optionalParameters)
        {
            if (optionalParameters != null)
            {
                var sb = formattedUrl.Contains("?") ? new StringBuilder("&") : new StringBuilder("?");
                foreach (var optionalParameter in optionalParameters)
                    sb.AppendFormat("{0}={1}&", optionalParameter.Key, optionalParameter.Value);

                if (sb.Length > 1)
                    sb.Remove(sb.Length - 1, 1);
                sb.Insert(0, formattedUrl);
                formattedUrl = sb.ToString();
            }
            return formattedUrl;
        }

        public void Register(string name, string path, params string[] parameters)
        {
            var url = GetUrl(string.Empty, path, false, parameters);
            _labeledRoutes.Add(name, url);
        }

        public void Register(string name, string assembly, string path, params string[] parameters)
        {
            var url = GetUrl(assembly, path, true, parameters);
            _labeledRoutes.Add(name, url);
        }

        public void Register<TViewModel>(string assembly, string path, params string[] parameters)
        {
            var url = GetUrl(assembly, path, true, parameters);
            _labeledRoutes[typeof(TViewModel).AssemblyQualifiedName] = url;
        }

        public Uri Resolve<TViewModel>(params object[] args)
        {
            return new Uri(string.Format(_labeledRoutes[typeof(TViewModel).AssemblyQualifiedName], args), UriKind.RelativeOrAbsolute);
        }

        public Uri Resolve<TViewModel>(IEnumerable<KeyValuePair<string, object>> optionalParameters, params object[] requiredParameters)
        {
            var formattedUrl = string.Format(_labeledRoutes[typeof(TViewModel).AssemblyQualifiedName], requiredParameters);
            formattedUrl = AppendParameters(formattedUrl, optionalParameters);
            return new Uri(formattedUrl, UriKind.RelativeOrAbsolute);
        }

        public Uri Resolve(string name, params object[] args)
        {
            return new Uri(string.Format(_labeledRoutes[name], args), UriKind.RelativeOrAbsolute);
        }

        public Uri Resolve(string name, IEnumerable<KeyValuePair<string, object>> optionalParameters, params object[] args)
        {
            var formattedUrl = string.Format(_labeledRoutes[name], args);
            formattedUrl = AppendParameters(formattedUrl, optionalParameters);
            return new Uri(formattedUrl, UriKind.RelativeOrAbsolute);
        }
    }
}

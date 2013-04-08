using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Polaris.PhoneLib.Toolkit.Services
{
    public class RouteManager : IRouteManager
    {
        private readonly IDictionary<Type, string> _typedRoutes;
        private readonly IDictionary<string, string> _labeledRoutes;

        public RouteManager()
        {
            _labeledRoutes = new Dictionary<string, string>();
            _typedRoutes = new Dictionary<Type, string>();
        }

        private static string GetUrl(string assembly, string path, string[] parameters)
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

            var uri = string.Format("/{0};component/{1}{2}", assembly, path, concatedParameters);
            return uri;
        }

        private static string AppendParameters(string formattedUrl, params KeyValuePair<string, object>[] optionalParameters)
        {
            if (optionalParameters != null)
            {
                var sb = formattedUrl.Contains("?") ? new StringBuilder("&") : new StringBuilder("?");
                for (int index = 0; index < optionalParameters.Length; index++)
                    sb.AppendFormat("{0}={1}&", optionalParameters[index].Key, optionalParameters[index].Value);
                if (sb.Length > 1)
                    sb.Remove(sb.Length - 1, 1);
                sb.Insert(0, formattedUrl);
                formattedUrl = sb.ToString();
            }
            return formattedUrl;
        }

        public void Register(string name, string assembly, string path, params string[] parameters)
        {
            var url = GetUrl(assembly, path, parameters);
            _labeledRoutes.Add(name, url);
        }

        public void Register<TViewModel>(string assembly, string path, params string[] parameters)
        {
            var url = GetUrl(assembly, path, parameters);
            _typedRoutes[typeof(TViewModel)] = url;
        }

        public Uri Resolve<TViewModel>(params object[] args)
        {
            return new Uri(string.Format(_typedRoutes[typeof(TViewModel)], args), UriKind.Relative);
        }

        public Uri Resolve<TViewModel>(KeyValuePair<string, object>[] optionalParameters, params object[] requiredParameters )
        {
            var formattedUrl = string.Format(_typedRoutes[typeof(TViewModel)], requiredParameters);
            formattedUrl = AppendParameters(formattedUrl, optionalParameters);
            return new Uri(formattedUrl, UriKind.Relative);
        }
        
        public Uri Resolve(string name, params object[] args)
        {
            return new Uri(string.Format(_labeledRoutes[name], args), UriKind.Relative);
        }
        
        public Uri Resolve(string name, KeyValuePair<string, object>[] optionalParameters, params object[] args)
        {
            var formattedUrl = string.Format(_labeledRoutes[name], args);
            formattedUrl = AppendParameters(formattedUrl, optionalParameters);
            return new Uri(formattedUrl, UriKind.Relative);
        }
    }
}

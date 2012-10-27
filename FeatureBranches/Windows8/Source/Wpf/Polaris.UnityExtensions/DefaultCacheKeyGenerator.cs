using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI;

namespace Polaris.UnityExtensions
{
    /// <summary>
    /// The default <see cref="ICacheKeyGenerator"/> used by the <see cref="PessimisticCacheHandler"/>.
    /// </summary>
    public class DefaultCacheKeyGenerator : ICacheKeyGenerator
    {
        private readonly LosFormatter serializer = new LosFormatter(false, "");

        #region ICacheKeyGenerator Members

        /// <summary>
        /// Create a cache key for the given method and set of input arguments.
        /// </summary>
        /// <param name="method">Method being called.</param>
        /// <param name="inputs">Input arguments.</param>
        /// <returns>A (hopefully) unique string to be used as a cache key.</returns>
        public string CreateCacheKey(MethodBase method, params object[] inputs)
        {
            try
            {
                var methodDeclaringType = method.DeclaringType;
                var methodDeclaringTypeFullName = methodDeclaringType != null ? method.DeclaringType.FullName : null;
                var methodName = method.Name;
                return CreateCacheKey(inputs, methodDeclaringType, methodDeclaringTypeFullName, methodName);
            }
            catch (Exception ex)
            {
                ex.GetType();
                return null;
            }
        }

        private string CreateCacheKey(object[] inputs, Type methodDeclaringType, string methodDeclaringTypeFullName, string methodName)
        {
            try
            {
                var sb = new StringBuilder();
                if (methodDeclaringType != null)
                {
                    sb.Append(methodDeclaringTypeFullName);
                }
                sb.Append(':');
                sb.Append(methodName);
                TextWriter writer = new StringWriter(sb);
                if (inputs != null)
                {
                    foreach (var input in inputs)
                    {
                        sb.Append(':');
                        if (input != null)
                        {
                            //Different instances of DateTime which represents the same value
                            //sometimes serialize differently due to some internal variables which are different.
                            //We therefore serialize it using Ticks instead. instead.
                            var inputDateTime = input as DateTime?;

                            if (inputDateTime.HasValue)
                            {
                                sb.Append(inputDateTime.Value.Ticks);
                            }
                            else
                            {
                                //Serialize the input and write it to the key StringBuilder.
                                serializer.Serialize(writer, input);
                            }
                        }
                    }
                }
                return sb.ToString();
            }
            catch
            {
                //Something went wrong when generating the key (probably an input-value was not serializble.
                //Return a null key.
                return null;
            }
        }

        #endregion ICacheKeyGenerator Members

        public string CreateCacheKey(MethodInfo method, object[] inputs)
        {
            try
            {
                var methodDeclaringType = method.DeclaringType;
                var methodDeclaringTypeFullName = methodDeclaringType != null ? method.DeclaringType.FullName : null;
                var methodName = method.Name;
                return CreateCacheKey(inputs, methodDeclaringType, methodDeclaringTypeFullName, methodName);
            }
            catch (Exception ex)
            {
                ex.GetType();
                return null;
            }
        }
    }
}
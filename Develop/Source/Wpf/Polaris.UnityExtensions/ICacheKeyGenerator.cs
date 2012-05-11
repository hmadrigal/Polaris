using System.Reflection;

namespace Polaris.UnityExtensions
{
    /// <summary>
    /// This interface describes classes that can be used to generate cache key strings
    /// for the <see cref="PessimisticCacheHandler"/>.
    /// </summary>
    public interface ICacheKeyGenerator
    {
        /// <summary>
        /// Creates a cache key for the given method and set of input arguments.
        /// </summary>
        /// <param name="method">Method being called.</param>
        /// <param name="inputs">Input arguments.</param>
        /// <returns>A (hopefully) unique string to be used as a cache key.</returns>
        string CreateCacheKey(MethodBase method, object[] inputs);

        string CreateCacheKey(MethodInfo method, object[] inputs);
    }
}
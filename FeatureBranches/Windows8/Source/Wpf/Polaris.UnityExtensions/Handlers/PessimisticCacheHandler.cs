using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Polaris.UnityExtensions
{
    /// <summary>
    /// An <see cref="ICallHandler"/> that implements caching of the return values of
    /// methods. This handler stores the return value in the Enterprise Library cache.
    /// </summary>
    [ConfigurationElementType(typeof(PessimisticCacheHandler)), Synchronization]
    public class PessimisticCacheHandler : ICallHandler
    {
        #region Fields

        [Dependency]
        public IUnityContainer Container { get; set; }

        private Dictionary<string, PessimisticCacheEntryStatus> pessimisticCacheStatus;

        /// <summary>
        /// The default expiration time for the cached entries: 5 minutes
        /// </summary>
        public static readonly TimeSpan DefaultExpirationTime = new TimeSpan(0, 5, 0);

        private readonly object cachedData;
        private readonly DefaultCacheKeyGenerator keyGenerator;
        private TimeSpan expirationTime = TimeSpan.Zero;
        private GetNextHandlerDelegate getNext;
        private IMethodInvocation input;
        ICacheManager cacheManager;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the expiration time for cache data.
        /// </summary>
        /// <value>The expiration time.</value>
        public TimeSpan ExpirationTime
        {
            get { return expirationTime; }
            set { expirationTime = value; }
        }

        // The order of the handler in the interception chaine
        public int Order
        {
            get { return 0; }
            set { }
        }

        #endregion Properties

        #region Constructors

        public PessimisticCacheHandler()
            : this(null, null, null, TimeSpan.Zero) { }

        public PessimisticCacheHandler(TimeSpan expirationTime)
            : this(null, null, null, expirationTime) { }

        /// <summary>
        /// This constructor is used when we wrap cached data in a CacheHandler so that
        /// we can reload the object after it has been removed from the cache.
        /// </summary>
        /// <param name="expirationTime"></param>
        /// <param name="storeOnlyForThisRequest"></param>
        /// <param name="input"></param>
        /// <param name="getNext"></param>
        /// <param name="cachedData"></param>
        public PessimisticCacheHandler(IMethodInvocation input, GetNextHandlerDelegate getNext, object cachedData, TimeSpan expirationTime, string cacheManagerName = null)
        {
            this.keyGenerator = new DefaultCacheKeyGenerator();

            this.pessimisticCacheStatus = new Dictionary<string, PessimisticCacheEntryStatus>();

            if (input != null)
                this.input = input;
            if (getNext != null)
                this.getNext = getNext;
            if (cachedData != null)
                this.cachedData = cachedData;

            if (expirationTime != TimeSpan.Zero)
                this.expirationTime = expirationTime;
            this.cacheManager = string.IsNullOrWhiteSpace(cacheManagerName)
                    ? EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>()
                    : EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>(cacheManagerName);
        }

        #endregion Constructors

        #region ICallHandler Members

        /// <summary>
        /// Implements the caching behavior of this handler.
        /// </summary>
        /// <param name="input"><see cref="IMethodInvocation"/> object describing the current call.</param>
        /// <param name="getNext">delegate used to get the next handler in the current pipeline.</param>
        /// <returns>Return value from target method, or cached result if previous inputs have been seen.</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            lock (input.MethodBase)
            {
                this.input = input;
                this.getNext = getNext;
                return loadUsingCache();
            }
        }

        #endregion ICallHandler Members

        #region Private Methods

        private IMethodReturn loadUsingCache()
        {
            //We need to synchronize calls to the CacheHandler on method level
            //to prevent duplicate calls to methods that could be cached.
            lock (input.MethodBase)
            {
                if (TargetMethodReturnsVoid(input))
                {
                    return getNext()(input, getNext);
                }

                var inputs = new object[input.Inputs.Count];
                for (int i = 0; i < inputs.Length; ++i)
                {
                    inputs[i] = input.Inputs[i];
                }
                string cacheKey = keyGenerator.CreateCacheKey(input.MethodBase, inputs);

                PessimisticCacheEntryStatus cacheEntryStatus;

                #region Gets PessimisticCacheEntryStatus

                if (!pessimisticCacheStatus.ContainsKey(cacheKey))
                {
                    cacheEntryStatus = pessimisticCacheStatus[cacheKey] = new PessimisticCacheEntryStatus()
                    {
                        IsOperationFaulted = true
                    };
                }
                else
                {
                    cacheEntryStatus = pessimisticCacheStatus[cacheKey];
                }

                #endregion Gets PessimisticCacheEntryStatus

                IMethodReturn returnResult = null;

                if (cacheEntryStatus.IsOperationFaulted || !cacheManager.Contains(cacheKey))
                {
                    returnResult = TryToInvoke(cacheKey, cacheEntryStatus, returnResult);
                }
                else
                {
                    returnResult = GetMethodReturnFromCache(cacheKey, input.Arguments);
                }
                return returnResult;
            }
        }

        private IMethodReturn TryToInvoke(string cacheKey, PessimisticCacheEntryStatus cacheEntryStatus, IMethodReturn returnResult)
        {
            returnResult = getNext()(input, getNext);
            if (returnResult.Exception == null && returnResult.ReturnValue != null)
            {
                AddToCache(cacheKey, returnResult.ReturnValue);
                cacheEntryStatus.IsOperationFaulted = false;
            }
            else
            {
                if (cacheManager.Contains(cacheKey))
                {
                    returnResult = GetMethodReturnFromCache(cacheKey, input.Arguments);
                }
            }
            return returnResult;
        }

        private IMethodReturn GetMethodReturnFromCache(string cacheKey, IParameterCollection arguments)
        {
            var cachedResult = getCachedResult(cacheKey);
            var returnResult = input.CreateMethodReturn(cachedResult, arguments);
            return returnResult;
        }

        public static void UpdateCache<T>(Object updatedValue, String methodName, Object[] input = null)
        {
            input = input != null ? input : new object[] { };
            var method = typeof(T).GetMethod(methodName);
            var keyGenerator = new DefaultCacheKeyGenerator();
            var cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            string cacheKey = keyGenerator.CreateCacheKey(method, input);
            if (updatedValue == null && cache.Contains(cacheKey))
            {
                cache.Remove(cacheKey);
            }
            else
            {
                cache.Add(cacheKey, updatedValue);
            }
        }

        private object getCachedResult(string cacheKey)
        {
            //When the method uses input that is not serializable
            //we cannot create a cache key and can therefore not
            //cache the data.
            if (string.IsNullOrEmpty(cacheKey))
            {
                return null;
            }

            object cachedValue = this.cacheManager.GetData(cacheKey);
            var cachedValueCast = cachedValue as PessimisticCacheHandler;
            if (cachedValueCast != null)
            {
                //This is an object that is reloaded when it is being removed.
                //It is therefore wrapped in a CacheHandler-object and we must
                //unwrap it before returning it.
                return cachedValueCast.cachedData;
            }
            return cachedValue;
        }

        private static bool TargetMethodReturnsVoid(IMethodInvocation input)
        {
            var targetMethod = input.MethodBase as MethodInfo;
            return targetMethod != null && targetMethod.ReturnType == typeof(void);
        }

        private void AddToCache(string key, object valueToCache)
        {
            if (key == null)
            {
                //When the method uses input that is not serializable
                //we cannot create a cache key and can therefore not
                //cache the data.
                return;
            }

            if (expirationTime.Equals(TimeSpan.Zero))
            {
                cacheManager.Add(key, valueToCache);
            }
            else
            {
                AbsoluteTime expiry = new AbsoluteTime(expirationTime);
                cacheManager.Add(key,
                    valueToCache,
                    CacheItemPriority.Normal,
                    null,
                    new ICacheItemExpiration[] { expiry });
            }
        }

        #endregion Private Methods
    }

    internal class PessimisticCacheEntryStatus
    {
        public bool IsOperationFaulted { get; set; }
    }
}
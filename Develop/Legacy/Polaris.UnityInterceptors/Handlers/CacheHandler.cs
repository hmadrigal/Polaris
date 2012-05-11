namespace Polaris.UnityInterceptors.Handlers
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using System.Reflection;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using Microsoft.Practices.EnterpriseLibrary.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

    /// <summary>    
    /// An <see cref="ICallHandler"/> that implements caching of the return values of    
    /// methods. This handler stores the return value in the Enterprise Library cache.    
    /// </summary>        
    public class CacheHandler : ICallHandler
    {
        /// <summary>        
        /// The default expiration time for the cached entries: 5 minutes        
        /// </summary>
        public static readonly TimeSpan DefaultExpirationTime = new TimeSpan(0, 5, 0);
        private readonly object cachedData;
        private readonly DefaultCacheKeyGenerator keyGenerator;
        private TimeSpan expirationTime;
        private GetNextHandlerDelegate getNext;
        private IMethodInvocation input;
        ICacheManager cache;

        /// <summary>        
        /// Gets or sets the expiration time for cache data.        
        /// </summary>        
        /// <value>The expiration time.</value>        
        public TimeSpan ExpirationTime
        {
            get { return expirationTime; }
            set { expirationTime = value; }
        }

        #region Constructors
        public CacheHandler()
            : this(null, null, null, TimeSpan.Zero) { }
        public CacheHandler(TimeSpan expirationTime)
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
        public CacheHandler(IMethodInvocation input, GetNextHandlerDelegate getNext, object cachedData, TimeSpan expirationTime, string cacheManagerName = null)
        {
            this.keyGenerator = new DefaultCacheKeyGenerator();

            if (input != null)
                this.input = input;
            if (getNext != null)
                this.getNext = getNext;
            if (cachedData != null)
                this.cachedData = cachedData;

            if (expirationTime != TimeSpan.Zero)
                this.expirationTime = expirationTime;
            this.cache = string.IsNullOrWhiteSpace(cacheManagerName)
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

        public int Order
        {
            get { return 0; }
            set { }
        }


        #endregion

        private IMethodReturn loadUsingCache()
        {
            //We need to synchronize calls to the CacheHandler on method level            
            //to prevent duplicate calls to methods that could be cached.            
            lock (input.MethodBase)
            {
                if (TargetMethodReturnsVoid(input) || this.cache == null)
                {
                    return getNext()(input, getNext);
                }

                var inputs = new object[input.Inputs.Count];

                for (var i = 0; i < inputs.Length; ++i)
                {
                    inputs[i] = input.Inputs[i];
                }

                var cacheKey = keyGenerator.CreateCacheKey(input.MethodBase, inputs);
                var cachedResult = getCachedResult(cacheKey);

                if (cachedResult == null)
                {
                    var realReturn = getNext()(input, getNext);
                    if (realReturn.Exception == null && realReturn.ReturnValue != null)
                    {
                        AddToCache(cacheKey, realReturn.ReturnValue);
                    } return realReturn;
                }

                var cachedReturn = input.CreateMethodReturn(cachedResult, input.Arguments);
                return cachedReturn;
            }
        }

        private object getCachedResult(string cacheKey)
        {
            //When the method uses input that is not serializable             
            //we cannot create a cache key and can therefore not             
            //cache the data.            
            if (cacheKey == null)
            {
                return null;
            }

            var cachedValue = this.cache.GetData(cacheKey);
            var cachedValueCast = cachedValue as CacheHandler;
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
                cache.Add(key, valueToCache);
            }
            else
            {
                var expiry = new AbsoluteTime(expirationTime);
                cache.Add(key,
                    valueToCache,
                    CacheItemPriority.Normal,
                    null,
                    new ICacheItemExpiration[] { expiry });
            }

        }
    }
}

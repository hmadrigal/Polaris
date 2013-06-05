using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace Polaris.PhoneLib.IoC
{
    public sealed class PhoneServiceLocator : SimpleIoc, Microsoft.Practices.ServiceLocation.IServiceLocator, System.IServiceProvider, Polaris.PhoneLib.Services.IPhoneServiceLocator
    {
        private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Delegate> _instanceFactories = new Dictionary<Type, Delegate>();
        private readonly Dictionary<Type, ConstructorInfo> _constructorInfos = new Dictionary<Type, ConstructorInfo>();
        private readonly object[] _emptyArguments = new object[0];

        public void RegisterSingleton<TInterface, TClass>()
            where TClass : class
            where TInterface : class
        {
            var typeOfInterface = typeof(TInterface);
            if (_singletons.ContainsKey(typeOfInterface))
                return;
            if (!base.IsRegistered<TClass>())
            {
                base.Register<TInterface, TClass>();
            }
            _singletons[typeOfInterface] = base.GetInstance<TInterface>();
        }

        public void RegisterTransient<TInterface>(Func<TInterface> factoryFunction)
        {
            var typeOfInterface = typeof(TInterface);
            _instanceFactories[typeOfInterface] = factoryFunction;
        }


        public void RegisterTransient<TConcrete>() where TConcrete : class
        {
            var typeOfConcrete = typeof(TConcrete);
            _instanceFactories[typeOfConcrete] = new Func<TConcrete>(() => MakeInstance(typeOfConcrete) as TConcrete);
        }


        public void RegisterTransient<TInterface, TClass>()
            where TClass : TInterface
        {
            Func<TInterface> factory = new Func<TInterface>(MakeInstance<TInterface, TClass>);
            RegisterTransient<TInterface>(factory);
        }


        private TInterface MakeInstance<TInterface, TClass>()
            where TClass : TInterface
        {
            Type index = typeof(TClass);
            ConstructorInfo constructorInfo = this._constructorInfos.ContainsKey(index) ? this._constructorInfos[index] : this._constructorInfos[index] = GetConstructorInfo(index);
            ParameterInfo[] parameters1 = constructorInfo.GetParameters();
            if (parameters1.Length == 0)
                return (TInterface)constructorInfo.Invoke(this._emptyArguments);
            object[] parameters2 = new object[parameters1.Length];
            foreach (ParameterInfo parameterInfo in parameters1)
                parameters2[parameterInfo.Position] = GetInstance(parameterInfo.ParameterType);
            return (TInterface)constructorInfo.Invoke(parameters2);
        }

        private object MakeInstance(Type index)
        {
            ConstructorInfo constructorInfo = this._constructorInfos.ContainsKey(index) ? this._constructorInfos[index] : this._constructorInfos[index] = GetConstructorInfo(index);
            ParameterInfo[] parameters1 = constructorInfo.GetParameters();
            if (parameters1.Length == 0)
                return constructorInfo.Invoke(this._emptyArguments);
            object[] parameters2 = new object[parameters1.Length];
            foreach (ParameterInfo parameterInfo in parameters1)
                parameters2[parameterInfo.Position] = GetInstance(parameterInfo.ParameterType);
            return constructorInfo.Invoke(parameters2);
        }

        private static ConstructorInfo GetConstructorInfo(Type index)
        {
            return index.GetConstructors().FirstOrDefault();
        }

        public void RegisterSingleton<TConcrete>() where TConcrete : class
        {
            var typeOfConcrete = typeof(TConcrete);
            var instance = MakeInstance(typeOfConcrete);
            RegisterSingleton<TConcrete>(instance);
        }

        public void RegisterSingleton<TInterface>(object instance) where TInterface : class
        {
            var typeOfInterface = typeof(TInterface);
            if (_singletons.ContainsKey(typeOfInterface))
                return;
            if (!base.IsRegistered<TInterface>())
            {
                base.Register<TInterface>(() => (TInterface)instance);
            }
            _singletons[typeOfInterface] = base.GetInstance<TInterface>();
        }

        public TInterface ResolveSingleton<TInterface>()
        {
            var typeOfInterface = typeof(TInterface);
            return _singletons.ContainsKey(typeOfInterface) ? (TInterface)_singletons[typeOfInterface] : default(TInterface);
        }

        public TInterface ResolveTransient<TInterface>()
        {
            var typeOfInterface = typeof(TInterface);
            if (_instanceFactories.ContainsKey(typeOfInterface))
                return (TInterface)_instanceFactories[typeOfInterface].DynamicInvoke();
            return default(TInterface);
        }

        private void InitializePhoneServiceLocatorServiceLocator()
        { }

        #region Singleton Pattern w/ Constructor
        private PhoneServiceLocator()
            : base()
        {
            InitializePhoneServiceLocatorServiceLocator();
        }
        public static PhoneServiceLocator Instance
        {
            get
            {
                return SingletonPhoneServiceLocatorServiceLocatorCreator._Instance;
            }
        }
        private class SingletonPhoneServiceLocatorServiceLocatorCreator
        {
            private SingletonPhoneServiceLocatorServiceLocatorCreator() { }
            public static PhoneServiceLocator _Instance = new PhoneServiceLocator();
        }
        #endregion

        #region IServiceLocator
        System.Collections.Generic.IEnumerable<TService> IServiceLocator.GetAllInstances<TService>()
        {
            foreach (var item in _singletons.Keys.OfType<TService>())
            {
                yield return item;
            }
            foreach (var item in base.GetAllInstances<TService>())
            {
                yield return item;
            }

        }

        System.Collections.Generic.IEnumerable<object> IServiceLocator.GetAllInstances(System.Type serviceType)
        {
            foreach (var item in _singletons.Keys.Where(i => i.GetType() == serviceType))
            {
                yield return item;
            }
            foreach (var item in base.GetAllInstances(serviceType))
            {
                yield return item;
            }

        }

        TService IServiceLocator.GetInstance<TService>(string key)
        {

            return base.GetInstance<TService>(key);
        }

        object IServiceLocator.GetInstance(System.Type serviceType, string key)
        {
            return base.GetInstance(serviceType, key);
        }

        object IServiceLocator.GetInstance(System.Type serviceType)
        {
            if (_singletons.ContainsKey(serviceType))
            {
                return _singletons[serviceType];
            }
            if (_instanceFactories.ContainsKey(serviceType))
            {
                return _instanceFactories[serviceType].DynamicInvoke();
            }
            return base.GetInstance(serviceType);
        }

        object System.IServiceProvider.GetService(System.Type serviceType)
        {

            if (_singletons.ContainsKey(serviceType))
                return _singletons[serviceType];
            if (_instanceFactories.ContainsKey(serviceType))
            {
                return _instanceFactories[serviceType].DynamicInvoke();
            }
            return base.GetService(serviceType);
        }
        #endregion

        public new TService GetInstance<TService>()
        {
            var serviceType = typeof(TService);
            if (_instanceFactories.ContainsKey(serviceType))
            {
                return ResolveTransient<TService>();
            }
            return base.GetInstance<TService>();
        }

        public new object GetInstance(Type serviceType)
        {
            if (_instanceFactories.ContainsKey(serviceType))
            {
                return _instanceFactories[serviceType].DynamicInvoke();
            }
            return base.GetInstance(serviceType);
        }

    }
}

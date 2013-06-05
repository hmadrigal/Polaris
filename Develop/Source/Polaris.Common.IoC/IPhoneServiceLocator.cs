using System;
namespace Polaris.Common.IoC
{
    public interface IPhoneServiceLocator
    {
        object GetInstance(Type serviceType);
        TService GetInstance<TService>();
        void RegisterSingleton<TConcrete>() where TConcrete : class;
        void RegisterSingleton<TInterface, TClass>()
            where TInterface : class
            where TClass : class;
        void RegisterSingleton<TInterface>(object instance) where TInterface : class;
        void RegisterTransient<TConcrete>() where TConcrete : class;
        void RegisterTransient<TInterface, TClass>() where TClass : TInterface;
        void RegisterTransient<TInterface>(Func<TInterface> factoryFunction);
        TInterface ResolveSingleton<TInterface>();
        TInterface ResolveTransient<TInterface>();
    }
}

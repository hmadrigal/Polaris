namespace Polaris.UnityInterceptors.Services
{
    using System;
    using Microsoft.Practices.Unity;

    public interface ITask
    {
        Guid Id { get; set; }
        object[] Arguments { get; set; }
        Type ContractType { get; set; }
        object Invoke(IUnityContainer container);
        string MethodName { get; set; }

        DateTime DueDate { get; set; }
        int RetryCount { get; set; }
    }
}

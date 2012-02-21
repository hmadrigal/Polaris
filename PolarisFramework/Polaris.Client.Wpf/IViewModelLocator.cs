//-----------------------------------------------------------------------
// <copyright file="IViewModelLocator.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Client.Wpf
{
    public interface IViewModelLocator
    {
        TViewModelType Resolve<TViewModelType>();
    }
}

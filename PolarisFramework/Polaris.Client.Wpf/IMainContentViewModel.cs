//-----------------------------------------------------------------------
// <copyright file="IMainContentViewModel.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Client.Wpf
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Represents a view model which is the main content or page.
    /// </summary>
    public interface IMainContentViewModel
    {
        /// <summary>
        /// Gets or sets the unity container for this view model.
        /// </summary>
        IUnityContainer Container { get; set; }

        /// <summary>
        /// Gets a flag that indicates whether this view model is already retrieving and using real data or not.
        /// </summary>
        /// <remarks>
        /// This property is used to decide whether mock values should be copied from the
        /// mock view model upon instantiation.
        /// </remarks>
        bool IsRetrievingRealData { get; }

        /// <summary>
        /// Initializes the view model.
        /// </summary>
        void Initialize();
    }
}
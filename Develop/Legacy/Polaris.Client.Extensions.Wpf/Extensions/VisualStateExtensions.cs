//-----------------------------------------------------------------------
// <copyright file="VisualStateExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Extensions
{
    using System.Windows;
    using System.Windows.Controls;

    public static class VisualStateExtensions
    {
        public static void Activate(this VisualState state, Control control, bool useTransitions)
        {
            VisualStateManager.GoToState(control, state.Name, useTransitions);
        }

        public static void Activate(this VisualState state, Control control)
        {
            state.Activate(control, true);
        }
    }
}
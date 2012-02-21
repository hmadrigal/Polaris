//-----------------------------------------------------------------------
// <copyright file="FrameworkElementExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Extensions
{
    public static class FrameworkElementExtensions
    {
        public static bool IsUserVisible(this System.Windows.FrameworkElement element, System.Windows.FrameworkElement container)
        {
            if (!element.IsVisible) { return false; }
            if (container == null) { return true; }
            System.Windows.Rect bounds = element.TransformToAncestor(container).TransformBounds(new System.Windows.Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            System.Windows.Rect rect = new System.Windows.Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
        }
    }
}

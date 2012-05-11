//-----------------------------------------------------------------------
// <copyright file="UIElementExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Extensions
{
    using System.Windows;
    using System.Windows.Media;

    public static class UIElementExtensions
    {
        public static Point GetAbsolutePosition(this UIElement target, Point? offset = null, Visual referenceVisual = null)
        {
            if (referenceVisual == null)
            {
                referenceVisual = Application.Current.MainWindow;
            }
            GeneralTransform gt = target.TransformToVisual(referenceVisual);
            if (offset == null)
            {
                offset = new Point(0, 0);
            }
            Point p = gt.Transform(offset.Value);
            return p;
        }
    }
}
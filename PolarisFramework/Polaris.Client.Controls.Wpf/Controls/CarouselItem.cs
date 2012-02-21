//-----------------------------------------------------------------------
// <copyright file="CarouselItem.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Client.Controls.Wpf.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Effects;
    [TemplateVisualState(Name = BeforeAddedStateName, GroupName = AddingStateGroupName)]
    [TemplateVisualState(Name = WhileAddingStateName, GroupName = AddingStateGroupName)]
    [TemplateVisualState(Name = AddedStateName, GroupName = AddingStateGroupName)]
    public class CarouselItem : ContentControl
    {

        public readonly static Double CircleArc = 360;

        BlurEffect blurEffect;

        private Boolean isSynchronized = true;

        internal Boolean IsSynchronized
        {
            get
            {
                return isSynchronized;
            }
            set
            {
                if (isSynchronized != value)
                {
                    isSynchronized = value;
                    if (value == true)
                    {
                        UpdatePosition(this.ScrollPosition);
                    }
                }
            }
        }

        #region AddingStates Visual States
        internal const string AddingStateGroupName = "AddingStates";
        internal const string BeforeAddedStateName = @"BeforeAdded";
        internal const string WhileAddingStateName = @"WhileAdding";
        internal const string AddedStateName = @"Added";
        #endregion

        #region TotalArc

        /// <summary>
        /// TotalArc Dependency Property
        /// </summary>
        public static readonly DependencyProperty TotalArcProperty =
            DependencyProperty.Register("TotalArc", typeof(double), typeof(CarouselItem),
                new FrameworkPropertyMetadata((double)360,
                    new PropertyChangedCallback(OnTotalArcChanged)));

        /// <summary>
        /// Gets or sets the TotalArc property.  This dependency property 
        /// indicates the total arc of the carousel.
        /// </summary>
        public double TotalArc
        {
            get { return (double)GetValue(TotalArcProperty); }
            set { SetValue(TotalArcProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TotalArc property.
        /// </summary>
        private static void OnTotalArcChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnTotalArcChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TotalArc property.
        /// </summary>
        protected virtual void OnTotalArcChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region ScrollPosition

        /// <summary>
        /// ScrollPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollPositionProperty =
            DependencyProperty.Register("ScrollPosition", typeof(Double), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Double)0.0,
                    new PropertyChangedCallback(OnScrollPositionChanged)));

        /// <summary>
        /// Gets or sets the ScrollPosition property.  This dependency property 
        /// indicates the current position of the item in the carousel scrolling cycle.
        /// </summary>
        public Double ScrollPosition
        {
            get { return (Double)GetValue(ScrollPositionProperty); }
            set { SetValue(ScrollPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ScrollPosition property.
        /// </summary>
        private static void OnScrollPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnScrollPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ScrollPosition property.
        /// </summary>
        protected virtual void OnScrollPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            var scrollPosition = (Double)e.NewValue;
            UpdatePosition(scrollPosition);
        }

        #endregion

        #region ScrollStep

        /// <summary>
        /// ScrollStep Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollStepProperty =
            DependencyProperty.Register("ScrollStep", typeof(double), typeof(CarouselItem),
                new FrameworkPropertyMetadata((double)0.1,
                    new PropertyChangedCallback(OnScrollStepChanged)));

        /// <summary>
        /// Gets or sets the ScrollStep property.  This dependency property 
        /// indicates the magnitude of the scroll step.
        /// </summary>
        public double ScrollStep
        {
            get { return (double)GetValue(ScrollStepProperty); }
            set { SetValue(ScrollStepProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ScrollStep property.
        /// </summary>
        private static void OnScrollStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnScrollStepChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ScrollStep property.
        /// </summary>
        protected virtual void OnScrollStepChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantAPosition

        /// <summary>
        /// QuadrantAPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantAPositionProperty =
            DependencyProperty.Register("QuadrantAPosition", typeof(Point), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantAPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantAPosition property.  This dependency property 
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
        public Point QuadrantAPosition
        {
            get { return (Point)GetValue(QuadrantAPositionProperty); }
            set { SetValue(QuadrantAPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantAPosition property.
        /// </summary>
        private static void OnQuadrantAPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantAPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantAPosition property.
        /// </summary>
        protected virtual void OnQuadrantAPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantBPosition

        /// <summary>
        /// QuadrantBPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBPositionProperty =
            DependencyProperty.Register("QuadrantBPosition", typeof(Point), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantBPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBPosition property.  This dependency property 
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
        public Point QuadrantBPosition
        {
            get { return (Point)GetValue(QuadrantBPositionProperty); }
            set { SetValue(QuadrantBPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantBPosition property.
        /// </summary>
        private static void OnQuadrantBPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantBPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantBPosition property.
        /// </summary>
        protected virtual void OnQuadrantBPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantCPosition

        /// <summary>
        /// QuadrantCPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCPositionProperty =
            DependencyProperty.Register("QuadrantCPosition", typeof(Point), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantCPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCPosition property.  This dependency property 
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
        public Point QuadrantCPosition
        {
            get { return (Point)GetValue(QuadrantCPositionProperty); }
            set { SetValue(QuadrantCPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantCPosition property.
        /// </summary>
        private static void OnQuadrantCPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantCPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantCPosition property.
        /// </summary>
        protected virtual void OnQuadrantCPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantDPosition

        /// <summary>
        /// QuadrantDPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDPositionProperty =
            DependencyProperty.Register("QuadrantDPosition", typeof(Point), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantDPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDPosition property.  This dependency property 
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
        public Point QuadrantDPosition
        {
            get { return (Point)GetValue(QuadrantDPositionProperty); }
            set { SetValue(QuadrantDPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantDPosition property.
        /// </summary>
        private static void OnQuadrantDPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantDPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantDPosition property.
        /// </summary>
        protected virtual void OnQuadrantDPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantASize

        /// <summary>
        /// QuadrantASize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantASizeProperty =
            DependencyProperty.Register("QuadrantASize", typeof(Size), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Size)new Size(),
                    new PropertyChangedCallback(OnQuadrantASizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantASize property.  This dependency property 
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
        public Size QuadrantASize
        {
            get { return (Size)GetValue(QuadrantASizeProperty); }
            set { SetValue(QuadrantASizeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantASize property.
        /// </summary>
        private static void OnQuadrantASizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantASizeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantASize property.
        /// </summary>
        protected virtual void OnQuadrantASizeChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantBSize

        /// <summary>
        /// QuadrantBSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBSizeProperty =
            DependencyProperty.Register("QuadrantBSize", typeof(Size), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Size)new Size(),
                    new PropertyChangedCallback(OnQuadrantBSizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBSize property.  This dependency property 
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
        public Size QuadrantBSize
        {
            get { return (Size)GetValue(QuadrantBSizeProperty); }
            set { SetValue(QuadrantBSizeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantBSize property.
        /// </summary>
        private static void OnQuadrantBSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantBSizeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantBSize property.
        /// </summary>
        protected virtual void OnQuadrantBSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantCSize

        /// <summary>
        /// QuadrantCSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCSizeProperty =
            DependencyProperty.Register("QuadrantCSize", typeof(Size), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Size)new Size(),
                    new PropertyChangedCallback(OnQuadrantCSizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCSize property.  This dependency property 
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
        public Size QuadrantCSize
        {
            get { return (Size)GetValue(QuadrantCSizeProperty); }
            set { SetValue(QuadrantCSizeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantCSize property.
        /// </summary>
        private static void OnQuadrantCSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantCSizeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantCSize property.
        /// </summary>
        protected virtual void OnQuadrantCSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantDSize

        /// <summary>
        /// QuadrantDSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDSizeProperty =
            DependencyProperty.Register("QuadrantDSize", typeof(Size), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Size)new Size(),
                    new PropertyChangedCallback(OnQuadrantDSizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDSize property.  This dependency property 
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
        public Size QuadrantDSize
        {
            get { return (Size)GetValue(QuadrantDSizeProperty); }
            set { SetValue(QuadrantDSizeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantDSize property.
        /// </summary>
        private static void OnQuadrantDSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantDSizeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantDSize property.
        /// </summary>
        protected virtual void OnQuadrantDSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantAOpacity

        /// <summary>
        /// QuadrantAOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantAOpacityProperty =
            DependencyProperty.Register("QuadrantAOpacity", typeof(Double), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Double)1.0,
                    new PropertyChangedCallback(OnQuadrantAOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantAOpacity property.  This dependency property 
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
        public Double QuadrantAOpacity
        {
            get { return (Double)GetValue(QuadrantAOpacityProperty); }
            set { SetValue(QuadrantAOpacityProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantAOpacity property.
        /// </summary>
        private static void OnQuadrantAOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantAOpacityChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantAOpacity property.
        /// </summary>
        protected virtual void OnQuadrantAOpacityChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantBOpacity

        /// <summary>
        /// QuadrantBOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBOpacityProperty =
            DependencyProperty.Register("QuadrantBOpacity", typeof(Double), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Double)1.0,
                    new PropertyChangedCallback(OnQuadrantBOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBOpacity property.  This dependency property 
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
        public Double QuadrantBOpacity
        {
            get { return (Double)GetValue(QuadrantBOpacityProperty); }
            set { SetValue(QuadrantBOpacityProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantBOpacity property.
        /// </summary>
        private static void OnQuadrantBOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantBOpacityChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantBOpacity property.
        /// </summary>
        protected virtual void OnQuadrantBOpacityChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantCOpacity

        /// <summary>
        /// QuadrantCOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCOpacityProperty =
            DependencyProperty.Register("QuadrantCOpacity", typeof(Double), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Double)1.0,
                    new PropertyChangedCallback(OnQuadrantCOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCOpacity property.  This dependency property 
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
        public Double QuadrantCOpacity
        {
            get { return (Double)GetValue(QuadrantCOpacityProperty); }
            set { SetValue(QuadrantCOpacityProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantCOpacity property.
        /// </summary>
        private static void OnQuadrantCOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantCOpacityChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantCOpacity property.
        /// </summary>
        protected virtual void OnQuadrantCOpacityChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantDOpacity

        /// <summary>
        /// QuadrantDOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDOpacityProperty =
            DependencyProperty.Register("QuadrantDOpacity", typeof(Double), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Double)1.0,
                    new PropertyChangedCallback(OnQuadrantDOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDOpacity property.  This dependency property 
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
        public Double QuadrantDOpacity
        {
            get { return (Double)GetValue(QuadrantDOpacityProperty); }
            set { SetValue(QuadrantDOpacityProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantDOpacity property.
        /// </summary>
        private static void OnQuadrantDOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantDOpacityChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantDOpacity property.
        /// </summary>
        protected virtual void OnQuadrantDOpacityChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantABlurRadius

        /// <summary>
        /// QuadrantABlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantABlurRadiusProperty =
            DependencyProperty.Register("QuadrantABlurRadius", typeof(Double), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Double)0.0,
                    new PropertyChangedCallback(OnQuadrantABlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantABlurRadius property.  This dependency property 
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
        public Double QuadrantABlurRadius
        {
            get { return (Double)GetValue(QuadrantABlurRadiusProperty); }
            set { SetValue(QuadrantABlurRadiusProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantABlurRadius property.
        /// </summary>
        private static void OnQuadrantABlurRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantABlurRadiusChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantABlurRadius property.
        /// </summary>
        protected virtual void OnQuadrantABlurRadiusChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantBBlurRadius

        /// <summary>
        /// QuadrantBBlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBBlurRadiusProperty =
            DependencyProperty.Register("QuadrantBBlurRadius", typeof(Double), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Double)0.0,
                    new PropertyChangedCallback(OnQuadrantBBlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBBlurRadius property.  This dependency property 
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
        public Double QuadrantBBlurRadius
        {
            get { return (Double)GetValue(QuadrantBBlurRadiusProperty); }
            set { SetValue(QuadrantBBlurRadiusProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantBBlurRadius property.
        /// </summary>
        private static void OnQuadrantBBlurRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantBBlurRadiusChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantBBlurRadius property.
        /// </summary>
        protected virtual void OnQuadrantBBlurRadiusChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantCBlurRadius

        /// <summary>
        /// QuadrantCBlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCBlurRadiusProperty =
            DependencyProperty.Register("QuadrantCBlurRadius", typeof(Double), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Double)0.0,
                    new PropertyChangedCallback(OnQuadrantCBlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCBlurRadius property.  This dependency property 
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
        public Double QuadrantCBlurRadius
        {
            get { return (Double)GetValue(QuadrantCBlurRadiusProperty); }
            set { SetValue(QuadrantCBlurRadiusProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantCBlurRadius property.
        /// </summary>
        private static void OnQuadrantCBlurRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantCBlurRadiusChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantCBlurRadius property.
        /// </summary>
        protected virtual void OnQuadrantCBlurRadiusChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region QuadrantDBlurRadius

        /// <summary>
        /// QuadrantDBlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDBlurRadiusProperty =
            DependencyProperty.Register("QuadrantDBlurRadius", typeof(Double), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Double)0.0,
                    new PropertyChangedCallback(OnQuadrantDBlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDBlurRadius property.  This dependency property 
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
        public Double QuadrantDBlurRadius
        {
            get { return (Double)GetValue(QuadrantDBlurRadiusProperty); }
            set { SetValue(QuadrantDBlurRadiusProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantDBlurRadius property.
        /// </summary>
        private static void OnQuadrantDBlurRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnQuadrantDBlurRadiusChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantDBlurRadius property.
        /// </summary>
        protected virtual void OnQuadrantDBlurRadiusChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePosition(ScrollPosition);
        }

        #endregion

        #region IsBlurEnabled

        /// <summary>
        /// IsBlurEnabled Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsBlurEnabledProperty =
            DependencyProperty.Register("IsBlurEnabled", typeof(bool), typeof(CarouselItem),
                new FrameworkPropertyMetadata((bool)true,
                    new PropertyChangedCallback(OnIsBlurEnabledChanged)));

        /// <summary>
        /// Gets or sets the IsBlurEnabled property.  This dependency property 
        /// indicates whether the blur effect is enabled or not.
        /// </summary>
        public bool IsBlurEnabled
        {
            get { return (bool)GetValue(IsBlurEnabledProperty); }
            set { SetValue(IsBlurEnabledProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsBlurEnabled property.
        /// </summary>
        private static void OnIsBlurEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnIsBlurEnabledChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IsBlurEnabled property.
        /// </summary>
        protected virtual void OnIsBlurEnabledChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region CurrentQuadrant

        /// <summary>
        /// CurrentQuadrant Read-Only Dependency Property
        /// </summary>
        private static readonly DependencyPropertyKey CurrentQuadrantPropertyKey
            = DependencyProperty.RegisterReadOnly("CurrentQuadrant", typeof(Quadrant), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Quadrant)Quadrant.Unknown,
                    new PropertyChangedCallback(OnCurrentQuadrantChanged)));

        public static readonly DependencyProperty CurrentQuadrantProperty
            = CurrentQuadrantPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the CurrentQuadrant property.  This dependency property 
        /// indicates the quadrant this item is currently at.
        /// </summary>
        public Quadrant CurrentQuadrant
        {
            get { return (Quadrant)GetValue(CurrentQuadrantProperty); }
        }

        /// <summary>
        /// Provides a secure method for setting the CurrentQuadrant property.  
        /// This dependency property indicates the quadrant this item is currently at.
        /// </summary>
        /// <param name="value">The new value for the property.</param>
        protected void SetCurrentQuadrant(Quadrant value)
        {
            SetValue(CurrentQuadrantPropertyKey, value);
        }

        /// <summary>
        /// Handles changes to the CurrentQuadrant property.
        /// </summary>
        private static void OnCurrentQuadrantChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnCurrentQuadrantChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CurrentQuadrant property.
        /// </summary>
        protected virtual void OnCurrentQuadrantChanged(DependencyPropertyChangedEventArgs e)
        {
#if DEBUG
            //System.Diagnostics.Debug.WriteLine("Item {0} has entered quadrant {1}", this.GetHashCode(), this.CurrentQuadrant);
#endif

        }

        #endregion

        #region VanishingPoint

        public event EventHandler VanishingPointReached;
        internal virtual void OnVanishingPointReached()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Item {0} (angle {1}) has reached vanishing point", this.GetHashCode(), this.GetCurrentAngle());
#endif
            var threadSafeInstance = VanishingPointReached;
            if (threadSafeInstance != null)
            {
                threadSafeInstance(this, EventArgs.Empty);
            }
            if (VanishingPointCommand != null)
            {
                if (VanishingPointCommand.CanExecute(VanishingPointCommandParameter))
                {
                    VanishingPointCommand.Execute(VanishingPointCommandParameter);
                }
            }
        }

        /// <summary>
        /// VanishingPoint Dependency Property
        /// </summary>
        public static readonly DependencyProperty VanishingPointProperty =
            DependencyProperty.Register("VanishingPoint", typeof(Quadrant), typeof(CarouselItem),
                new FrameworkPropertyMetadata((Quadrant)Quadrant.Unknown,
                    new PropertyChangedCallback(OnVanishingPointChanged)));

        /// <summary>
        /// Gets or sets the VanishingPoint property.  This dependency property 
        /// indicates the quadrant at which items "vanish" in the distance.
        /// This is very useful when it is necessary to remove items from the visual
        /// space smoothly. Items that have reached the vanishing point should be
        /// removable from the collection without been noticed by the user.
        /// It is necessary that the vanishing point is either off-screen or
        /// visually unrecognizable for this mechanism to work smoothly.
        /// </summary>
        public Quadrant VanishingPoint
        {
            get { return (Quadrant)GetValue(VanishingPointProperty); }
            set { SetValue(VanishingPointProperty, value); }
        }

        /// <summary>
        /// Handles changes to the VanishingPoint property.
        /// </summary>
        private static void OnVanishingPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItem)d).OnVanishingPointChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the VanishingPoint property.
        /// </summary>
        protected virtual void OnVanishingPointChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region VanishingPointCommand

        /// <summary>
        /// VanishingPointCommand Dependency Property
        /// </summary>
        public static readonly DependencyProperty VanishingPointCommandProperty =
            DependencyProperty.Register("VanishingPointCommand", typeof(ICommand), typeof(CarouselItem),
                new FrameworkPropertyMetadata((ICommand)null));

        /// <summary>
        /// Gets or sets the VanishingPointCommand property.  This dependency property 
        /// indicates the command that is executed when this item reaches the vanishing point.
        /// </summary>
        public ICommand VanishingPointCommand
        {
            get { return (ICommand)GetValue(VanishingPointCommandProperty); }
            set { SetValue(VanishingPointCommandProperty, value); }
        }

        #endregion

        #region VanishingPointCommandParameter

        /// <summary>
        /// VanishingPointCommandParameter Dependency Property
        /// </summary>
        public static readonly DependencyProperty VanishingPointCommandParameterProperty =
            DependencyProperty.Register("VanishingPointCommandParameter", typeof(object), typeof(CarouselItem),
                new FrameworkPropertyMetadata((object)null));

        /// <summary>
        /// Gets or sets the VanishingPointCommandParameter property.  This dependency property 
        /// indicates the parameter that will be sent to the executed command when the vanishing point is reached.
        /// </summary>
        public object VanishingPointCommandParameter
        {
            get { return (object)GetValue(VanishingPointCommandParameterProperty); }
            set { SetValue(VanishingPointCommandParameterProperty, value); }
        }

        #endregion

        static CarouselItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CarouselItem), new FrameworkPropertyMetadata(typeof(CarouselItem)));
        }

        private BlurEffect GetBlurEffect()
        {
            if (this.blurEffect != null) { return this.blurEffect; }
            var blurEffect = this.Effect as BlurEffect;
            if (blurEffect == null)
            {
                this.Effect = new System.Windows.Media.Effects.BlurEffect();
                blurEffect = this.Effect as System.Windows.Media.Effects.BlurEffect;
                blurEffect.RenderingBias = System.Windows.Media.Effects.RenderingBias.Performance;
            }
            this.blurEffect = blurEffect;
            return blurEffect;
        }

        private void UpdatePositionAsync(double scrollPosition)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(new Action<Object>((state) =>
            {
                UpdatePosition(scrollPosition);
            })));
        }

        private void UpdatePosition(double scrollPosition)
        {
            if (!isSynchronized) { return; }

            var scrollPositionDescriptor = GetScrollPositionDescriptor(scrollPosition);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                Canvas.SetTop(this, scrollPositionDescriptor.Top);
                Canvas.SetLeft(this, scrollPositionDescriptor.Left);
                this.Width = scrollPositionDescriptor.Width;
                this.Height = scrollPositionDescriptor.Height;
                this.Opacity = scrollPositionDescriptor.Opacity;
                var blurEffect = (IsBlurEnabled ? GetBlurEffect() : null);
                if (blurEffect != null)
                {
                    blurEffect.Radius = scrollPositionDescriptor.BlurRadius;
                }
            }));
        }

        internal ScrollPositionDescriptor GetScrollPositionDescriptor(double scrollPosition)
        {
            var normalizedArc = TotalArc % CircleArc;
            normalizedArc = (normalizedArc == 0 ? CircleArc : normalizedArc);
            var degrees = (scrollPosition * ScrollStep) % normalizedArc;

            var quadrantSize = normalizedArc / 4;
            var aQuadrantAngle = 0 * quadrantSize;
            var bQuadrantAngle = 1 * quadrantSize;
            var cQuadrantAngle = 2 * quadrantSize;
            var dQuadrantAngle = 3 * quadrantSize;
            var quadrantUnit = 1 / quadrantSize;


            var initialY = 0.0;
            var finalY = 0.0;
            var initialX = 0.0;
            var finalX = 0.0;
            var initialBlurRadius = 0.0;
            var finalBlurRadius = 0.0;
            var initialSize = new Size(0, 0);
            var finalSize = new Size(0, 0);
            var initialOpacity = 0.0;
            var finalOpacity = 0.0;
            var quadrantAngle = 0.0;
            var degreesAbsolute = Math.Abs(degrees);
            if (degrees < 0)
            {
                degrees = (normalizedArc + degrees) % normalizedArc;
            }
            if (degrees >= aQuadrantAngle && degrees < bQuadrantAngle)
            {
                SetCurrentQuadrant(Quadrant.QuadrantA);
                quadrantAngle = aQuadrantAngle;
                initialY = QuadrantAPosition.Y;
                finalY = QuadrantBPosition.Y;
                initialX = QuadrantAPosition.X;
                finalX = QuadrantBPosition.X;
                initialSize = QuadrantASize;
                finalSize = QuadrantBSize;
                initialOpacity = QuadrantAOpacity;
                finalOpacity = QuadrantBOpacity;
                initialBlurRadius = QuadrantABlurRadius;
                finalBlurRadius = QuadrantBBlurRadius;
            }
            else if (degrees >= bQuadrantAngle && degrees < cQuadrantAngle)
            {
                SetCurrentQuadrant(Quadrant.QuadrantB);
                quadrantAngle = bQuadrantAngle;
                initialY = QuadrantBPosition.Y;
                finalY = QuadrantCPosition.Y;
                initialX = QuadrantBPosition.X;
                finalX = QuadrantCPosition.X;
                initialSize = QuadrantBSize;
                finalSize = QuadrantCSize;
                initialOpacity = QuadrantBOpacity;
                finalOpacity = QuadrantCOpacity;
                initialBlurRadius = QuadrantBBlurRadius;
                finalBlurRadius = QuadrantCBlurRadius;
            }
            else if (degrees >= cQuadrantAngle && degrees < dQuadrantAngle)
            {
                SetCurrentQuadrant(Quadrant.QuadrantC);
                quadrantAngle = cQuadrantAngle;
                initialY = QuadrantCPosition.Y;
                finalY = QuadrantDPosition.Y;
                initialX = QuadrantCPosition.X;
                finalX = QuadrantDPosition.X;
                initialSize = QuadrantCSize;
                finalSize = QuadrantDSize;
                initialOpacity = QuadrantCOpacity;
                finalOpacity = QuadrantDOpacity;
                initialBlurRadius = QuadrantCBlurRadius;
                finalBlurRadius = QuadrantDBlurRadius;
            }
            else // if (degrees >= dQuadrantAngle && degrees < normalizedArc)
            {
                SetCurrentQuadrant(Quadrant.QuadrantD);
                quadrantAngle = dQuadrantAngle;
                initialY = QuadrantDPosition.Y;
                finalY = QuadrantAPosition.Y;
                initialX = QuadrantDPosition.X;
                finalX = QuadrantAPosition.X;
                initialSize = QuadrantDSize;
                finalSize = QuadrantASize;
                initialOpacity = QuadrantDOpacity;
                finalOpacity = QuadrantAOpacity;
                initialBlurRadius = QuadrantDBlurRadius;
                finalBlurRadius = QuadrantABlurRadius;
            }

            var normalizedMagnitude = (degrees - quadrantAngle) * quadrantUnit;
            var yMagnitude = finalY - initialY;
            var xMagnitude = finalX - initialX;
            var widthMagnitude = finalSize.Width - initialSize.Width;
            var heightMagnitude = finalSize.Height - initialSize.Height;
            var opacityMagnitude = finalOpacity - initialOpacity;
            var blurRadiusMagnitude = finalBlurRadius - initialBlurRadius;
            var currentYDelta = normalizedMagnitude * yMagnitude;
            var currentXDelta = normalizedMagnitude * xMagnitude;
            var currentOpacityDelta = normalizedMagnitude * opacityMagnitude;
            var currentBlurRadiusDelta = normalizedMagnitude * blurRadiusMagnitude;
            var currentWidthDelta = normalizedMagnitude * widthMagnitude;
            var currentHeightDelta = normalizedMagnitude * heightMagnitude;
            var newY = currentYDelta + initialY;
            var newX = currentXDelta + initialX;
            var newOpacity = currentOpacityDelta + initialOpacity;
            var newBlurRadius = currentBlurRadiusDelta + initialBlurRadius;
            var newWidth = currentWidthDelta + initialSize.Width;
            var newHeight = currentHeightDelta + initialSize.Height;

            var scrollPositionDescriptor = new ScrollPositionDescriptor()
            {
                Left = newX,
                Top = newY,
                Opacity = newOpacity,
                Width = newWidth,
                Height = newHeight,
                BlurRadius = newBlurRadius,
            };
            return scrollPositionDescriptor;
        }

        public double GetCurrentAngle()
        {
            var normalizedArc = TotalArc % CarouselItem.CircleArc;
            normalizedArc = (normalizedArc == 0 ? CarouselItem.CircleArc : normalizedArc);
            var currentAngle = (ScrollPosition * ScrollStep) % normalizedArc;
            if (currentAngle < 0) { currentAngle = 360 + currentAngle; }
            return currentAngle;
        }

    }

    internal class ScrollPositionDescriptor
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Opacity { get; set; }
        public double BlurRadius { get; set; }
    }


    public enum Quadrant
    {
        /// <summary>
        /// Default value for the quadrant, used before initialization.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Represents the quadrant that goes from 0 to 89 degrees.
        /// </summary>
        QuadrantA,
        /// <summary>
        /// Represents the quadrant that goes from 90 to 179 degrees.
        /// </summary>
        QuadrantB,
        /// <summary>
        /// Represents the quadrant that goes from 180 to 269 degrees.
        /// </summary>
        QuadrantC,
        /// <summary>
        /// Represents the quadrant that goes from 270 to 359 degrees.
        /// </summary>
        QuadrantD,
    }
}

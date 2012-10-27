namespace Polaris
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using Microsoft.Practices.Prism.Events;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;
    using Polaris.Extensions;
    using Polaris.Services;

    public abstract class ViewLifetimeControllerBase : IObserver<EventPattern<TransitionEventArgs>>
    {
        readonly TimeSpan DEFAULT_DELAY = new TimeSpan(0, 0, 0, 0, 400);

        public IUnityContainer Container { get; set; }

        protected IEventAggregator EventAggregator { get; set; }

        protected IRegionManager RegionManager { get; set; }

        protected Dispatcher UiDispatcher { get; set; }

        protected IViewRegistryService ViewRegistryService { get; set; }

        readonly Dictionary<ITransitionViewModel, UnloadingView> unloadingViews = new Dictionary<ITransitionViewModel, UnloadingView>();

        protected ViewLifetimeControllerBase(IUnityContainer container)
        {
            Container = container;

            BeforeResolveDependencies();
            OnResolveDependencies();
            BeforeInitialize();
            OnInitialize();

            SubscribeToEvents();
        }

        protected virtual void SubscribeToEvents()
        {
        }

        protected virtual void OnResolveDependencies()
        {
            EventAggregator = Container.Resolve<IEventAggregator>();
            RegionManager = Container.Resolve<IRegionManager>();
            UiDispatcher = Container.Resolve<Dispatcher>(GlobalInstanceNames.UiDispatcher);
            ViewRegistryService = Container.Resolve<IViewRegistryService>();
        }

        protected virtual void OnInitialize()
        {
        }

        protected virtual void BeforeResolveDependencies() { }

        protected virtual void BeforeInitialize() { }

        protected void AddViewToRegionByName(string regionName, string viewName, object userState = null)
        {
            FrameworkElement view = ViewRegistryService.ResolveView(viewName);

            ViewModelBase viewModelBase = view.DataContext as ViewModelBase;//PW.Windows.Contract.AssertIsType<ViewModelBase>(() => view.DataContext, view.DataContext);
            //PW.Windows.Contract.AssertNotNull(() => viewModelBase, viewModelBase, "Error for view : " + viewName);
            if (viewModelBase == null)
            {
                throw new InvalidOperationException("Error for view : " + viewName);
            }
            viewModelBase.Initialize(userState);

            var region = RegionManager.Regions[regionName];

            if (!region.Views.Contains(view))
                RegionManager.Regions[regionName].Add(view);

            ITransitionViewModel transitionViewModel = view.DataContext as ITransitionViewModel;
            if (transitionViewModel != null)
                transitionViewModel.BeginLoad();
        }

        protected void ClearRegion(string regionToClear)
        {
            var viewsToRemove = (from UserControl view in RegionManager.Regions[regionToClear].Views
                                 select view).ToArray();
            foreach (var view in viewsToRemove)
            {
                var viewModel = view.DataContext as ITransitionViewModel;
                if (viewModel == null)
                {
#warning To add call to Polaris Framework
                    //((ViewModelBase)view.DataContext).Removing();
                    RegionManager.Regions[regionToClear].Remove(view);
                }
                else
                {
                    if (!unloadingViews.ContainsKey(viewModel))
                    {
                        var observable = viewModel.GetTransitionCompleted(TransitionState.Unloaded);
                        var subscription = observable.Subscribe(this);
                        unloadingViews.Add(viewModel, new UnloadingView
                        {
                            RegionName = regionToClear,
                            Subscription = subscription,
                            View = view,
                        });
                    }
                    viewModel.BeginUnload();
                }
            }
        }

        public virtual void Start()
        {
            ClearRegion(CommonRegionNames.MainRegion);
            AddViewToRegionByName(CommonRegionNames.MainRegion, GlobalInstanceNames.StartupView);
        }

        public void DelayedStart(TimeSpan? delayTime = null)
        {
            if (delayTime == null) { delayTime = DEFAULT_DELAY; }
            Action startAction = Start;
            startAction.InvokeWithDelay(delayTime.Value, Application.Current.Dispatcher);
        }

        public virtual void OnCompleted()
        {
            //throw new NotImplementedException();
        }

        public virtual void OnError(Exception error)
        {
            //throw new NotImplementedException();
        }

        public virtual void OnNext(EventPattern<TransitionEventArgs> value)
        {
            var viewModel = value.Sender as ITransitionViewModel;
            if (viewModel == null || !unloadingViews.ContainsKey(viewModel))
                return;
            var unloadingView = unloadingViews[viewModel];
            unloadingViews.Remove(viewModel);
            unloadingView.Subscription.Dispose();
            RegionManager.Regions[unloadingView.RegionName].Remove(unloadingView.View);
        }
    }

    internal class UnloadingView
    {
        public UserControl View { get; set; }

        public IDisposable Subscription { get; set; }

        public string RegionName { get; set; }
    }
}
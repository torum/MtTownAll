using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using MtTownAll.Models;
using MtTownAll.ViewModels;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

namespace MtTownAll.Views;

public sealed partial class ShellPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    private Type? _currentPage;

    public ShellPage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();

        DataContext = ViewModel;

    }

    public void CallMeWhenMainWindowIsReady(MainWindow wnd)
    {
        wnd.SetTitleBar(AppTitleBar);

        //SetRegionsForCustomTitleBar();

        // Do this after everything is initilized even App.MainWnd in app.xaml.cs.
        //ViewModel.StartMPC(this.XamlRoot);
    }

    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        // This does not allways fire. We might need to use diffrent grid and use navigated event.
        // Update interactive regions (for backbutton) if the size of the window changes.
        //SetRegionsForCustomTitleBar();
    }

    private void SetRegionsForCustomTitleBar()
    {
        var m_AppWindow = App.MainWnd?.AppWindow;

        if (m_AppWindow is null)
        {
            return;
        }

        if (App.MainWnd?.ExtendsContentIntoTitleBar != true)
        {
            return;
        }

        var scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;

        //
        /*
        GeneralTransform transform = this.SearchBox.TransformToVisual(null);
        Rect bounds = transform.TransformBounds(new Rect(0, 0,
                                                         this.SearchBox.ActualWidth,
                                                         this.SearchBox.ActualHeight));
        Windows.Graphics.RectInt32 SearchBoxRect = GetRect(bounds, scaleAdjustment);

        GeneralTransform transform = this.DummyButton.TransformToVisual(null);
        Rect bounds = transform.TransformBounds(new Rect(0, 0,
                                                         this.DummyButton.ActualWidth,
                                                         this.DummyButton.ActualHeight));
        Windows.Graphics.RectInt32 DummyButtonRect = GetRect(bounds, scaleAdjustment);
        */


        // Settings button
        var transform1 = this.SettingsButton.TransformToVisual(null);
        var bounds1 = transform1.TransformBounds(new Rect(0, 0,
                                                         this.SettingsButton.ActualWidth,
                                                         this.SettingsButton.ActualHeight));
        Windows.Graphics.RectInt32 SettingsButton = GetRect(bounds1, scaleAdjustment);

        /*
        // Back button
        double width = this.BackButton.Width;//ActualWidth won't work in certain cases.
        double height = this.BackButton.Height;//ActualHeight won't work in certain cases.

        if (this.BackButton.Visibility != Visibility.Visible)
        {
            //Debug.WriteLine("BackButton.Visibility != Visibility.Visible");
            width = 0;
            height = 0;
        }

        GeneralTransform transform = this.BackButton.TransformToVisual(null);
        Rect bounds = transform.TransformBounds(new Rect(0, 0,
                                                    width,
                                                    height));
        Windows.Graphics.RectInt32 BackButtonRect = GetRect(bounds, scaleAdjustment);

        */
        //
        //var rectArray = new Windows.Graphics.RectInt32[] { SearchBoxRect, BackButtonRect };
        //var rectArray = new Windows.Graphics.RectInt32[] { BackButtonRect, SettingsButton };
        var rectArray = new Windows.Graphics.RectInt32[] { SettingsButton };


        InputNonClientPointerSource nonClientInputSrc =
            InputNonClientPointerSource.GetForWindowId(m_AppWindow.Id);
        nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
    }

    private static Windows.Graphics.RectInt32 GetRect(Rect bounds, double scale)
    {
        return new Windows.Graphics.RectInt32(
            _X: (int)Math.Round(bounds.X * scale),
            _Y: (int)Math.Round(bounds.Y * scale),
            _Width: (int)Math.Round(bounds.Width * scale),
            _Height: (int)Math.Round(bounds.Height * scale)
        );
    }

    private void NaviView_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.NavigationFrame.Navigate(typeof(TestPage), this.NavigationFrame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom }))
        {
            _currentPage = typeof(TestPage);
            var queuePage = ViewModel.MainMenuItems.FirstOrDefault();
            if (queuePage != null)
            {
                queuePage.Selected = true;
            }
        }
    }

    private void NaviView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked == true)
        {
            if (this.NavigationFrame.Navigate(typeof(SettingsPage), this.NavigationFrame, args.RecommendedNavigationTransitionInfo))//, args.RecommendedNavigationTransitionInfo //new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft }
            {
                _currentPage = typeof(SettingsPage);

            }
            return;
        }

        /*


        if (_currentPage is null)
        {
            return;
        }
        ///

        */
    }

    private void NaviView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (sender is null)
        {
            return;
        }

        if (ViewModel is not MainViewModel vm)
        {
            return;
        }

        if (args.SelectedItem is NodeMenuTest)
        {
            if (_currentPage == typeof(TestPage))
            {
                return;
            }

            if (this.NavigationFrame.Navigate(typeof(TestPage), this.NavigationFrame, args.RecommendedNavigationTransitionInfo))//, args.RecommendedNavigationTransitionInfo //new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft }
            {
                _currentPage = typeof(TestPage);
                vm.SelectedNodeMenu = args.SelectedItem as NodeTree;
            }
        }
        else if (args.SelectedItem is NodeMenuPrefecture)
        {
            if (_currentPage == typeof(PrefecturePage))
            {
                return;
            }
            if (this.NavigationFrame.Navigate(typeof(PrefecturePage), this.NavigationFrame, args.RecommendedNavigationTransitionInfo))
            {
                _currentPage = typeof(PrefecturePage);
                vm.SelectedNodeMenu = args.SelectedItem as NodeTree;
            }
        }
        else if (args.SelectedItem is NodeMenuTownAll)
        {
            if (_currentPage == typeof(TownAllPage))
            {
                return;
            }
            if (this.NavigationFrame.Navigate(typeof(TownAllPage), this.NavigationFrame, args.RecommendedNavigationTransitionInfo))
            {
                _currentPage = typeof(TownAllPage);
                vm.SelectedNodeMenu = args.SelectedItem as NodeTree;
            }
        }
        else if (args.SelectedItem is NodeMenuPostalCode)
        {
            if (_currentPage == typeof(PostalCodePage))
            {
                return;
            }
            if (this.NavigationFrame.Navigate(typeof(PostalCodePage), this.NavigationFrame, args.RecommendedNavigationTransitionInfo))
            {
                _currentPage = typeof(PostalCodePage);
                vm.SelectedNodeMenu = args.SelectedItem as NodeTree;
            }
        }
        else
        {
            // clear vm selected just in case.
            vm.SelectedNodeMenu = null;
        }
    }
}

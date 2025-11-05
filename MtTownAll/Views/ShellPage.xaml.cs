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
    }


    private void NaviView_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.NavigationFrame.Navigate(typeof(TestPage), this.NavigationFrame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom }))
        {
            _currentPage = typeof(TestPage);
            var queuePage = ViewModel.MainMenuItems.FirstOrDefault();
            queuePage?.Selected = true;
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
        else if (args.SelectedItem is NodeMenuRailLine)
        {
            if (_currentPage == typeof(RailLinePage))
            {
                return;
            }
            if (this.NavigationFrame.Navigate(typeof(RailLinePage), this.NavigationFrame, args.RecommendedNavigationTransitionInfo))
            {
                _currentPage = typeof(RailLinePage);
                vm.SelectedNodeMenu = args.SelectedItem as NodeTree;
            }
        }
        else if (args.SelectedItem is NodeMenuRailStation)
        {
            if (_currentPage == typeof(RailStationPage))
            {
                return;
            }
            if (this.NavigationFrame.Navigate(typeof(RailStationPage), this.NavigationFrame, args.RecommendedNavigationTransitionInfo))
            {
                _currentPage = typeof(RailStationPage);
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

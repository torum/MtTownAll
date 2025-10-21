using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MtTownAll.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MtTownAll.Views;

public sealed partial class MainWindow : Window
{
    private Microsoft.UI.Dispatching.DispatcherQueue? _currentDispatcherQueue;// = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
    public Microsoft.UI.Dispatching.DispatcherQueue? CurrentDispatcherQueue => _currentDispatcherQueue;

    public readonly CancellationTokenSource Cts = new();

    private readonly MainViewModel _vm;

    private readonly ElementTheme theme = ElementTheme.Default;

    public MainWindow()
    {
        // This DispatcherQueue should be alive as long as MainWindow is alive. Make sure to clear when the window is closed.
        _currentDispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();

        // For reading shift-jis csv.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        _vm = App.GetService<MainViewModel>();

        InitializeComponent();

        this.ExtendsContentIntoTitleBar = true;

        if (this.AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.PreferredMinimumWidth = 500;
            presenter.PreferredMinimumHeight = 350;
        }

        this.Content = App.GetService<ShellPage>();

        if (this.Content is ShellPage root)
        {
            root.RequestedTheme = theme;

            //SetCapitionButtonColor();

            root.CallMeWhenMainWindowIsReady(this);
        }
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        _currentDispatcherQueue = null;

        Cts.Cancel();
        Cts.Dispose();
    }
}

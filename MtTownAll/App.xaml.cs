using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using MtTownAll.Services;
using MtTownAll.Services.Contracts;
using MtTownAll.ViewModels;
using MtTownAll.Views;
using WinRT.Interop;


namespace MtTownAll;

public partial class App : Application
{
    public static MainWindow? MainWnd
    {
        get; private set;
    }

    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Core Services
            services.AddSingleton<IMtPrefAllDataService, MtPrefAllDataService>();
            services.AddSingleton<IMtTownAllDataService, MtTownAllDataService>();
            services.AddSingleton<IXKenAllDataService, XKenAllDataService>();
            services.AddSingleton<IRailLineDataService, RailLineDataService>();
            services.AddSingleton<IRailStationDataService, RailStationDataService>();
            // Views and ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ShellPage>();
        }).
        Build();

        //Microsoft.UI.Xaml.Application.Current.UnhandledException += App_UnhandledException;
        //TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
    }

    protected async override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        // Single instance.
        // https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/guides/applifecycle
        var mainInstance = Microsoft.Windows.AppLifecycle.AppInstance.FindOrRegisterForKey("MPDCtrlMain");
        // If the instance that's executing the OnLaunched handler right now
        // isn't the "main" instance.
        if (!mainInstance.IsCurrent)
        {
            // Redirect the activation (and args) to the "main" instance, and exit.
            var activatedEventArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();
            await mainInstance.RedirectActivationToAsync(activatedEventArgs);

            System.Diagnostics.Process.GetCurrentProcess().Kill();
            return;
        }
        else
        {
            // Otherwise, register for activation redirection
            Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().Activated += App_Activated;
        }

        MainWnd = App.GetService<MainWindow>();
        MainWnd.AppWindow.Show(true);
    }

    private void App_Activated(object? sender, Microsoft.Windows.AppLifecycle.AppActivationArguments e)
    {
        if (MainWnd is null)
        {
            return;
        }

        App.MainWnd?.DispatcherQueue?.TryEnqueue(() =>
        {
            if (MainWnd is null)
            {
                return;
            }

            MainWnd.Activate();

            var hWnd = WindowNative.GetWindowHandle(MainWnd);
            NativeMethods.ShowWindow(hWnd, NativeMethods.SW_RESTORE); // Ensure it's not minimized
            NativeMethods.SetForegroundWindow(hWnd); // Attempt to set it as the foreground window
        });
    }


    #region == BringToFront ==

    private static partial class NativeMethods
    {

        internal const int SW_RESTORE = 9; // Restores a minimized window and brings it to the foreground.

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool SetForegroundWindow(IntPtr hWnd);

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);

    }

    #endregion
}



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MtTownAll.Services;
using MtTownAll.Services.Contracts;
using MtTownAll.ViewModels;
using MtTownAll.Views;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.ApplicationSettings;
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
            //services.AddSingleton<IDialogService, DialogService>();

            // Views and ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ShellPage>();

            services.AddSingleton<TestPage>();
            services.AddSingleton<PrefecturePage>();
            services.AddSingleton<PostalCodePage>();
            services.AddSingleton<TownAllPage>(); 
            services.AddSingleton<SettingsPage>();


            // Configuration
            //services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
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

        //MainWnd = new(); // < No
        MainWnd = App.GetService<MainWindow>();

        // Too late here. In order to set themes, sets content in MainWindow constructor.
        //MainWnd.Content = App.GetService<ShellPage>();

        //MainWnd.Activate(); // Activate won't work..
        MainWnd.AppWindow.Show();

        //_window = new MainWindow();
        //_window.Activate();
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

            //MainWindow?.BringToFront();
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



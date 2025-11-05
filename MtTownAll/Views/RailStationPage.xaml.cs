using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.Storage.Pickers;
using MtTownAll.Models;
using MtTownAll.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MtTownAll.Views;

public sealed partial class RailStationPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public RailStationPage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();
    }
}

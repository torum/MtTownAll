using Microsoft.UI.Xaml.Controls;
using MtTownAll.ViewModels;

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

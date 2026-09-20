using Microsoft.UI.Xaml.Controls;
using MtTownAll.ViewModels;

namespace MtTownAll.Views;

public sealed partial class TownAllPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public TownAllPage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();
    }
}


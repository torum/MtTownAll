using Microsoft.UI.Xaml.Controls;
using MtTownAll.ViewModels;

namespace MtTownAll.Views;

public sealed partial class RailLinePage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public RailLinePage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();
    }
}

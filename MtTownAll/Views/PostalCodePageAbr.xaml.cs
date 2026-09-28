using Microsoft.UI.Xaml.Controls;
using MtTownAll.ViewModels;

namespace MtTownAll.Views;

public sealed partial class PostalCodeAbrPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public PostalCodeAbrPage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();
    }
}

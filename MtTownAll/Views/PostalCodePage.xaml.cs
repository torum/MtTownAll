using Microsoft.UI.Xaml.Controls;
using MtTownAll.ViewModels;

namespace MtTownAll.Views;

public sealed partial class PostalCodePage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public PostalCodePage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();
    }
}

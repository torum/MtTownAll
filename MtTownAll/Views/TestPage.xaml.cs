using Microsoft.UI.Xaml.Controls;
using MtTownAll.ViewModels;


namespace MtTownAll.Views;

public sealed partial class TestPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public TestPage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();

        DataContext = ViewModel;
    }
}

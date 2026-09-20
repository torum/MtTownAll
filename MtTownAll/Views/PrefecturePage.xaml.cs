using Microsoft.UI.Xaml.Controls;
using MtTownAll.ViewModels;

namespace MtTownAll.Views;

public sealed partial class PrefecturePage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public PrefecturePage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();

        DataContext = ViewModel;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MtTownAll.Models;
using MtTownAll.Services;
using MtTownAll.Services.Contracts;

namespace MtTownAll.ViewModels;

public sealed partial class MainViewModel : ObservableValidator
{
    #region == Gneric Properties ==

    private bool _isWorking;
    public bool IsWorking
    {
        get => _isWorking;
        set
        {
            if (_isWorking == value)
            {
                return;
            }

            _isWorking = value;
            OnPropertyChanged(nameof(IsWorking));
        }
    }

    private readonly MenuTreeBuilder _mainMenuItems = new("");
    public ObservableCollection<NodeTree> MainMenuItems
    {
        get => _mainMenuItems.Children;
        set
        {
            _mainMenuItems.Children = value;
            OnPropertyChanged(nameof(MainMenuItems));
        }
    }

    private NodeTree? _selectedNodeMenu = new NodeMenu("root");
    public NodeTree? SelectedNodeMenu
    {
        get => _selectedNodeMenu;
        set
        {
            if (_selectedNodeMenu == value)
            {
                return;
            }

            _selectedNodeMenu = value;
            OnPropertyChanged(nameof(SelectedNodeMenu));

            if (value is null)
            {
                App.MainWnd?.CurrentDispatcherQueue?.TryEnqueue(() =>
                {

                });

                return;
            }

            if (value is NodeMenuTownAll)
            {
                //
            }
            else if (value is NodeMenuPrefecture)
            {
                //
            }
            else if (value is NodeMenuPostalCode)
            {
                //
            }
            else if (value is NodeMenuTest)
            {
                //
            }
            else if (value is NodeMenu)
            {
                if (value.Name != "root")
                {
                    throw new NotImplementedException();
                }
            }

        }
    }

    #endregion

    #region == Postal Code ==

    private ObservableCollection<PostalCode> _postalCodeSource = [];
    public ObservableCollection<PostalCode> PostalCodeSource
    {
        get => _postalCodeSource;
        set
        {
            if (_postalCodeSource == value)
            {
                return;
            }

            _postalCodeSource = value;
            OnPropertyChanged(nameof(PostalCodeSource));
        }
    }

    #endregion

    #region == Town All ==

    private ObservableCollection<Town> _townAllSource = [];
    public ObservableCollection<Town> TownAllSource
    {
        get => _townAllSource;
        set
        {
            if (_townAllSource == value)
            {
                return;
            }

            _townAllSource = value;
            OnPropertyChanged(nameof(TownAllSource));
        }
    }

    #endregion

    #region == Test ==

    [ObservableProperty]
    public partial string ErrorMessages { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string PostalCodeErrorMessage { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool PostalCodeHasError { get; set; } = false;

    [ObservableProperty]
    public partial bool PostalCodeReturnedMultipleAddresses { get; set; } = false;

    public string FullAddress => $"{SelectedPrefecture?.PrefectureName} {SelectedSikuchouson}";

    private string postalCode = "000-0000";//string.Empty;

    //[Required(ErrorMessage = "郵便番号の入力は必須です。")]
    //[MinLength(7, ErrorMessage = "郵便番号は7桁で入力してください。")]
    //[MaxLength(8, ErrorMessage = "郵便番号は7桁以内で入力してください。")]
    [RegularExpression("^[0-9]{3}-?[0-9]{4}$", ErrorMessage = "郵便番号は半角英数のxxx-xxxxまたはxxxxxxxの形式で入力してください。")]
    public string PostalCode
    {
        get => postalCode;
        set
        {
            //SetProperty(ref postalCode, value, true);
            SetProperty(ref postalCode, value, false);
            //ValidateProperty(PostalCode, nameof(PostalCode));

            if (GetErrors(nameof(PostalCode)).Count() > 0)
            {
                PostalCodeErrorMessage = string.Join(", ", GetErrors(nameof(PostalCode)).Select(e => e.ErrorMessage));
                PostalCodeHasError = true;
            }
            else
            {
                PostalCodeErrorMessage = string.Empty;
                PostalCodeHasError = false;

                GetAddressesFromPostalCode(postalCode);
            }
        }
    }

    public ObservableCollection<string> MultiplePostalCodeAddresses { get; } = [];

    private string selectedMultiplePostalCodeAddresses = string.Empty;
    public string SelectedMultiplePostalCodeAddresses
    {
        get => selectedMultiplePostalCodeAddresses;
        set => SetProperty(ref selectedMultiplePostalCodeAddresses, value, false);
    }

    public ObservableCollection<Prefecture> PrefectureSource { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullAddress))]
    public partial Prefecture? SelectedPrefecture { get; set; }

    public ObservableCollection<string> SikuchousonSource { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullAddress))]
    public partial string? SelectedSikuchouson { get; set; }

    #endregion

    private readonly IPrefectureDataService _prefectureDataService;
    private readonly IPostalCodeDataService _postalCodeDataService;

    public MainViewModel(IPrefectureDataService prefectureDataService, IPostalCodeDataService postalCodeDataService)
    {
        _prefectureDataService = prefectureDataService;
        _postalCodeDataService = postalCodeDataService;

        PopulatePrefectures();

        ErrorsChanged += (sender, arg) => { this.UpdateErrorMessages(arg); };
    }

    private void UpdateErrorMessages(DataErrorsChangedEventArgs arg)
    {
        if (HasErrors)
            ErrorMessages = "入力項目にエラーがあります。";
        else
            ErrorMessages = string.Empty;


        //string message = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
        //Debug.WriteLine(message);
    }

    private async void PopulatePrefectures()
    {
        var data = await _prefectureDataService.GetPrefectureDataAsync();

        foreach (var item in data)
        {
            PrefectureSource.Add(item);
        }
    }

    private async void GetAddressesFromPostalCode(string value)
    {
        SelectedPrefecture = null;

        SikuchousonSource.Clear();

        MultiplePostalCodeAddresses.Clear();

        PostalCodeReturnedMultipleAddresses = false;

        var data = await _postalCodeDataService.GetPostalCodeDataAsync(value);

        if (data.Count() <= 0)
        {
            PostalCodeReturnedMultipleAddresses = false;

            // show error message?

            return;
        }
        else if (data.Count() > 1)
        {
            PostalCodeReturnedMultipleAddresses = true;
        }
        else
        {
            PostalCodeReturnedMultipleAddresses = false;
        }

        var i = 0;
        foreach (var item in data)
        {
            MultiplePostalCodeAddresses.Add(item.PrefectureName + item.SikuchousonName + item.ChouikiName);

            //SikuchousonDataSource.Add(item.SikuchousonName);

            if (i == 0)
            {
                //SelectedPrefecture = PrefectureDataSource.Where(x => x.PrefectureName == item.PrefectureName).FirstOrDefault();
                //SelectedSikuchouson = SikuchousonDataSource.FirstOrDefault();//item.SikuchousonName;
            }

            i++;
        }

    }
}

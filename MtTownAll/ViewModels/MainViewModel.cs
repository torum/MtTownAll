using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;
using Microsoft.Windows.Storage.Pickers;
using MtTownAll.Models;
using MtTownAll.Services;
using MtTownAll.Services.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

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

    #region == KenAll (Postal Code) ==

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

    #region == MtTownAll ==

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

    #region == MtPrefAll ==

    public ObservableCollection<Prefecture> PrefectureSource { get; } = [];

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

            if (GetErrors(nameof(PostalCode)).Any())
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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullAddress))]
    public partial Prefecture? SelectedPrefecture { get; set; }

    public ObservableCollection<string> SikuchousonSource { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullAddress))]
    public partial string? SelectedSikuchouson { get; set; }

    #endregion

    // DB
    private static string DataBaseFilePath => System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + "Address.db";
    private SqliteConnectionStringBuilder connectionStringBuilder;

    // Services
    private readonly IMtPrefAllDataService _prefectureDataService;
    private readonly IMtTownAllDataService _townDataService;
    private readonly IXKenAllDataService _postalCodeDataService;

    public MainViewModel(IMtPrefAllDataService prefectureDataService, IXKenAllDataService postalCodeDataService, IMtTownAllDataService townDataService)
    {
        _prefectureDataService = prefectureDataService;
        _townDataService = townDataService;
        _postalCodeDataService = postalCodeDataService;

        connectionStringBuilder = new SqliteConnectionStringBuilder($"Data Source={DataBaseFilePath};Pooling=false"); // Set Pooling=false so that the app does not hold a file lock.

        PopulatePrefectures();

        ErrorsChanged += (sender, arg) => { this.UpdateErrorMessages(arg); };
    }

    private void UpdateErrorMessages(DataErrorsChangedEventArgs arg)
    {
        Debug.WriteLine($"UpdateErrorMessages {arg}");

        if (HasErrors)
            ErrorMessages = "入力項目にエラーがあります。";
        else
            ErrorMessages = string.Empty;

        string message = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
        Debug.WriteLine(message);
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

        // TODO:
        connectionStringBuilder = new SqliteConnectionStringBuilder($"Data Source={DataBaseFilePath};Pooling=false");// Set Pooling=false so that the app does not hold a file lock.

        var data = await _postalCodeDataService.SelectAddressesByPostalCodeAsync(connectionStringBuilder, value);

        if (!data.Any())
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

    #region == Commands ==

    [RelayCommand]
    public async Task FileKenAllOpen()
    {
        PostalCodeSource.Clear();

        if (App.MainWnd is null)
        {
            return;
        }

        var filePicker = new FileOpenPicker(App.MainWnd.AppWindow.Id);

        // x-ken-all.csv
        filePicker.FileTypeFilter.Add(".csv");
        filePicker.SuggestedStartLocation = PickerLocationId.Desktop;

        var file = await filePicker.PickSingleFileAsync();

        if (file == null)
        {
            return;
        }
        
        IsWorking = true;

        var kenAll = await Task.Run(() => _postalCodeDataService.ParseXKenAllCsv(file.Path), App.MainWnd.Cts.Token);

        PostalCodeSource = kenAll;
        
        IsWorking = false;

        InsertAllIntoXKenAllTableCommand.NotifyCanExecuteChanged();
    }


    [RelayCommand]
    public async Task FileTownAllOpen()
    {
        TownAllSource.Clear();

        if (App.MainWnd is null)
        {
            return;
        }

        var filePicker = new FileOpenPicker(App.MainWnd.AppWindow.Id);

        // mt_town_all.csv
        filePicker.FileTypeFilter.Add(".csv");
        filePicker.SuggestedStartLocation = PickerLocationId.Desktop;

        var file = await filePicker.PickSingleFileAsync();

        if (file == null)
        {
            return;
        }

        IsWorking = true;

        var townAll = await Task.Run(() => _townDataService.ParseMtTownAllCsv(file.Path), App.MainWnd.Cts.Token);

        TownAllSource = townAll;

        IsWorking = false;

        InsertAllIntoMtTownAllTableCommand.NotifyCanExecuteChanged();
    }


    [RelayCommand(CanExecute = nameof(CanInsertAllIntoXKenAllTable))]
    public async Task InsertAllIntoXKenAllTable()
    {
        if (App.MainWnd is null)
        {
            return;
        }

        if (PostalCodeSource is null)
        {
            return;
        }

        if (PostalCodeSource.Count <= 0)
        {
            return;
        }

        var savePicker = new Microsoft.Windows.Storage.Pickers.FileSavePicker(App.MainWnd.AppWindow.Id)
        {
            SuggestedStartLocation = PickerLocationId.Desktop,
            SuggestedFileName = "x-ken-all"
        };
        savePicker.FileTypeChoices.Add("SQLite Database", [".db"]);

        var result = await savePicker.PickSaveFileAsync();
        if (result is not null)
        {
            connectionStringBuilder = new SqliteConnectionStringBuilder($"Data Source={result.Path};Pooling=false");// Set Pooling=false so that the app does not hold a file lock.

            try
            {
                IsWorking = true;

                var ret = await Task.Run(() => _postalCodeDataService.InsertAllXKenAllData(connectionStringBuilder, PostalCodeSource), App.MainWnd.Cts.Token);
                // TODO: error check.

                IsWorking = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception @CanInsertAllIntoXKenAllTable(): {ex}");

                IsWorking = false;
            }
        }
    }

    private bool CanInsertAllIntoXKenAllTable()
    {
        if (PostalCodeSource is null)
        {
            return false;
        }

        if (PostalCodeSource.Count <= 0)
        {
            return false;   
        }

        return true;
    }

    [RelayCommand(CanExecute = nameof(CanInsertAllIntoMtPrefAllTable))]
    public async Task InsertAllIntoMtPrefAllTable()
    {
        if (App.MainWnd is null)
        {
            return;
        }

        if (PrefectureSource is null)
        {
            return;
        }

        if (PrefectureSource.Count <= 0)
        {
            return;
        }

        var savePicker = new Microsoft.Windows.Storage.Pickers.FileSavePicker(App.MainWnd.AppWindow.Id)
        {
            SuggestedStartLocation = PickerLocationId.Desktop,
            SuggestedFileName = "mt_pref_all"
        };
        savePicker.FileTypeChoices.Add("SQLite Database", [".db"]);

        var result = await savePicker.PickSaveFileAsync();
        if (result is not null)
        {
            connectionStringBuilder = new SqliteConnectionStringBuilder($"Data Source={result.Path};Pooling=false");// Set Pooling=false so that the app does not hold a file lock.

            try
            {
                IsWorking = true;

                var ret = await Task.Run(() => _prefectureDataService.InsertAllPrefectureData(connectionStringBuilder, PrefectureSource), App.MainWnd.Cts.Token);
                // TODO: error check.

                IsWorking = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception @InsertAllIntoMtPrefAllTable(): {ex}");

                IsWorking = false;
            }
        }
    }

    private bool CanInsertAllIntoMtPrefAllTable()
    {
        if (PrefectureSource is null)
        {
            return false;
        }

        if (PrefectureSource.Count <= 0)
        {
            return false;
        }

        return true;
    }

    [RelayCommand(CanExecute = nameof(CanInsertAllIntoMtTownAllTable))]
    public async Task InsertAllIntoMtTownAllTable()
    {
        if (App.MainWnd is null)
        {
            return;
        }

        if (TownAllSource is null)
        {
            return;
        }

        if (TownAllSource.Count <= 0)
        {
            return;
        }

        var savePicker = new Microsoft.Windows.Storage.Pickers.FileSavePicker(App.MainWnd.AppWindow.Id)
        {
            SuggestedStartLocation = PickerLocationId.Desktop,
            SuggestedFileName = "mt_town_all"
        };
        savePicker.FileTypeChoices.Add("SQLite Database", [".db"]);

        var result = await savePicker.PickSaveFileAsync();
        if (result is not null)
        {
            connectionStringBuilder = new SqliteConnectionStringBuilder($"Data Source={result.Path};Pooling=false");// Set Pooling=false so that the app does not hold a file lock.

            try
            {
                IsWorking = true;

                var ret = await Task.Run(() => _townDataService.InsertAllTownData(connectionStringBuilder, TownAllSource), App.MainWnd.Cts.Token);
                // TODO: error check.

                IsWorking = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception @InsertAllIntoMtTownAllTable(): {ex}");

                IsWorking = false;
            }
        }
    }

    private bool CanInsertAllIntoMtTownAllTable()
    {
        if (TownAllSource is null)
        {
            return false;
        }

        if (TownAllSource.Count <= 0)
        {
            return false;
        }

        return true;
    }


    #endregion
}

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
            else if (value is NodeMenuRailLine)
            {
                //
            }
            else if (value is NodeMenuRailStation)
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

    #region == InfoBars ==

    //
    private string _infoBarInfoTitleXKenAll = "";
    public string InfoBarInfoTitleXKenAll
    {
        get
        {
            return _infoBarInfoTitleXKenAll;
        }
        set
        {
            _infoBarInfoTitleXKenAll = value;
            OnPropertyChanged(nameof(InfoBarInfoTitleXKenAll));
        }
    }

    private string _infoBarInfoMessageXKenAll = "";
    public string InfoBarInfoMessageXKenAll
    {
        get
        {
            return _infoBarInfoMessageXKenAll;
        }
        set
        {
            _infoBarInfoMessageXKenAll = value;
            OnPropertyChanged(nameof(InfoBarInfoMessageXKenAll));
        }
    }

    private bool _isShowInfoWindowXKenAll;
    public bool IsShowInfoWindowXKenAll

    {
        get { return _isShowInfoWindowXKenAll; }
        set
        {
            if (_isShowInfoWindowXKenAll == value)
                return;

            _isShowInfoWindowXKenAll = value;

            if (!_isShowInfoWindowXKenAll)
            {
                InfoBarInfoTitleXKenAll = string.Empty;
                InfoBarInfoMessageXKenAll = string.Empty;
            }

            OnPropertyChanged(nameof(IsShowInfoWindowXKenAll));
        }
    }

    //
    private string _infoBarInfoTitleMtTownAll = "";
    public string InfoBarInfoTitleMtTownAll
    {
        get
        {
            return _infoBarInfoTitleMtTownAll;
        }
        set
        {
            _infoBarInfoTitleMtTownAll = value;
            OnPropertyChanged(nameof(InfoBarInfoTitleMtTownAll));
        }
    }

    private string _infoBarInfoMessageMtTownAll = "";
    public string InfoBarInfoMessageMtTownAll
    {
        get
        {
            return _infoBarInfoMessageMtTownAll;
        }
        set
        {
            _infoBarInfoMessageMtTownAll = value;
            OnPropertyChanged(nameof(InfoBarInfoMessageMtTownAll));
        }
    }

    private bool _isShowInfoWindowMtTownAll;
    public bool IsShowInfoWindowMtTownAll

    {
        get { return _isShowInfoWindowMtTownAll; }
        set
        {
            if (_isShowInfoWindowMtTownAll == value)
                return;

            _isShowInfoWindowMtTownAll = value;

            if (!_isShowInfoWindowMtTownAll)
            {
                InfoBarInfoTitleMtTownAll = string.Empty;
                InfoBarInfoMessageMtTownAll = string.Empty;
            }

            OnPropertyChanged(nameof(IsShowInfoWindowMtTownAll));
        }
    }

    //
    private string _infoBarInfoTitleRailLine = "";
    public string InfoBarInfoTitleRailLine
    {
        get
        {
            return _infoBarInfoTitleRailLine;
        }
        set
        {
            _infoBarInfoTitleRailLine = value;
            OnPropertyChanged(nameof(InfoBarInfoTitleRailLine));
        }
    }

    private string _infoBarInfoMessageRailLine = "";
    public string InfoBarInfoMessageRailLine
    {
        get
        {
            return _infoBarInfoMessageRailLine;
        }
        set
        {
            _infoBarInfoMessageRailLine = value;
            OnPropertyChanged(nameof(InfoBarInfoMessageRailLine));
        }
    }

    private bool _isShowInfoWindowRailLine;
    public bool IsShowInfoWindowRailLine

    {
        get { return _isShowInfoWindowRailLine; }
        set
        {
            if (_isShowInfoWindowRailLine == value)
                return;

            _isShowInfoWindowRailLine = value;

            if (!_isShowInfoWindowRailLine)
            {
                InfoBarInfoTitleRailLine = string.Empty;
                InfoBarInfoMessageRailLine = string.Empty;
            }

            OnPropertyChanged(nameof(IsShowInfoWindowRailLine));
        }
    }

    //
    private string _infoBarInfoTitleRailStation = "";
    public string InfoBarInfoTitleRailStation
    {
        get
        {
            return _infoBarInfoTitleRailStation;
        }
        set
        {
            _infoBarInfoTitleRailStation = value;
            OnPropertyChanged(nameof(InfoBarInfoTitleRailStation));
        }
    }

    private string _infoBarInfoMessageRailStation = "";
    public string InfoBarInfoMessageRailStation
    {
        get
        {
            return _infoBarInfoMessageRailStation;
        }
        set
        {
            _infoBarInfoMessageRailStation = value;
            OnPropertyChanged(nameof(InfoBarInfoMessageRailStation));
        }
    }

    private bool _isShowInfoWindowRailStation;
    public bool IsShowInfoWindowRailStation

    {
        get { return _isShowInfoWindowRailStation; }
        set
        {
            if (_isShowInfoWindowRailStation == value)
                return;

            _isShowInfoWindowRailStation = value;

            if (!_isShowInfoWindowRailStation)
            {
                InfoBarInfoTitleRailStation = string.Empty;
                InfoBarInfoMessageRailStation = string.Empty;
            }

            OnPropertyChanged(nameof(IsShowInfoWindowRailStation));
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

    #region == RailLine ==

    private ObservableCollection<RailLine> _railLineSource = [];
    public ObservableCollection<RailLine> RailLineSource
    {
        get => _railLineSource;
        set
        {
            if (_railLineSource == value)
            {
                return;
            }

            _railLineSource = value;
            OnPropertyChanged(nameof(RailLineSource));
        }
    }

    #endregion

    #region == RailStation ==

    private ObservableCollection<RailStation> _railStationeSource = [];
    public ObservableCollection<RailStation> RailStationSource
    {
        get => _railStationeSource;
        set
        {
            if (_railStationeSource == value)
            {
                return;
            }

            _railStationeSource = value;
            OnPropertyChanged(nameof(RailStationSource));
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
    private readonly IRailLineDataService _railLineDataService;
    private readonly IRailStationDataService _railStationDataService;

    public MainViewModel(IMtPrefAllDataService prefectureDataService, IXKenAllDataService postalCodeDataService, IMtTownAllDataService townDataService, IRailLineDataService railLineDataService, IRailStationDataService railStationDataService)
    {
        _prefectureDataService = prefectureDataService;
        _townDataService = townDataService;
        _postalCodeDataService = postalCodeDataService;
        _railLineDataService = railLineDataService;
        _railStationDataService = railStationDataService;

        connectionStringBuilder = new SqliteConnectionStringBuilder($"Data Source={DataBaseFilePath};Pooling=false"); // Set Pooling=false so that the app does not hold a file lock.

        PopulatePrefectures();

        ErrorsChanged += (sender, arg) => { this.UpdateErrorMessages(arg); };
        _railStationDataService = railStationDataService;
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

        IsShowInfoWindowXKenAll = false;

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

        try
        {
            var kenAll = await Task.Run(() => _postalCodeDataService.ParseXKenAllCsv(file.Path), App.MainWnd.Cts.Token);

            PostalCodeSource = kenAll;

            if (kenAll.Count == 0)
            {
                InfoBarInfoTitleXKenAll = "CSVファイルの読み込み失敗";
                InfoBarInfoMessageXKenAll = "選択したファイルが「x-ken-all.csv」かどうか確認してください。";
                IsShowInfoWindowXKenAll = true;
            }
        }
        catch (CsvHelper.MissingFieldException ex)
        {
            Debug.WriteLine($"CsvHelper.MissingFieldException @FileKenAllOpen {ex}");

            InfoBarInfoTitleXKenAll = "CSVファイルの読み込みでエラー";
            InfoBarInfoMessageXKenAll = "カラム数が違います。選択したファイルが「x-ken-all.csv」かどうか確認してください。";
            IsShowInfoWindowXKenAll = true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception @FileKenAllOpen {ex}");

            InfoBarInfoTitleXKenAll = "CSVファイルの読み込みでエラー";
            InfoBarInfoMessageXKenAll = $"{ex}";
            IsShowInfoWindowXKenAll = true;
        }
        finally
        {
            IsWorking = false;
        }

        InsertAllIntoXKenAllTableCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    public async Task FileTownAllOpen()
    {
        TownAllSource.Clear();

        IsShowInfoWindowMtTownAll = false;

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

        try
        {
            var townAll = await Task.Run(() => _townDataService.ParseMtTownAllCsv(file.Path), App.MainWnd.Cts.Token);

            TownAllSource = townAll;

            if (townAll.Count == 0)
            {
                InfoBarInfoTitleMtTownAll = "CSVファイルの読み込み失敗";
                InfoBarInfoMessageMtTownAll = "選択したファイルが「mt_town_all.csv」かどうか確認してください。";
                IsShowInfoWindowMtTownAll = true;
            }
        }
        catch (CsvHelper.MissingFieldException ex)
        {
            Debug.WriteLine($"CsvHelper.MissingFieldException @FileTownAllOpen {ex}");
            InfoBarInfoTitleMtTownAll = "CSVファイルの読み込みでエラー";
            InfoBarInfoMessageMtTownAll = "カラム数が違います。選択したファイルが「mt_town_all.csv」かどうか確認してください。";
            IsShowInfoWindowMtTownAll = true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception @FileTownAllOpen {ex}");

            InfoBarInfoTitleMtTownAll = "CSVファイルの読み込みでエラー";
            InfoBarInfoMessageMtTownAll = $"{ex}";
            IsShowInfoWindowMtTownAll = true;
        }
        finally
        {
            IsWorking = false;
        }

        InsertAllIntoMtTownAllTableCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    public async Task FileRailLineOpen()
    {
        RailLineSource.Clear();

        IsShowInfoWindowRailLine = false;

        if (App.MainWnd is null)
        {
            return;
        }

        var filePicker = new FileOpenPicker(App.MainWnd.AppWindow.Id);

        filePicker.FileTypeFilter.Add(".csv");
        filePicker.SuggestedStartLocation = PickerLocationId.Desktop;

        var file = await filePicker.PickSingleFileAsync();

        if (file == null)
        {
            return;
        }

        IsWorking = true;

        try
        {
            var rline = await Task.Run(() => _railLineDataService.ParseRailLineCsv(file.Path), App.MainWnd.Cts.Token);

            RailLineSource = rline;

            if (rline.Count == 0)
            {
                InfoBarInfoTitleRailLine = "CSVファイルの読み込み失敗";
                InfoBarInfoMessageRailLine = "選択したファイルが「lineyyyymmddfree.csv」かどうか確認してください。";
                IsShowInfoWindowRailLine = true;
            }
        }
        catch (CsvHelper.MissingFieldException ex)
        {
            Debug.WriteLine($"CsvHelper.MissingFieldException @FileRailLineOpen {ex}");

            InfoBarInfoTitleRailLine = "CSVファイルの読み込みでエラー";
            InfoBarInfoMessageRailLine = "カラム数が違います。選択したファイルが「lineyyyymmddfree.csv」かどうか確認してください。";
            IsShowInfoWindowRailLine = true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception @FileRailLineOpen {ex}");

            InfoBarInfoTitleRailLine = "CSVファイルの読み込みでエラー";
            InfoBarInfoMessageRailLine = $"{ex}";
            IsShowInfoWindowRailLine = true;
        }
        finally
        {
            IsWorking = false;
        }

        InsertAllIntoRailLineTableCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    public async Task FileRailStataionOpen()
    {
        RailStationSource.Clear();

        IsShowInfoWindowRailStation = false;

        if (App.MainWnd is null)
        {
            return;
        }

        var filePicker = new FileOpenPicker(App.MainWnd.AppWindow.Id);

        filePicker.FileTypeFilter.Add(".csv");
        filePicker.SuggestedStartLocation = PickerLocationId.Desktop;

        var file = await filePicker.PickSingleFileAsync();

        if (file == null)
        {
            return;
        }

        IsWorking = true;

        try
        {
            var rStation = await Task.Run(() => _railStationDataService.ParseRailStationCsv(file.Path), App.MainWnd.Cts.Token);

            RailStationSource = rStation;

            if (rStation.Count == 0)
            {
                InfoBarInfoTitleRailStation = "CSVファイルの読み込み失敗";
                InfoBarInfoMessageRailStation = "選択したファイルが「stationyyyymmddfree.csv」かどうか確認してください。";
                IsShowInfoWindowRailStation = true;
            }
        }
        catch (CsvHelper.MissingFieldException ex)
        {
            Debug.WriteLine($"CsvHelper.MissingFieldException @FileRailStationOpen {ex}");

            InfoBarInfoTitleRailLine = "CSVファイルの読み込みでエラー";
            InfoBarInfoMessageRailLine = "カラム数が違います。選択したファイルが「stationyyyymmddfree.csv」かどうか確認してください。";
            IsShowInfoWindowRailLine = true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception @FileRailStationOpen {ex}");

            InfoBarInfoTitleRailStation = "CSVファイルの読み込みでエラー";
            InfoBarInfoMessageRailStation = $"{ex}";
            IsShowInfoWindowRailStation = true;
        }
        finally
        {
            IsWorking = false;
        }

        InsertAllIntoRailStationTableCommand.NotifyCanExecuteChanged();
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
                Debug.WriteLine($"Exception @InsertAllIntoXKenAllTable(): {ex}");

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

    [RelayCommand(CanExecute = nameof(CanInsertAllIntoRailLineTable))]
    public async Task InsertAllIntoRailLineTable()
    {
        if (App.MainWnd is null)
        {
            return;
        }
        
        if (RailLineSource is null)
        {
            return;
        }

        if (RailLineSource.Count <= 0)
        {
            return;
        }

        var savePicker = new Microsoft.Windows.Storage.Pickers.FileSavePicker(App.MainWnd.AppWindow.Id)
        {
            SuggestedStartLocation = PickerLocationId.Desktop,
            SuggestedFileName = "rail_lines"
        };
        savePicker.FileTypeChoices.Add("SQLite Database", [".db"]);

        var result = await savePicker.PickSaveFileAsync();
        if (result is not null)
        {
            connectionStringBuilder = new SqliteConnectionStringBuilder($"Data Source={result.Path};Pooling=false");// Set Pooling=false so that the app does not hold a file lock.

            try
            {
                IsWorking = true;

                var ret = await Task.Run(() => _railLineDataService.InsertAllRainLineData(connectionStringBuilder, RailLineSource), App.MainWnd.Cts.Token);
                // TODO: error check.

                IsWorking = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception @InsertAllIntoRailLineTable(): {ex}");

                IsWorking = false;
            }
        }

    }

    private bool CanInsertAllIntoRailLineTable()
    {
        if (RailLineSource is null)
        {
            return false;
        }

        if (RailLineSource.Count <= 0)
        {
            return false;
        }

        return true;
    }


    [RelayCommand(CanExecute = nameof(CanInsertAllIntoRailStationTable))]
    public async Task InsertAllIntoRailStationTable()
    {
        if (App.MainWnd is null)
        {
            return;
        }

        if (RailStationSource is null)
        {
            return;
        }

        if (RailStationSource.Count <= 0)
        {
            return;
        }

        var savePicker = new Microsoft.Windows.Storage.Pickers.FileSavePicker(App.MainWnd.AppWindow.Id)
        {
            SuggestedStartLocation = PickerLocationId.Desktop,
            SuggestedFileName = "rail_stations"
        };
        savePicker.FileTypeChoices.Add("SQLite Database", [".db"]);

        var result = await savePicker.PickSaveFileAsync();
        if (result is not null)
        {
            connectionStringBuilder = new SqliteConnectionStringBuilder($"Data Source={result.Path};Pooling=false");// Set Pooling=false so that the app does not hold a file lock.

            try
            {
                IsWorking = true;

                var ret = await Task.Run(() => _railStationDataService.InsertAllRailStationData(connectionStringBuilder, RailStationSource), App.MainWnd.Cts.Token);
                // TODO: error check.

                IsWorking = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception @InsertAllIntoRailStationTable(): {ex}");

                IsWorking = false;
            }
        }

    }

    private bool CanInsertAllIntoRailStationTable()
    {
        if (RailStationSource is null)
        {
            return false;
        }

        if (RailStationSource.Count <= 0)
        {
            return false;
        }

        return true;
    }


    #endregion
}

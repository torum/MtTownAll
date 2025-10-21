using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.Storage.Pickers;
using MtTownAll.Models;
using MtTownAll.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MtTownAll.Views;

public sealed partial class PostalCodePage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    private readonly SqliteConnectionStringBuilder connectionStringBuilder;

    // TODO:
    private string DataBaseFilePath => System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + "Address.db";

    public PostalCodePage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();

        DataContext = ViewModel;

        connectionStringBuilder = new SqliteConnectionStringBuilder("Data Source=" + DataBaseFilePath);
    }

    public void Save(object sender, RoutedEventArgs e)
    {
        if (ViewModel.PostalCodeSource.Count < 1)
        {
            return;
        }

        try
        {
            using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            try
            {
                connection.Open();

                using var tableCmd = connection.CreateCommand();

                tableCmd.Transaction = connection.BeginTransaction();
                try
                {
                    // Create table if not exists.
                    tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS postal_codes (" +
                        "municipality_code TEXT NOT NULL," +
                        "postal_code TEXT NOT NULL," + // PRIMARY KEY
                        "prefecture_name TEXT NOT NULL," +
                        "sikuchouson_name TEXT," +
                        "chouiki_name TEXT" +
                        ")";

                    tableCmd.ExecuteNonQuery();

                    // Insert data
                    foreach (var hoge in ViewModel.PostalCodeSource)
                    {
                        var sqlInsertIntoRent = String.Format(
    "INSERT INTO postal_codes " +
    "(municipality_code, postal_code, prefecture_name, sikuchouson_name, chouiki_name) " +
    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
    hoge.MunicipalityCode,
    hoge.Code,
    hoge.PrefectureName,
    hoge.SikuchousonName,
    hoge.ChouikiName);

                        tableCmd.CommandText = sqlInsertIntoRent;

                        var InsertIntoRentResult = tableCmd.ExecuteNonQuery();
                    }

                    tableCmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    tableCmd.Transaction.Rollback();

                    Debug.WriteLine("DB Error: " + ex.Message);
                }
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                Debug.WriteLine("DB Error: " + ex.Message);
                if (ex.InnerException != null)
                    throw ex.InnerException;
            }
            catch (System.InvalidOperationException ex)
            {
                Debug.WriteLine("DB Error: " + ex.Message);
                if (ex.InnerException != null)
                    throw ex.InnerException;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DB Error: " + ex.Message);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("DB Error: " + ex.Message);
        }


        Debug.WriteLine("Insert Done");
    }

    public async void FileOpen(object sender, RoutedEventArgs e)
    {
        ViewModel.PostalCodeSource.Clear();
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

        await Task.Run(() => ParseKenAllCsv(file.Path), App.MainWnd.Cts.Token);
    }

    private void ParseKenAllCsv(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        App.MainWnd?.CurrentDispatcherQueue?.TryEnqueue(() =>
        {
            ViewModel.IsWorking = true;
        });
        
        var _kenAll = new ObservableCollection<PostalCode>();

        var config = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            HasHeaderRecord = false,
            Encoding = Encoding.GetEncoding("shift_jis")
        };
        using var reader = new StreamReader(filePath, Encoding.GetEncoding("shift_jis"));
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<PostalCodeClassMapper>();

            var records = csv.GetRecords<PostalCode>();

            foreach (var record in records)
            {
                var obj = new PostalCode
                {
                    PrefectureName = record.PrefectureName,
                    ChouikiName = record.ChouikiName,
                    SikuchousonName = record.SikuchousonName,
                    MunicipalityCode = record.MunicipalityCode,
                    Code = record.Code
                };

                _kenAll.Add(obj);
            }
        }

        App.MainWnd?.CurrentDispatcherQueue?.TryEnqueue(() =>
        {
            if (App.MainWnd.Cts.IsCancellationRequested)
            {
                Debug.WriteLine("IsCancellationRequested before update PostalCodeSource @ParseKenAllCsv");
                return;
            }

            ViewModel.PostalCodeSource = _kenAll;
            ViewModel.IsWorking = false;
        });

        Debug.WriteLine("Open Done");
    }

    class PostalCodeMapper : CsvHelper.Configuration.ClassMap<PostalCode>
    {
        public PostalCodeMapper()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }

    class PostalCodeClassMapper : CsvHelper.Configuration.ClassMap<PostalCode>
    {
        public PostalCodeClassMapper()
        {
            Map(x => x.MunicipalityCode).Index(0);
            Map(x => x.Code).Index(2);
            Map(x => x.PrefectureName).Index(6);
            Map(x => x.SikuchousonName).Index(7);
            Map(x => x.ChouikiName).Index(8);
        }
    }
}

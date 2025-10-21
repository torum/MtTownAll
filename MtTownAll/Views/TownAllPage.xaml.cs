using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
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

        DataContext = ViewModel;
    }

    public async void FileOpen(object sender, RoutedEventArgs e)
    {
        ViewModel.TownAllSource.Clear();

        if (App.MainWnd is null)
        {
            return;
        }

        var filePicker = new FileOpenPicker(App.MainWnd.AppWindow.Id);
        filePicker.FileTypeFilter.Add(".csv");
        filePicker.SuggestedStartLocation = PickerLocationId.Desktop;

        var file = await filePicker.PickSingleFileAsync();

        if (file is null)
        {
            return;
        }

        await Task.Run(() => ParseTownAllCsv(file.Path), App.MainWnd.Cts.Token);
    }

    private void ParseTownAllCsv(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        App.MainWnd?.CurrentDispatcherQueue?.TryEnqueue(() =>
        {
            ViewModel.IsWorking = true;
        });

        var _townAll = new ObservableCollection<Town>();

        var config = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            HasHeaderRecord = true,
            Encoding = Encoding.UTF8,
        };

        using var reader = new StreamReader(filePath, Encoding.UTF8);
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<TownCodeClassMapper>();

            var records = csv.GetRecords<Town>();

            foreach (var record in records)
            {
                if (App.MainWnd is null)
                {
                    Debug.WriteLine("(App.MainWnd is null) @ParseTownAllCsv");
                    break;
                }

                if (App.MainWnd.Cts.IsCancellationRequested)
                {
                    Debug.WriteLine("IsCancellationRequested in foreach @ParseTownAllCsv");
                    break;
                }

                Town obj = new();
                obj.PrefectureName = record.PrefectureName;
                obj.TownName = record.TownName;
                obj.CountyName = record.CountyName;
                obj.Choume = record.Choume;
                obj.SikuchousonName = record.SikuchousonName;
                obj.PostalCode = record.PostalCode == "0" ? string.Empty : record.PostalCode;
                obj.MunicipalityCode = record.MunicipalityCode;
                obj.TownID = record.TownID;
                obj.ChouAzaType = record.ChouAzaType;
                obj.WardName = record.WardName;
                obj.KoazaName = record.KoazaName;

                _townAll.Add(obj);
            }
        }

        App.MainWnd?.CurrentDispatcherQueue?.TryEnqueue(() =>
        {
            if (App.MainWnd.Cts.IsCancellationRequested)
            {
                Debug.WriteLine("IsCancellationRequested before update TownAllSource @ParseTownAllCsv");
                return;
            }

            ViewModel.TownAllSource = _townAll;
            ViewModel.IsWorking = false;
        });

        Debug.WriteLine("Open Done");
    }


    class TownCodeMapper : CsvHelper.Configuration.ClassMap<Town>
    {
        public TownCodeMapper()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }

    class TownCodeClassMapper : CsvHelper.Configuration.ClassMap<Town>
    {
        /*
lg_code,machiaza_id,machiaza_type,pref,pref_kana,pref_roma,county,county_kana,county_roma,city,city_kana,city_roma,ward,ward_kana,ward_roma,oaza_cho,oaza_cho_kana,oaza_cho_roma,chome,chome_kana,chome_number,koaza,koaza_kana,koaza_roma,machiaza_dist,rsdt_addr_flg,rsdt_addr_mtd_code,oaza_cho_aka_flg,koaza_aka_code,oaza_cho_gsi_uncmn,koaza_gsi_uncmn,status_flg,wake_num_flg,efct_date,ablt_date,src_code,post_code,remarks
011011,0001001,2,–kŠC“¹,ƒzƒbƒJƒCƒhƒE,Hokkaido,,,,ŽD–yŽs,ƒTƒbƒ|ƒƒV,Sapporo-shi,’†‰›‹æ,ƒ`ƒ…ƒEƒIƒEƒN,Chuo-ku,ˆ®ƒP‹u,ƒAƒTƒqƒKƒIƒJ,Asahigaoka,‚P’š–Ú,‚Pƒ`ƒ‡ƒEƒ,1,,,,,1,1,0,0,0,0,1,1,1947-04-17,,0,,
132241,0013001,2,“Œ‹ž“s,ƒgƒEƒLƒ‡ƒEƒg,Tokyo,,,,‘½–€Žs,ƒ^ƒ}ƒV,Tama-shi,,,,¹ƒ–‹u,ƒqƒWƒŠƒKƒIƒJ,,‚P’š–Ú,‚Pƒ`ƒ‡ƒEƒ,1,,,,,0,0,0,0,0,0,1,1,1947-04-17,,0,,
         */

        public TownCodeClassMapper()
        {
            Map(x => x.MunicipalityCode).Index(0);
            Map(x => x.TownID).Index(1);
            Map(x => x.ChouAzaType).Index(2);
            Map(x => x.PrefectureName).Index(3);
            Map(x => x.CountyName).Index(6);
            Map(x => x.SikuchousonName).Index(9);
            Map(x => x.WardName).Index(12);
            Map(x => x.TownName).Index(15);
            Map(x => x.Choume).Index(18);
            Map(x => x.KoazaName).Index(21);
            Map(x => x.PostalCode).Index(35);
        }
    }
}


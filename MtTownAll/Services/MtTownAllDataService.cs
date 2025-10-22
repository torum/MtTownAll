using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using MtTownAll.Services;
using MtTownAll.Services.Contracts;
using MtTownAll.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtTownAll.Services;

public class MtTownAllDataService : IMtTownAllDataService
{
    public MtTownAllDataService()
    {
        //
    }

    public ObservableCollection<Town> ParseMtTownAllCsv(string filePath)
    {
        var townAll = new ObservableCollection<Town>();

        if (string.IsNullOrEmpty(filePath))
        {
            return townAll;
        }

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
                if (csv.ColumnCount != 38)
                {
                    Debug.WriteLine($"if (csv.ColumnCount != 38) @ParseMtTownAllCsv {csv.ColumnCount}");

                    // TODO: return error code or msg.
                    //return;
                    continue;
                }

                if (App.MainWnd is null)
                {
                    Debug.WriteLine("(App.MainWnd is null) @ParseMtTownAllCsv");
                    break;
                }

                if (App.MainWnd.Cts.IsCancellationRequested)
                {
                    Debug.WriteLine("IsCancellationRequested in foreach @ParseMtTownAllCsv");
                    break;
                }

                Town obj = new()
                {
                    PrefectureName = record.PrefectureName,
                    TownName = record.TownName,
                    CountyName = record.CountyName,
                    Choume = record.Choume,
                    SikuchousonName = record.SikuchousonName,
                    PostalCode = record.PostalCode,//record.PostalCode == "0" ? string.Empty : record.PostalCode,
                    MunicipalityCode = record.MunicipalityCode,
                    TownID = record.TownID,
                    ChouAzaType = record.ChouAzaType,
                    WardName = record.WardName,
                    KoazaName = record.KoazaName
                };

                townAll.Add(obj);
            }
        }

        Debug.WriteLine("Open Done @ParseMtTownAllCsv in IMtTownAllDataService");

        return townAll;
    }

    class TownCodeMapper : CsvHelper.Configuration.ClassMap<Town>
    {
        public TownCodeMapper()
        {
            AutoMap(CultureInfo.CurrentCulture);
        }
    }

    class TownCodeClassMapper : CsvHelper.Configuration.ClassMap<Town>
    {
        /*
        lg_code,machiaza_id,machiaza_type,pref,pref_kana,pref_roma,county,county_kana,county_roma,city,city_kana,city_roma,ward,ward_kana,ward_roma,oaza_cho,oaza_cho_kana,oaza_cho_roma,chome,chome_kana,chome_number,koaza,koaza_kana,koaza_roma,machiaza_dist,rsdt_addr_flg,rsdt_addr_mtd_code,oaza_cho_aka_flg,koaza_aka_code,oaza_cho_gsi_uncmn,koaza_gsi_uncmn,status_flg,wake_num_flg,efct_date,ablt_date,src_code,post_code,remarks
        011011,0001001,2,北海道,ホッカイドウ,Hokkaido,,,,札幌市,サッポロシ,Sapporo-shi,中央区,チュウオウク,Chuo-ku,旭ケ丘,アサヒガオカ,Asahigaoka,１丁目,１チョウメ,1,,,,,1,1,0,0,0,0,1,1,1947-04-17,,0,,
        132241,0013001,2,東京都,トウキョウト,Tokyo,,,,多摩市,タマシ,Tama-shi,,,,聖ヶ丘,ヒジリガオカ,,１丁目,１チョウメ,1,,,,,0,0,0,0,0,0,1,1,1947-04-17,,0,,
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
            Map(x => x.PostalCode).Index(36);
        }
    }

    public bool InsertAllTownData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<Town> data)
    {
        if (!data.Any())
        {
            return false;
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
                    tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS mt_town_all (" +
                        "lg_code TEXT NOT NULL," +
                        "machiaza_id TEXT NOT NULL," + // PRIMARY KEY
                        "machiaza_type TEXT NOT NULL," +
                        "pref TEXT NOT NULL," +
                        "county TEXT," +
                        "city TEXT NOT NULL," +
                        "ward TEXT," +
                        "oaza_cho TEXT," +
                        "chome TEXT," +
                        "koaza TEXT," +
                        "post_code TEXT" +
                        ")";

                    tableCmd.ExecuteNonQuery();

                    // Insert data
                    foreach (var hoge in data)
                    {
                        var sqlInsertIntoRent = String.Format(
    "INSERT OR IGNORE INTO mt_town_all " +
    "(lg_code, machiaza_id, machiaza_type, pref, county, city, ward, oaza_cho, chome, koaza, post_code) " +
    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')",
    hoge.MunicipalityCode,
    hoge.TownID,
    hoge.ChouAzaType, 
    hoge.PrefectureName,
    hoge.CountyName,
    hoge.SikuchousonName,
    hoge.WardName,
    hoge.TownName,
    hoge.Choume,
    hoge.KoazaName,
    hoge.PostalCode);

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

        Debug.WriteLine("Insert Done @InsertAllTownData in IMtTownAllDataService");

        return true;
    }

}

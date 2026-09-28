using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace MtTownAll.Services.Contracts;

public class AbrPostalCodeService : IAbrPostalCodeService
{
    public ObservableCollection<AbrPostalCode> ParseAbrPostalCodeCsv(string filePath)
    {
        var result = new ObservableCollection<AbrPostalCode>();

        if (string.IsNullOrEmpty(filePath))
        {
            return result;
        }

        var config = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            HasHeaderRecord = true,
            Encoding = Encoding.UTF8,
        };

        using var reader = new StreamReader(filePath, Encoding.UTF8);
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<AbrPostalCodeClassMapper>();

            var records = csv.GetRecords<AbrPostalCode>();

            foreach (var record in records)
            {
                if (csv.ColumnCount != 38)
                {
                    Debug.WriteLine($"if (csv.ColumnCount != 38) @ParseAbrPostalCodeCsv {csv.ColumnCount}");

                    // TODO: return error code or msg.
                    return result;
                    //continue;
                }

                if (App.MainWnd is null)
                {
                    Debug.WriteLine("(App.MainWnd is null) @ParseAbrPostalCodeCsv");
                    break;
                }

                if (App.MainWnd.Cts.IsCancellationRequested)
                {
                    Debug.WriteLine("IsCancellationRequested in foreach @ParseAbrPostalCodeCsv");
                    break;
                }
                /*
                AbrPostalCode obj = new()
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

                result.Add(obj);
                */
            }
        }

        Debug.WriteLine("Open Done @ParseAbrPostalCodeCsv in AbrPostalCodeService");

        return result;
    }

    class AbrPostalCodeMapper : CsvHelper.Configuration.ClassMap<AbrPostalCode>
    {
        public AbrPostalCodeMapper()
        {
            AutoMap(CultureInfo.CurrentCulture);
        }
    }

    class AbrPostalCodeClassMapper : CsvHelper.Configuration.ClassMap<AbrPostalCode>
    {
        public AbrPostalCodeClassMapper()
        {
            Map(x => x.LgCode).Index(0);
            Map(x => x.MachiazaId).Index(1);
            Map(x => x.Pref).Index(2);
            Map(x => x.County).Index(3);
            Map(x => x.City).Index(4);
            Map(x => x.Ward).Index(5);
            Map(x => x.KyotoSt).Index(6);
            Map(x => x.OazaCho).Index(7);
            Map(x => x.Chome).Index(8);
            Map(x => x.Koaza).Index(9);
            Map(x => x.MachiazaDist).Index(10);
            Map(x => x.PostCode).Index(11);
            Map(x => x.AddDate).Index(12);
            Map(x => x.DltDate).Index(13);
        }
    }

    public bool InsertAbrPostalCodeData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<AbrPostalCode> data)
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
                    tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS abr_post_code (" +
                        "lg_code TEXT NOT NULL," +
                        "machiaza_id TEXT NOT NULL," + // NOT PRIMARY KEY. Can be empty.
                        "pref TEXT NOT NULL," +
                        "county TEXT NOT NULL," +
                        "city TEXT," +
                        "ward TEXT NOT NULL," +
                        "kyoto_st TEXT," +
                        "oaza_cho TEXT," +
                        "chome TEXT," +
                        "koaza TEXT," +
                        "machiaza_dist TEXT," +
                        "post_code TEXT," +
                        "add_date TEXT," +
                        "dlt_date NOT TEXT" +
                        ")";

                    tableCmd.ExecuteNonQuery();

                    // Insert data
                    foreach (var hoge in data)
                    {
                        var sqlInsertIntoRent = String.Format(
    "INSERT OR IGNORE INTO mt_town_all " +
    "(lg_code, machiaza_id, pref, county, city, ward, kyoto_st, oaza_cho, chome, koaza, machiaza_dist, post_code, add_date, dlt_date) " +
    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}')",
    hoge.LgCode,
    hoge.MachiazaId,
    hoge.Pref,
    hoge.County,
    hoge.City,
    hoge.Ward,
    hoge.KyotoSt,
    hoge.OazaCho,
    hoge.Chome,
    hoge.Koaza,
    hoge.MachiazaDist,
    hoge.PostCode,
    hoge.AddDate,
    hoge.DltDate
    );

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


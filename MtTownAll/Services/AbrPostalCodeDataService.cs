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
using MtTownAll.Services.Contracts;

namespace MtTownAll.Services;

public class AbrPostalCodeDataService : IAbrPostalCodeDataService
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
                if (csv.ColumnCount != 14)
                {
                    Debug.WriteLine($"if (csv.ColumnCount != 14) @ParseAbrPostalCodeCsv {csv.ColumnCount}");

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

                AbrPostalCode obj = new()
                {
                    LgCode = record.LgCode,
                    MachiazaId = record.MachiazaId,
                    Pref = record.Pref,
                    County = record.County,
                    City = record.City,
                    Ward = record.Ward,
                    KyotoSt = record.KyotoSt,
                    OazaCho = record.OazaCho,
                    Chome = record.Chome,
                    Koaza = record.Koaza,
                    MachiazaDist = record.MachiazaDist,
                    PostCode = record.PostCode,//record.PostalCode == "0" ? string.Empty : record.PostalCode,
                    AddDate = record.AddDate,
                    DltDate = record.DltDate
                };

                result.Add(obj);
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
                        "dlt_date TEXT" +
                        ")";

                    tableCmd.ExecuteNonQuery();

                    /*
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
                    */
                    tableCmd.CommandText = "INSERT OR IGNORE INTO abr_post_code " +
                        "(lg_code, machiaza_id, pref, county, city, ward, kyoto_st, oaza_cho, chome, koaza, machiaza_dist, post_code, add_date, dlt_date) " +
                        "VALUES ($lg, $mach, $pref, $count, $city, $ward, $kyoto, $oaza, $chome, $koaza, $dist, $post, $add, $dlt)";

                    // Clear any existing parameters just in case
                    tableCmd.Parameters.Clear();

                    var lgParam = tableCmd.CreateParameter(); lgParam.ParameterName = "$lg"; tableCmd.Parameters.Add(lgParam);
                    var machParam = tableCmd.CreateParameter(); machParam.ParameterName = "$mach"; tableCmd.Parameters.Add(machParam);
                    var prefParam = tableCmd.CreateParameter(); prefParam.ParameterName = "$pref"; tableCmd.Parameters.Add(prefParam);
                    var countParam = tableCmd.CreateParameter(); countParam.ParameterName = "$count"; tableCmd.Parameters.Add(countParam);
                    var cityParam = tableCmd.CreateParameter(); cityParam.ParameterName = "$city"; tableCmd.Parameters.Add(cityParam);
                    var wardParam = tableCmd.CreateParameter(); wardParam.ParameterName = "$ward"; tableCmd.Parameters.Add(wardParam);
                    var kyotoParam = tableCmd.CreateParameter(); kyotoParam.ParameterName = "$kyoto"; tableCmd.Parameters.Add(kyotoParam);
                    var oazaParam = tableCmd.CreateParameter(); oazaParam.ParameterName = "$oaza"; tableCmd.Parameters.Add(oazaParam);
                    var chomeParam = tableCmd.CreateParameter(); chomeParam.ParameterName = "$chome"; tableCmd.Parameters.Add(chomeParam);
                    var koazaParam = tableCmd.CreateParameter(); koazaParam.ParameterName = "$koaza"; tableCmd.Parameters.Add(koazaParam);
                    var distParam = tableCmd.CreateParameter(); distParam.ParameterName = "$dist"; tableCmd.Parameters.Add(distParam);
                    var postParam = tableCmd.CreateParameter(); postParam.ParameterName = "$post"; tableCmd.Parameters.Add(postParam);
                    var addParam = tableCmd.CreateParameter(); addParam.ParameterName = "$add"; tableCmd.Parameters.Add(addParam);
                    var dltParam = tableCmd.CreateParameter(); dltParam.ParameterName = "$dlt"; tableCmd.Parameters.Add(dltParam);

                    // Insert data
                    foreach (var hoge in data)
                    {
                        lgParam.Value = hoge.LgCode ?? string.Empty;
                        machParam.Value = hoge.MachiazaId ?? string.Empty;
                        prefParam.Value = hoge.Pref ?? string.Empty;
                        countParam.Value = hoge.County ?? string.Empty;
                        cityParam.Value = hoge.City ?? string.Empty;
                        wardParam.Value = hoge.Ward ?? string.Empty;
                        kyotoParam.Value = hoge.KyotoSt ?? string.Empty;
                        oazaParam.Value = hoge.OazaCho ?? string.Empty;
                        chomeParam.Value = hoge.Chome ?? string.Empty;
                        koazaParam.Value = hoge.Koaza ?? string.Empty;
                        distParam.Value = hoge.MachiazaDist ?? string.Empty;
                        postParam.Value = hoge.PostCode ?? string.Empty;
                        addParam.Value = hoge.AddDate ?? string.Empty;
                        dltParam.Value = hoge.DltDate ?? string.Empty;

                        var InsertIntoRentResult = tableCmd.ExecuteNonQuery();
                    }

                    tableCmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    tableCmd.Transaction.Rollback();

                    Debug.WriteLine("DB Error: " + ex.ToString());
                }
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                Debug.WriteLine("DB Error: " + ex.ToString());
                if (ex.InnerException != null)
                    throw ex.InnerException;
            }
            catch (System.InvalidOperationException ex)
            {
                Debug.WriteLine("DB Error: " + ex.ToString());
                if (ex.InnerException != null)
                    throw ex.InnerException;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DB Error: " + ex.ToString());
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("DB Error: " + ex.ToString());
        }

        Debug.WriteLine("Insert Done @InsertAllAbrPostalCodeData in IAbrPostalCodeDataService");

        return true;
    }


}


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

public class XKenAllDataService : IXKenAllDataService
{
    public XKenAllDataService()
    {
        //
    }

    public ObservableCollection<PostalCode> ParseXKenAllCsv(string filePath)
    {
        var kenAll = new ObservableCollection<PostalCode>();

        if (string.IsNullOrEmpty(filePath))
        {
            return kenAll;
        }

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
                if (csv.ColumnCount != 15)
                {
                    Debug.WriteLine($"if (csv.ColumnCount != 15) @ParseXKenAllCsv {csv.ColumnCount}");

                    // TODO: return error code or msg.
                    return kenAll;
                    //continue;
                }

                if (App.MainWnd is null)
                {
                    break;
                }
                
                if (App.MainWnd.Cts.IsCancellationRequested)
                {
                    Debug.WriteLine("IsCancellationRequested in foreach @ParseKenAllCsv");
                    break;
                }

                var obj = new PostalCode
                {
                    PrefectureName = record.PrefectureName,
                    ChouikiName = record.ChouikiName,
                    SikuchousonName = record.SikuchousonName,
                    MunicipalityCode = record.MunicipalityCode,
                    Code = record.Code
                };

                kenAll.Add(obj);
            }
        }

        Debug.WriteLine("Open Done @ParseXKenAllCsv in IXKenAllDataService");

        return kenAll;
    }

    class PostalCodeMapper : CsvHelper.Configuration.ClassMap<PostalCode>
    {
        public PostalCodeMapper()
        {
            AutoMap(CultureInfo.CurrentCulture);
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

    public bool InsertAllXKenAllData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<PostalCode> data)
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
                    tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS x_ken_all (" +
                        "municipality_code TEXT NOT NULL," +
                        "postal_code TEXT NOT NULL," + 
                        "prefecture_name TEXT NOT NULL," +
                        "sikuchouson_name TEXT," +
                        "chouiki_name TEXT" +
                        ")";

                    tableCmd.ExecuteNonQuery();

                    // Insert data
                    foreach (var hoge in data)
                    {
                        var sqlInsertIntoRent = String.Format(
    "INSERT OR IGNORE INTO x_ken_all " +
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

        Debug.WriteLine("Insert Done @InsertAllXKenAllDataAsync in IXKenAllDataService");

        //await Task.CompletedTask;
        return true;
    }

    private static List<PostalCode> SelectAddressesByPostalCode(SqliteConnectionStringBuilder connectionStringBuilder, string postalCode)
    {
        var list = new List<PostalCode>();

        if (string.IsNullOrEmpty(postalCode))
        {
            return list;
        }

        postalCode = postalCode.Replace("-", string.Empty);

        try
        {
            using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();

            cmd.CommandText = $"SELECT * FROM x_ken_all WHERE postal_code = '{postalCode}'";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var hoge = new PostalCode();

                var lgCode = Convert.ToString(reader["municipality_code"]);
                hoge.MunicipalityCode = string.IsNullOrEmpty(lgCode) ? string.Empty : lgCode;

                var psCode = Convert.ToString(reader["postal_code"]);
                hoge.Code = string.IsNullOrEmpty(psCode) ? string.Empty : psCode;

                var pref = Convert.ToString(reader["prefecture_name"]);
                hoge.PrefectureName = string.IsNullOrEmpty(pref) ? string.Empty : pref;

                var city = Convert.ToString(reader["sikuchouson_name"]);
                hoge.SikuchousonName = string.IsNullOrEmpty(city) ? string.Empty : city;

                var town = Convert.ToString(reader["chouiki_name"]);
                hoge.ChouikiName = string.IsNullOrEmpty(town) ? string.Empty : town;

                list.Add(hoge);
            }
        }
        catch (System.Reflection.TargetInvocationException ex)
        {
            Debug.WriteLine("Opps. TargetInvocationException");

            if (ex.InnerException != null)
                throw ex.InnerException;
        }
        catch (System.InvalidOperationException ex)
        {
            Debug.WriteLine("Opps. InvalidOperationException");

            if (ex.InnerException != null)
                throw ex.InnerException;
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                Debug.WriteLine(ex.InnerException.Message);
            }
            else
            {
                Debug.WriteLine(ex.Message);
            }
        }

        return list;
    }

    public async Task<IEnumerable<PostalCode>> SelectAddressesByPostalCodeAsync(SqliteConnectionStringBuilder connectionStringBuilder, string postalCode)
    {
        List<PostalCode> postalCodes = [.. SelectAddressesByPostalCode(connectionStringBuilder, postalCode)];

        await Task.CompletedTask;
        return postalCodes;
    }

}

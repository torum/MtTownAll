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

public class RailLineDataService : IRailLineDataService
{
    public ObservableCollection<RailLine> ParseRailLineCsv(string filePath)
    {
        var rline = new ObservableCollection<RailLine>();

        if (string.IsNullOrEmpty(filePath))
        {
            return rline;
        }

        var config = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            HasHeaderRecord = true,
            Encoding = Encoding.UTF8
        };
        
        using var reader = new StreamReader(filePath, Encoding.UTF8);
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<RailLineClassMapper>();

            var records = csv.GetRecords<RailLine>();

            foreach (var record in records)
            {
                if (csv.ColumnCount != 13)
                {
                    Debug.WriteLine($"if (csv.ColumnCount != 13) @ParseRailLineCsv {csv.ColumnCount}");

                    // TODO: return error code or msg.
                    return rline;
                    //continue;
                }

                if (App.MainWnd is null)
                {
                    break;
                }
                
                if (App.MainWnd.Cts.IsCancellationRequested)
                {
                    Debug.WriteLine("IsCancellationRequested in foreach @ParseRailLineCsv");
                    break;
                }

                var obj = new RailLine
                {
                    LineCode = record.LineCode,
                    LineName = record.LineName,
                    //LineType = record.LineType,
                    LineLon = record.LineLon,
                    LineLat = record.LineLat,
                    LineMapZoom = record.LineMapZoom,
                    LineStatus = record.LineStatus,
                    LineSort = record.LineSort
                };

                rline.Add(obj);
            }
        }

        Debug.WriteLine("Open Done @ParseRailLineCsv in IRailLineDataService");
        
        return rline;
    }

    class RailLineMapper : CsvHelper.Configuration.ClassMap<RailLine>
    {
        public RailLineMapper()
        {
            AutoMap(CultureInfo.CurrentCulture);
        }
    }

    class RailLineClassMapper : CsvHelper.Configuration.ClassMap<RailLine>
    {
        public RailLineClassMapper()
        {
            Map(x => x.LineCode).Index(0);
            Map(x => x.LineName).Index(2);
            //Map(x => x.LineType).Index(7);
            Map(x => x.LineLon).Index(8);
            Map(x => x.LineLat).Index(9);
            Map(x => x.LineMapZoom).Index(10);
            Map(x => x.LineStatus).Index(11);
            Map(x => x.LineSort).Index(12);
        }
    }

    public bool InsertAllRainLineData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<RailLine> data)
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
                    tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS rail_lines (" +
                        "line_cd TEXT NOT NULL," +
                        "line_name TEXT NOT NULL," +
                        //"line_type TEXT," +
                        "lon TEXT," +
                        "lat TEXT," +
                        "zoom TEXT," +
                        "e_status TEXT," +
                        "e_sort TEXT" +
                        ")";

                    tableCmd.ExecuteNonQuery();

                    // Insert data
                    foreach (var hoge in data)
                    {
                        var sqlInsertIntoRent = String.Format(
    "INSERT OR IGNORE INTO rail_lines " +
    "(line_cd, line_name, lon, lat, zoom, e_status, e_sort) " + //, line_type
    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
    hoge.LineCode,
    hoge.LineName,
    //hoge.LineType,
    hoge.LineLon,
    hoge.LineLat,
    hoge.LineMapZoom,
    hoge.LineStatus,
    hoge.LineSort);

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

        Debug.WriteLine("Insert Done @InsertAllRainLineData in RailLineDataService");

        //await Task.CompletedTask;
        return true;
    }

}

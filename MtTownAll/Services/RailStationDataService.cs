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

public class RailStationDataService : IRailStationDataService
{
    public ObservableCollection<RailStation> ParseRailStationCsv(string filePath)
    {
        var rstation = new ObservableCollection<RailStation>();

        if (string.IsNullOrEmpty(filePath))
        {
            return rstation;
        }

        var config = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            HasHeaderRecord = true,
            Encoding = Encoding.UTF8
        };
        
        using var reader = new StreamReader(filePath, Encoding.UTF8);
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<RailStationClassMapper>();

            var records = csv.GetRecords<RailStation>();

            foreach (var record in records)
            {
                if (csv.ColumnCount != 15)
                {
                    Debug.WriteLine($"if (csv.ColumnCount != 15) @ParseRailStationCsv {csv.ColumnCount}");

                    // TODO: return error code or msg.
                    return rstation;
                    //continue;
                }

                if (App.MainWnd is null)
                {
                    break;
                }
                
                if (App.MainWnd.Cts.IsCancellationRequested)
                {
                    Debug.WriteLine("IsCancellationRequested in foreach @ParseRailStationCsv");
                    break;
                }

                var obj = new RailStation
                {
                    StationCode = record.StationCode,
                    StationName = record.StationName,
                    LineCode = record.LineCode,
                    PrefCode = record.PrefCode,
                    StationLon = record.StationLon,
                    StationLat = record.StationLat,
                    StationStatus = record.StationStatus,
                    StationSort = record.StationSort
                };

                rstation.Add(obj);
            }
        }

        Debug.WriteLine("Open Done @ParseRailStationCsv in IRailStationDataService");
        
        return rstation;
    }

    class RailStationMapper : CsvHelper.Configuration.ClassMap<RailStation>
    {
        public RailStationMapper()
        {
            AutoMap(CultureInfo.CurrentCulture);
        }
    }

    class RailStationClassMapper : CsvHelper.Configuration.ClassMap<RailStation>
    {
        public RailStationClassMapper()
        {
            Map(x => x.StationCode).Index(0);
            Map(x => x.StationName).Index(2);
            Map(x => x.LineCode).Index(5);
            Map(x => x.PrefCode).Index(6);
            Map(x => x.StationLon).Index(9);
            Map(x => x.StationLat).Index(10);
            Map(x => x.StationStatus).Index(13);
            Map(x => x.StationSort).Index(14);
        }
    }

    public bool InsertAllRailStationData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<RailStation> data)
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
                    tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS rail_stations (" +
                        "station_cd TEXT NOT NULL," +
                        "station_name TEXT NOT NULL," +
                        "line_cd TEXT NOT NULL," +
                        "pref_cd TEXT," + //0無しint.
                        "lon TEXT," +
                        "lat TEXT," +
                        "e_status TEXT," +
                        "e_sort TEXT" +
                        ")";

                    tableCmd.ExecuteNonQuery();

                    // Insert data
                    foreach (var hoge in data)
                    {
                        var sqlInsertIntoRent = String.Format(
    "INSERT OR IGNORE INTO rail_stations " +
    "(station_cd, station_name, line_cd, pref_cd, lon, lat, e_status, e_sort) " +
    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')",
    hoge.StationCode,
    hoge.StationName,
    hoge.LineCode,
    hoge.PrefCode,
    hoge.StationLon,
    hoge.StationLat,
    hoge.StationStatus,
    hoge.StationSort);

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

        Debug.WriteLine("Insert Done @InsertAllRailStationData in RailStationDataService");

        //await Task.CompletedTask;
        return true;
    }

}

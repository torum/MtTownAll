using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MtTownAll.Services.Contracts;

public interface IRailLineDataService
{
    bool InsertAllRainLineData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<RailLine> data);

    ObservableCollection<RailLine> ParseRailLineCsv(string filePath);

}
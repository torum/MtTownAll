using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace MtTownAll.Services.Contracts;

public interface IRailStationDataService
{
    bool InsertAllRailStationData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<RailStation> data);

    ObservableCollection<RailStation> ParseRailStationCsv(string filePath);

}
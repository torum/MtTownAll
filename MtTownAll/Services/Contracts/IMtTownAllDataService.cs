using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MtTownAll.Services.Contracts;


public interface IMtTownAllDataService
{
    ObservableCollection<Town> ParseMtTownAllCsv(string filePath);

    bool InsertAllTownData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<Town> data);
}
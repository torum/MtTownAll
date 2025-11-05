using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MtTownAll.Services.Contracts;

public interface IRailLineDataService
{
    //Task<IEnumerable<PostalCode>> SelectAddressesByPostalCodeAsync(SqliteConnectionStringBuilder connectionStringBuilder, string postalCode);

    //bool InsertAllXKenAllData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<PostalCode> data);

    ObservableCollection<RailLine> ParseRailLineCsv(string filePath);

}
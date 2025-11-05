using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace MtTownAll.Services.Contracts;

public interface IRailStationDataService
{
    //Task<IEnumerable<PostalCode>> SelectAddressesByPostalCodeAsync(SqliteConnectionStringBuilder connectionStringBuilder, string postalCode);

    //bool InsertAllXKenAllData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<PostalCode> data);

    ObservableCollection<RailStation> ParseRailStationCsv(string filePath);

}
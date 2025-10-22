using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MtTownAll.Services.Contracts;

public interface IXKenAllDataService
{
    Task<IEnumerable<PostalCode>> SelectAddressesByPostalCodeAsync(SqliteConnectionStringBuilder connectionStringBuilder, string postalCode);

    bool InsertAllXKenAllData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<PostalCode> data);

    ObservableCollection<PostalCode> ParseXKenAllCsv(string filePath);

}
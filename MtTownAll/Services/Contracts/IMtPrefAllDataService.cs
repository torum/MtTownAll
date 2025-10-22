using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MtTownAll.Services.Contracts;

public interface IMtPrefAllDataService
{
    Task<IEnumerable<Prefecture>> GetPrefectureDataAsync();

    bool InsertAllPrefectureData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<Prefecture> data);

}

using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MtTownAll.Services.Contracts;

public interface IMtPrefAllDataService
{
    IEnumerable<Prefecture> GetPrefectureData();

    bool InsertAllPrefectureData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<Prefecture> data);

}

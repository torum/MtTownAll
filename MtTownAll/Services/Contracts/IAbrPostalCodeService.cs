using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MtTownAll.Services.Contracts;

public interface IAbrPostalCodeService
{
    ObservableCollection<AbrPostalCode> ParseAbrPostalCodeCsv(string filePath);

    bool InsertAbrPostalCodeData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<AbrPostalCode> data);
}

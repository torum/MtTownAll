using System.Collections.Generic;
using System.Threading.Tasks;
using MtTownAll.Models;

namespace MtTownAll.Services.Contracts;

public interface IPrefectureDataService
{
    Task<IEnumerable<Prefecture>> GetPrefectureDataAsync();

}

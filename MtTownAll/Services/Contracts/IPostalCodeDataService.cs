using System.Collections.Generic;
using System.Threading.Tasks;
using MtTownAll.Models;

namespace MtTownAll.Services.Contracts;


public interface IPostalCodeDataService
{
    Task<IEnumerable<PostalCode>> GetPostalCodeDataAsync(string postalCode);

}
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace MtTownAll.Models;

public class TownCoordinates
{
    //全国地方公共団体コード
    [Index(0)]
    public string MunicipalityCode
    {
        get; set;
    } = string.Empty;

    //町字ID
    [Index(1)]
    public string TownID
    {
        get; set;
    } = string.Empty;

    //代表点_経度
    [Index(3)]
    public string Longitude
    {
        get; set;
    } = string.Empty;

    //代表点_緯度
    [Index(4)]
    public string Latitude
    {
        get; set;
    } = string.Empty;

    // 代表点_座標参照系 eg.EPSG:6668
    [Index(5)]
    public string CRS
    {
        get; set;
    } = string.Empty;

    //代表点_地図情報レベル
    [Index(6)]
    public string MapInfoLovel
    {
        get; set;
    } = string.Empty;
}

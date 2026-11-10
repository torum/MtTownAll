using CsvHelper;

namespace MtTownAll.Models;

[CsvRecord]
public partial class TownCoordinates
{
    //全国地方公共団体コード
    [CsvColumn(Index = 0)]
    public string MunicipalityCode
    {
        get; set;
    } = string.Empty;

    //町字ID
    [CsvColumn(Index = 1)]
    public string TownID
    {
        get; set;
    } = string.Empty;

    //代表点_経度
    [CsvColumn(Index = 3)]
    public string Longitude
    {
        get; set;
    } = string.Empty;

    //代表点_緯度
    [CsvColumn(Index = 4)]
    public string Latitude
    {
        get; set;
    } = string.Empty;

    // 代表点_座標参照系 eg.EPSG:6668
    [CsvColumn(Index = 5)]
    public string CRS
    {
        get; set;
    } = string.Empty;

    //代表点_地図情報レベル
    [CsvColumn(Index = 6)]
    public string MapInfoLovel
    {
        get; set;
    } = string.Empty;
}

using CsvHelper;

namespace MtTownAll.Models;

// 駅データ.jp
// https://ekidata.jp/

// 駅データ
// stationyyyymmddfree.csv

// データベーステーブル名：rail_stations

[CsvRecord]
public partial class RailStation
{
    // 駅コード（カラム名: station_cd）
    [CsvColumn(Index = 0)]
    public string StationCode
    {
        get; set;
    } = string.Empty;

    // 駅名: station_name
    [CsvColumn(Index = 2)]
    public string StationName
    {
        get; set;
    } = string.Empty;

    // 路線コード（カラム名:line_cd）
    [CsvColumn(Index = 5)]
    public string LineCode
    {
        get; set;
    } = string.Empty;

    // 都道府県コード（カラム名:pref_cd） 0無しint.
    [CsvColumn(Index = 6)]
    public string PrefCode
    {
        get; set;
    } = string.Empty;

    // 経度: lon
    [CsvColumn(Index = 9)]
    public string StationLon
    {
        get; set;
    } = string.Empty;

    // 緯度: lat
    [CsvColumn(Index = 10)]
    public string StationLat
    {
        get; set;
    } = string.Empty;

    // ステータス: e_status 「0:運用中　1:運用前　2:廃止」
    [CsvColumn(Index = 13)]
    public string StationStatus
    {
        get; set;
    } = string.Empty;

    // ソートキー: e_sort
    [CsvColumn(Index = 14)]
    public string StationSort
    {
        get; set;
    } = string.Empty;


}

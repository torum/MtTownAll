using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace MtTownAll.Models;

// 駅データ.jp
// https://ekidata.jp/

// 路線データ
// lineyyyymmddfree.csv

// データベーステーブル名：rail_lines

public class RailLine
{
    // 路線コード（カラム名:line_cd）
    [Index(0)]
    public string LineCode
    {
        get; set;
    } = string.Empty;

    // 路線名: line_name
    [Index(2)]
    public string LineName
    {
        get; set;
    } = string.Empty;

    /*
    // 路線区分: line_type 「0:その他　1:新幹線 2:一般 3:地下鉄 4:市電・路面電車 5:モノレール・新交通」
    [Index(7)]
    public string LineType
    {
        get; set;
    } = string.Empty;
    */

    // 経度: lon
    [Index(8)]
    public string LineLon
    {
        get; set;
    } = string.Empty;

    // 緯度: lat
    [Index(9)]
    public string LineLat
    {
        get; set;
    } = string.Empty;

    // Google map 倍率: zoom
    [Index(10)]
    public string LineMapZoom
    {
        get; set;
    } = string.Empty;

    // ステータス: e_status 「0:運用中　1:運用前　2:廃止」
    [Index(11)]
    public string LineStatus
    {
        get; set;
    } = string.Empty;

    // ソートキー: e_sort
    [Index(12)]
    public string LineSort
    {
        get; set;
    } = string.Empty;


}

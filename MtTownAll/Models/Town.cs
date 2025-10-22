using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace MtTownAll.Models;

// 日本 町字マスター データセット
// https://registry-catalog.registries.digital.go.jp/dataset/o1-000000_g2-000003

public class Town
{
    // 全国地方公共団体コード (lg_code)
    // lg_code, 全国地方公共団体コード, 文字列（半角数字）, 6桁, Not Null:Yes, PK, 町字の上位階層の行政区域となる市区町村を一意に識別するためのコード。総務省「全国地方公共団体コード」に従って6桁のコードを収録。
    [Index(0)]
    public string MunicipalityCode
    {
        get; set;
    } = string.Empty;

    // 町字id (machiaza_id)
    // Not Null:Yes,
    [Index(1)]
    public string TownID
    {
        get; set;
    } = string.Empty;

    // 町字区分コード (machiaza_type)
    // machiaza_type, 町字区分コード ,文字列（半角数字）, 1桁, Not Null:Yes, 収録する町字の区分。(1:大字・町 2:丁目 3:小字 4:大字・町/丁目/小字なし 5:道路方式の住居表示における道路名)
    [Index(2)]
    public string ChouAzaType
    {
        get; set;
    } = string.Empty;

    // 都道府県名 (pref)
    // Not Null:Yes,
    [Index(3)]
    public string PrefectureName
    {
        get; set;
    } = string.Empty;

    // 郡名 (county)
    [Index(6)]
    public string CountyName
    {
        get; set;
    } = string.Empty;

    // 市区町村名 (city) 
    // Not Null:Yes,
    [Index(9)]
    public string SikuchousonName
    {
        get; set;
    } = string.Empty;

    // 政令市区名（ward） e.g.中央区
    [Index(12)]
    public string WardName
    {
        get; set;
    } = string.Empty;

    // 大字・町名（oaza_cho） e.g.旭ヶ丘
    [Index(15)]
    public string TownName
    {
        get; set;
    } = string.Empty;

    // 丁目名 (chome) e.g. 一丁目
    [Index(18)]
    public string Choume
    {
        get; set;
    } = string.Empty;

    // 子字 (koaza)
    [Index(21)]
    public string KoazaName
    {
        get; set;
    } = string.Empty;

    // 郵便番号 (post_code)
    [Index(36)]
    public string PostalCode
    {
        get; set;
    } = string.Empty;
}

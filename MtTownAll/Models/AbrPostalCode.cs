using CsvHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MtTownAll.Models;

// ABR町字・郵便番号変換表
// abr_post_code.csv
// lg_code,machiaza_id,pref,county,city,ward,kyoto_st,oaza_cho,chome,koaza,machiaza_dist,post_code,add_date,dlt_date

// 郵便番号（データベーステーブル名：abr_postal_codes）
[CsvRecord]
public partial class AbrPostalCode
{
    // 
    [CsvColumn(Index = 0)]
    public string LgCode
    {
        get; set;
    } = string.Empty;

    //町字コードID　（空の場合もあることに注意）
    [CsvColumn(Index = 1)]
    public string MachiazaId
    {
        get; set;
    } = string.Empty;

    //
    [CsvColumn(Index = 2)]
    public string Pref
    {
        get; set;
    } = string.Empty;

    //
    [CsvColumn(Index = 3)]
    public string County
    {
        get; set;
    } = string.Empty;

    //
    [CsvColumn(Index = 4)]
    public string City
    {
        get; set;
    } = string.Empty;

    [CsvColumn(Index = 5)]
    public string Ward
    {
        get; set;
    } = string.Empty;

    //
    [CsvColumn(Index = 6)]
    public string KyotoSt
    {
        get; set;
    } = string.Empty;

    //
    [CsvColumn(Index = 7)]
    public string OazaCho
    {
        get; set;
    } = string.Empty;

    //
    [CsvColumn(Index = 8)]
    public string Chome
    {
        get; set;
    } = string.Empty;

    [CsvColumn(Index = 9)]
    public string Koaza
    {
        get; set;
    } = string.Empty;

    [CsvColumn(Index = 10)]
    public string MachiazaDist
    {
        get; set;
    } = string.Empty;

    [CsvColumn(Index = 11)]
    public string PostCode
    {
        get; set;
    } = string.Empty;

    [CsvColumn(Index = 12)]
    public string AddDate
    {
        get; set;
    } = string.Empty;

    [CsvColumn(Index = 13)]
    public string DltDate
    {
        get; set;
    } = string.Empty;
}
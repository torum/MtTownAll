namespace MtTownAll.Models;

public class Prefecture
{
    // 市区町村コード
    public string MunicipalityCode
    {
        get; set;
    } = string.Empty;

    // 都道府県コード
    public string Code
    {
        get; set;
    } = string.Empty;

    public string PrefectureName
    {
        get; set;
    } = string.Empty;

    public string PrefectureNameKana
    {
        get; set;
    } = string.Empty;

    public string PrefectureNameRoma
    {
        get; set;
    } = string.Empty;
}

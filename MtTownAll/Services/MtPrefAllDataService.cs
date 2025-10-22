using Microsoft.Data.Sqlite;
using MtTownAll.Models;
using MtTownAll.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MtTownAll.Services;

public class MtPrefAllDataService : IMtPrefAllDataService
{
    private List<Prefecture> _prefectures;

    private static IEnumerable<Prefecture> Prefectures =>
        [
            new()
            {
                MunicipalityCode = "010006",
                Code = "01",
                PrefectureName = "北海道",
                PrefectureNameKana = "ホッカイドウ",
                PrefectureNameRoma = "Hokkaido"
            },
            new()
            {
                MunicipalityCode = "020001",
                Code = "02",
                PrefectureName = "青森県",
                PrefectureNameKana = "アオモリケン",
                PrefectureNameRoma = "Aomori"
            },
            new()
            {
                MunicipalityCode = "030007",
                Code = "03",
                PrefectureName = "岩手県",
                PrefectureNameKana = "イワテケン",
                PrefectureNameRoma = "Iwate"
            },
            new()
            {
                MunicipalityCode = "040002",
                Code = "04",
                PrefectureName = "宮城県",
                PrefectureNameKana = "ミヤギケン",
                PrefectureNameRoma = "Miyagi"
            },
            new()
            {
                MunicipalityCode = "050008",
                Code = "05",
                PrefectureName = "秋田県",
                PrefectureNameKana = "アキタケン",
                PrefectureNameRoma = "Akita"
            },
            new()
            {
                MunicipalityCode = "060003",
                Code = "06",
                PrefectureName = "山形県",
                PrefectureNameKana = "ヤマガタケン",
                PrefectureNameRoma = "Yamagata"
            },
            new()
            {
                MunicipalityCode = "070009",
                Code = "07",
                PrefectureName = "福島県",
                PrefectureNameKana = "フクシマケン",
                PrefectureNameRoma = "Fukushima"
            },
            new()
            {
                MunicipalityCode = "080004",
                Code = "08",
                PrefectureName = "茨城県",
                PrefectureNameKana = "イバラキケン",
                PrefectureNameRoma = "Ibaraki"
            },
            new()
            {
                MunicipalityCode = "090000",
                Code = "09",
                PrefectureName = "栃木県",
                PrefectureNameKana = "トチギケン",
                PrefectureNameRoma = "Tochigi"
            },
            new()
            {
                MunicipalityCode = "100005",
                Code = "10",
                PrefectureName = "群馬県",
                PrefectureNameKana = "グンマケン",
                PrefectureNameRoma = "Gumma"
            },
            new()
            {
                MunicipalityCode = "110001",
                Code = "11",
                PrefectureName = "埼玉県",
                PrefectureNameKana = "サイタマケン",
                PrefectureNameRoma = "Saitama"
            },
            new()
            {
                MunicipalityCode = "120006",
                Code = "12",
                PrefectureName = "千葉県",
                PrefectureNameKana = "チバケン",
                PrefectureNameRoma = "Chiba"
            },
            new()
            {
                MunicipalityCode = "130001",
                Code = "13",
                PrefectureName = "東京都",
                PrefectureNameKana = "トウキョウト",
                PrefectureNameRoma = "Tokyo"
            },
            new()
            {
                MunicipalityCode = "140007",
                Code = "14",
                PrefectureName = "神奈川県",
                PrefectureNameKana = "カナガワケン",
                PrefectureNameRoma = "Kanagawa"
            },
            new()
            {
                MunicipalityCode = "150002",
                Code = "15",
                PrefectureName = "新潟県",
                PrefectureNameKana = "ニイガタケン",
                PrefectureNameRoma = "Niigata"
            },
            new()
            {
                MunicipalityCode = "160008",
                Code = "16",
                PrefectureName = "富山県",
                PrefectureNameKana = "トヤマケン",
                PrefectureNameRoma = "Toyama"
            },
            new()
            {
                MunicipalityCode = "170003",
                Code = "17",
                PrefectureName = "石川県",
                PrefectureNameKana = "イシカワケン",
                PrefectureNameRoma = "Ishikawa"
            },
            new()
            {
                MunicipalityCode = "180009",
                Code = "18",
                PrefectureName = "福井県",
                PrefectureNameKana = "フクイケン",
                PrefectureNameRoma = "Fukui"
            },
            new()
            {
                MunicipalityCode = "190004",
                Code = "19",
                PrefectureName = "山梨県",
                PrefectureNameKana = "ヤマナシケン",
                PrefectureNameRoma = "Yamanashi"
            },
            new()
            {
                MunicipalityCode = "200000",
                Code = "20",
                PrefectureName = "長野県",
                PrefectureNameKana = "ナガノケン",
                PrefectureNameRoma = "Nagano"
            },
            new()
            {
                MunicipalityCode = "210005",
                Code = "21",
                PrefectureName = "岐阜県",
                PrefectureNameKana = "ギフケン",
                PrefectureNameRoma = "Gifu"
            },
            new()
            {
                MunicipalityCode = "220001",
                Code = "22",
                PrefectureName = "静岡県",
                PrefectureNameKana = "シズオカケン",
                PrefectureNameRoma = "Shizuoka"
            },
            new()
            {
                MunicipalityCode = "230006",
                Code = "23",
                PrefectureName = "愛知県",
                PrefectureNameKana = "アイチケン",
                PrefectureNameRoma = "Aichi"
            },
            new()
            {
                MunicipalityCode = "240001",
                Code = "24",
                PrefectureName = "三重県",
                PrefectureNameKana = "ミエケン",
                PrefectureNameRoma = "Mie"
            },
            new()
            {
                MunicipalityCode = "250007",
                Code = "25",
                PrefectureName = "滋賀県",
                PrefectureNameKana = "シガケン",
                PrefectureNameRoma = "Shiga"
            },
            new()
            {
                MunicipalityCode = "260002",
                Code = "26",
                PrefectureName = "京都府",
                PrefectureNameKana = "キョウトフ",
                PrefectureNameRoma = "Kyoto"
            },
            new()
            {
                MunicipalityCode = "270008",
                Code = "27",
                PrefectureName = "大阪府",
                PrefectureNameKana = "オオサカフ",
                PrefectureNameRoma = "Osaka"
            },
            new()
            {
                MunicipalityCode = "280003",
                Code = "28",
                PrefectureName = "兵庫県",
                PrefectureNameKana = "ヒョウゴケン",
                PrefectureNameRoma = "Hyogo"
            },
            new()
            {
                MunicipalityCode = "290009",
                Code = "29",
                PrefectureName = "奈良県",
                PrefectureNameKana = "ナラケン",
                PrefectureNameRoma = "Nara"
            },
            new()
            {
                MunicipalityCode = "300004",
                Code = "30",
                PrefectureName = "和歌山県",
                PrefectureNameKana = "ワカヤマケン",
                PrefectureNameRoma = "Wakayama"
            },
            new()
            {
                MunicipalityCode = "310000",
                Code = "31",
                PrefectureName = "鳥取県",
                PrefectureNameKana = "トットリケン",
                PrefectureNameRoma = "Tottori"
            },
            new()
            {
                MunicipalityCode = "320005",
                Code = "32",
                PrefectureName = "島根県",
                PrefectureNameKana = "シマネケン",
                PrefectureNameRoma = "Shimane"
            },
            new()
            {
                MunicipalityCode = "330001",
                Code = "33",
                PrefectureName = "岡山県",
                PrefectureNameKana = "オカヤマケン",
                PrefectureNameRoma = "Okayama"
            },
            new()
            {
                MunicipalityCode = "340006",
                Code = "34",
                PrefectureName = "広島県",
                PrefectureNameKana = "ヒロシマケン",
                PrefectureNameRoma = "Hiroshima"
            },
            new()
            {
                MunicipalityCode = "350001",
                Code = "35",
                PrefectureName = "山口県",
                PrefectureNameKana = "ヤマグチケン",
                PrefectureNameRoma = "Yamaguchi"
            },
            new()
            {
                MunicipalityCode = "360007",
                Code = "36",
                PrefectureName = "徳島県",
                PrefectureNameKana = "トクシマケン",
                PrefectureNameRoma = "Tokushima"
            },
            new()
            {
                MunicipalityCode = "370002",
                Code = "37",
                PrefectureName = "香川県",
                PrefectureNameKana = "カガワケン",
                PrefectureNameRoma = "Kagawa"
            },
            new()
            {
                MunicipalityCode = "380008",
                Code = "38",
                PrefectureName = "愛媛県",
                PrefectureNameKana = "エヒメケン",
                PrefectureNameRoma = "Ehime"
            },
            new()
            {
                MunicipalityCode = "390003",
                Code = "39",
                PrefectureName = "高知県",
                PrefectureNameKana = "コウチケン",
                PrefectureNameRoma = "Kochi"
            },
            new()
            {
                MunicipalityCode = "400009",
                Code = "40",
                PrefectureName = "福岡県",
                PrefectureNameKana = "フクオカケン",
                PrefectureNameRoma = "Fukuoka"
            },
            new()
            {
                MunicipalityCode = "410004",
                Code = "41",
                PrefectureName = "佐賀県",
                PrefectureNameKana = "サガケン",
                PrefectureNameRoma = "Saga"
            },
            new()
            {
                MunicipalityCode = "420000",
                Code = "42",
                PrefectureName = "長崎県",
                PrefectureNameKana = "ナガサキケン",
                PrefectureNameRoma = "Nagasaki"
            },
            new()
            {
                MunicipalityCode = "430005",
                Code = "43",
                PrefectureName = "熊本県",
                PrefectureNameKana = "クマモトケン",
                PrefectureNameRoma = "Kumamoto"
            },
            new()
            {
                MunicipalityCode = "440001",
                Code = "44",
                PrefectureName = "大分県",
                PrefectureNameKana = "オオイタケン",
                PrefectureNameRoma = "Oita"
            },
            new()
            {
                MunicipalityCode = "450006",
                Code = "45",
                PrefectureName = "宮崎県",
                PrefectureNameKana = "ミヤザキケン",
                PrefectureNameRoma = "Miyazaki"
            },
            new()
            {
                MunicipalityCode = "460001",
                Code = "46",
                PrefectureName = "鹿児島県",
                PrefectureNameKana = "カゴシマケン",
                PrefectureNameRoma = "Kagoshima"
            },
            new()
            {
                MunicipalityCode = "470007",
                Code = "47",
                PrefectureName = "沖縄県",
                PrefectureNameKana = "オキナワケン",
                PrefectureNameRoma = "Okinawa"
            }
        ];

    public MtPrefAllDataService()
    {
        _prefectures = [.. Prefectures];
    }

    public async Task<IEnumerable<Prefecture>> GetPrefectureDataAsync()
    {
        _prefectures ??= [.. Prefectures];

        await Task.CompletedTask;
        return _prefectures;
    }

    public bool InsertAllPrefectureData(SqliteConnectionStringBuilder connectionStringBuilder, IEnumerable<Prefecture> data)
    {
        if (!data.Any())
        {
            return false;
        }

        try
        {
            using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            try
            {
                connection.Open();

                using var tableCmd = connection.CreateCommand();

                tableCmd.Transaction = connection.BeginTransaction();
                try
                {
                    // Create table if not exists.
                    tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS mt_pref_all (" +
                        "code TEXT NOT NULL," +
                        "lg_code TEXT NOT NULL," + // PRIMARY KEY
                        "pref TEXT NOT NULL," +
                        "pref_kana TEXT," +
                        "pref_roma TEXT" +
                        ")";

                    tableCmd.ExecuteNonQuery();

                    // Insert data
                    foreach (var hoge in data)
                    {
                        var sqlInsertIntoRent = String.Format(
    "INSERT OR IGNORE INTO mt_pref_all " +
    "(code, lg_code, pref, pref_kana, pref_roma) " +
    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
    hoge.Code,
    hoge.MunicipalityCode,
    hoge.PrefectureName,
    hoge.PrefectureNameKana,
    hoge.PrefectureNameRoma);

                        tableCmd.CommandText = sqlInsertIntoRent;

                        var InsertIntoRentResult = tableCmd.ExecuteNonQuery();
                    }

                    tableCmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    tableCmd.Transaction.Rollback();

                    Debug.WriteLine("DB Error: " + ex.Message);
                }
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                Debug.WriteLine("DB Error: " + ex.Message);
                if (ex.InnerException != null)
                    throw ex.InnerException;
            }
            catch (System.InvalidOperationException ex)
            {
                Debug.WriteLine("DB Error: " + ex.Message);
                if (ex.InnerException != null)
                    throw ex.InnerException;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DB Error: " + ex.Message);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("DB Error: " + ex.Message);
        }

        Debug.WriteLine("Insert Done @InsertAllPrefectureDataAsync in IMtPrefAllDataService");

        return true;
    }
}

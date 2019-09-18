using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ExcelDataReader;
using Newtonsoft.Json;

namespace LanguageParser
{
    [System.Security.SecurityCritical]
    class Program
    {
        public static List<string> FilesTranslatePathNames = new List<string> { "en", "ru" };

        public static string FilePathWithTranslations = @"C:\Users\Dmitry PC\Downloads\USB_QnA_and_Reports_All_Languages_20190903-reduced_All Langs.xls";
        public static string PathWithTranslationFiles = @"D:\i18n\";
        static void Main(string[] args)
        {

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(FilePathWithTranslations, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());

                    // 2. Use the AsDataSet extension method
                    var result = reader.AsDataSet();

                    var table = result.Tables[0];
                    var keys = new List<KeyTranslateModel>();
                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        var model = new KeyTranslateModel
                        {
                            Name = table.Rows[i].ItemArray[5].ToString(),
                            English = table.Rows[i].ItemArray[8].ToString(),
                            Portuguese = table.Rows[i].ItemArray[9].ToString(),
                            French = table.Rows[i].ItemArray[10].ToString(),
                            German = table.Rows[i].ItemArray[11].ToString(),
                            Spanish = table.Rows[i].ItemArray[12].ToString(),
                            Italian = table.Rows[i].ItemArray[13].ToString(),
                            Russian = table.Rows[i].ItemArray[14].ToString(),
                            Japanese = table.Rows[i].ItemArray[15].ToString(),
                            Korean = table.Rows[i].ItemArray[16].ToString(),
                            ChineseSimplified = table.Rows[i].ItemArray[17].ToString(),
                        };
                        //model.English.KeyValue.Add(table.Rows[i].ItemArray[5].ToString(),table.Rows[i].ItemArray[8].ToString());
                        keys.Add(model);
                    }
                    // The result of each spreadsheet is in result.Tables

                    foreach (var key in keys)
                    {
                        foreach (var p in key.GetType().GetProperties().Where(p => !p.GetGetMethod().GetParameters().Any()))
                        {
                            var fileNameAttribute = p.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                                .Cast<DisplayNameAttribute>().FirstOrDefault();
                            if (fileNameAttribute != null)
                            {
                                var fileName = fileNameAttribute.DisplayName;
                                string json;

                                var path = $@"{PathWithTranslationFiles}{fileName}.json";

                                using (StreamReader r = new StreamReader(path))
                                {
                                    json = r.ReadToEnd();
                                }

                                var splitedJson = json.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                                var lineWithKey = splitedJson.First(x => x.Contains("\"" + key.Name + "\":"));

                                var value = lineWithKey.Split(':')[1];

                                var comma = lineWithKey.Contains(',') ? "," :"";

                                var changedLineWithKey = lineWithKey.Replace(value, $" \"{p.GetValue(key, null)}\"{comma}");


                                var newJson = json.Replace(lineWithKey, changedLineWithKey);
                                File.WriteAllText(path, newJson, Encoding.UTF8);
                            }
                        }
                    }
                }


            }

            Console.Read();
        }
    }

    public class KeyTranslateModel
    {
        //public KeyTranslateModel()
        //{
        //    English = new LanguageModel()
        //    {
        //        TranslateFileName = "en"
        //    };
        //}

        public string Name { get; set; }

        [DisplayName("en")]
        public string English { get; set; }

        [DisplayName("pt")]
        public string Portuguese { get; set; }

        [DisplayName("fr")]
        public string French { get; set; }

        [DisplayName("de")]
        public string German { get; set; }

        [DisplayName("es")]
        public string Spanish { get; set; }

        [DisplayName("it")]
        public string Italian { get; set; }

        [DisplayName("ru")]
        public string Russian { get; set; }

        [DisplayName("ja")]
        public string Japanese { get; set; }

        [DisplayName("ko")]
        public string Korean { get; set; }

        [DisplayName("zh-hans")]
        public string ChineseSimplified { get; set; }
    }

    public class LanguageModel
    {
        public LanguageModel()
        {
            KeyValue = new Dictionary<string, string>();
        }

        public string TranslateFileName { get; set; }
        public Dictionary<string, string> KeyValue { get; set; }
    }
}

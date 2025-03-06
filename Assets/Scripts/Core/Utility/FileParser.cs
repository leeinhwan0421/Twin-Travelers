using System.Collections.Generic;
using System.IO;

namespace TwinTravelers.Core.Utility
{
    #region ini
    /// <summary>
    /// INI 파일을 읽고, 특정 필드를 읽고 불러옵니다.
    /// </summary>
    public static class INIParser
    {
        /// <summary>
        /// INI 파일을 읽어서 Dictionary로 반환합니다.
        /// </summary>
        /// <param name="path">INI 파일의 위치</param>
        /// <returns>INI 파일 내부 데이터를 , <Key, Value> 형태로 반환합니다.</returns>
        public static Dictionary<string, string> ReadINI(string path)
        {
            var data = new Dictionary<string, string>();

            if (!File.Exists(path))
                return data;

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";") || line.StartsWith("#"))
                    continue;

                var keyValue = line.Split(new[] { '=' }, 2);
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();
                    data[key] = value;
                }
            }

            return data;
        }

        /// <summary>
        /// Dictionary를 INI 파일로 저장합니다.
        /// </summary>
        /// <param name="path">INI 파일의 위치</param>
        /// <param name="data">Dictionary</param>
        public static void WriteINI(string path, Dictionary<string, string> data)
        {
            StreamWriter writer = new StreamWriter(path);

            foreach (var entry in data)
            {
                writer.WriteLine($"{entry.Key}={entry.Value}");
            }

            writer.Close();
        }
    }
    #endregion
}

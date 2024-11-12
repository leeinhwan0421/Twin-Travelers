using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#region JSON
public static class JsonParser
{

}
#endregion

#region ini
public static class INIParser
{
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

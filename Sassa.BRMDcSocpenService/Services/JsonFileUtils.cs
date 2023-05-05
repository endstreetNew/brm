using System.Text.Json.Serialization;
using System.Text.Json;
using static Sassa.BRM.Services.TimedService;

namespace Sassa.BRM.Services
{
    // Native/JsonFileUtils.cs
    public static class JsonFileUtils
    {
        
        private static readonly JsonSerializerOptions _options = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public static void WriteJson(object obj, string fileName)
        {
            var jsonString = JsonSerializer.Serialize(obj, _options);
            if(!Directory.Exists(fileName.Substring(0,fileName.LastIndexOf("\\"))))
            {
                Directory.CreateDirectory(fileName.Substring(0, fileName.LastIndexOf("\\")));
            }
            File.WriteAllText(fileName, jsonString);
            GlobalVars entry = (GlobalVars)obj;
            File.AppendAllText(fileName.Substring(0, fileName.LastIndexOf("\\")) + "\\logFile.json",entry.Progress + " " + System.DateTime.Now.ToShortTimeString() + Environment.NewLine  );
        }

        public static T ReadJson<T>(string fileName) where T : new()
        {
            if (!File.Exists(fileName)) return new T();
            string jsonString = File.ReadAllText(fileName);
            if (string.IsNullOrEmpty(jsonString)) return new T();
            return JsonSerializer.Deserialize<T>(jsonString);
        }

    }
}

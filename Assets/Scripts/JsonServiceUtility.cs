using System.IO;
using UnityEngine;

/// <summary>
/// Утилита для боллее комфортного использования JsonUtility
/// </summary>
public static class JsonServiceUtility
{
    /// <summary>
    /// Сереализует объект в файл JSON по ключу
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    public static void SaveData(object data, string key = "ApplicationData")
    {
        string jsonData = JsonUtility.ToJson(data, true);   
        
        string path = BuildPath(key);

        File.WriteAllText(path, jsonData);
    }

    /// <summary>
    /// Позволяет десериализовать данные JSON по ключу
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T LoadData<T>(string key = "ApplicationData")
    {
        try
        {
            string path = BuildPath(key);

            string jsonData = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(jsonData);

            return data;
        }
        catch
        {
            return default;
        }
    }

    private static string BuildPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key + ".json");
    }
}

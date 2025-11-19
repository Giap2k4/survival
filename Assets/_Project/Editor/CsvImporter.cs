#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public static class CsvImporter
{
    private const string CSV_FOLDER = "Assets/_Project/Csv/Feature/";
    private const string FEATURE_FOLDER = "Assets/_Project/Feature/UI/";

    [MenuItem("Tools/CSV/Import All %#h")] // Ctrl + Shift + H
    public static void ImportAll()
    {
        if (!Directory.Exists(CSV_FOLDER))
        {
            Debug.LogError($"❌ Folder không tồn tại: {CSV_FOLDER}");
            return;
        }

        var csvFiles = Directory.GetFiles(CSV_FOLDER, "*.csv", SearchOption.AllDirectories);

        if (csvFiles.Length == 0)
        {
            Debug.LogWarning("⚠ Không tìm thấy file CSV nào.");
            return;
        }

        foreach (var csv in csvFiles)
        {
            Import(csv);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("<color=lime>✔ CSV Import Completed!</color>");
    }

    private static void Import(string csvPath)
    {
        string featureName = Path.GetFileNameWithoutExtension(csvPath);

        string resourcePath = $"{FEATURE_FOLDER}{featureName}/Resources";
        if (!Directory.Exists(resourcePath))
        {
            Debug.LogWarning($"⚠ Bỏ qua {featureName}. Folder không tồn tại: {resourcePath}");
            return;
        }

        // tìm scriptableObject collection
        Type collectionType = FindType($"{featureName}Collection");
        if (collectionType == null)
        {
            Debug.LogWarning($"⚠ Không tìm thấy class: {featureName}Collection");
            return;
        }

        string assetPath = $"{resourcePath}/{featureName}Collection.asset";
        ScriptableObject so = AssetDatabase.LoadAssetAtPath(assetPath, collectionType) as ScriptableObject;

        if (so == null)
        {
            so = ScriptableObject.CreateInstance(collectionType);
            AssetDatabase.CreateAsset(so, assetPath);
        }

        // Tìm field dataGroups
        var modelField = collectionType.GetField("dataGroups", BindingFlags.Public | BindingFlags.Instance);
        if (modelField == null)
        {
            Debug.LogError($"❌ {featureName}Collection không có biến public dataGroups");
            return;
        }

        string csvContent = File.ReadAllText(csvPath);
        var rows = ParseCsv(csvContent);

        var header = rows[0];
        rows.RemoveAt(0);

        object modelBuilt = BuildModel(modelField.FieldType, header, rows);
        modelField.SetValue(so, modelBuilt);

        EditorUtility.SetDirty(so);
        Debug.Log($"<color=#00d1ff>✔ Imported → {Path.GetFileName(csvPath)}</color>");
    }

    private static List<string[]> ParseCsv(string text)
    {
        return text.Replace("\r", "")
                   .Split('\n')
                   .Where(l => !string.IsNullOrWhiteSpace(l))
                   .Select(l => l.Split(','))
                   .ToList();
    }

    private static Type FindType(string name)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == name);
    }

    private static object BuildModel(Type modelType, string[] header, List<string[]> rows)
    {
        // CASE A: dataGroups là MẢNG (HeroModel[])
        if (modelType.IsArray)
        {
            Type elementType = modelType.GetElementType();           // HeroModel
            Array array = Array.CreateInstance(elementType, rows.Count);

            var elementFields = elementType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < rows.Count; i++)
            {
                string[] row = rows[i];
                object element = Activator.CreateInstance(elementType);

                foreach (var f in elementFields)
                {
                    string fieldName = f.Name;                        // id, nameHero, level...
                    int colIndex = Array.FindIndex(
                        header,
                        h => SnakeToCamel(h) == fieldName            // nameHero -> nameHero, name_hero -> nameHero
                    );

                    if (colIndex >= 0 && !string.IsNullOrWhiteSpace(row[colIndex]))
                    {
                        object val = ConvertValue(f.FieldType, row[colIndex]);
                        f.SetValue(element, val);
                    }
                }

                array.SetValue(element, i);
            }

            return array;
        }

        // CASE B: dataGroups là 1 OBJECT (SevenDayLoginModel)
        object model = Activator.CreateInstance(modelType);
        var fields = modelType.GetFields(BindingFlags.Public | BindingFlags.Instance);

        // --- map scalar global (liveTimes, shardConvert, ...) ---
        foreach (var field in fields.Where(f => !f.FieldType.IsArray))
        {
            string fieldName = field.Name;
            int idx = Array.FindIndex(header, h => SnakeToCamel(h) == fieldName);

            if (idx >= 0 && !string.IsNullOrWhiteSpace(rows[0][idx]))
            {
                object converted = ConvertValue(field.FieldType, rows[0][idx]);
                field.SetValue(model, converted);
            }
        }

        // --- phần xử lý mảng & group-by (SevenDayLogin) giữ như cũ ---
        foreach (var field in fields.Where(f => f.FieldType.IsArray))
        {
            Type elementType = field.FieldType.GetElementType();
            var subFields = elementType.GetFields();

            var keyField = subFields.FirstOrDefault(f => f.FieldType == typeof(int) &&
                                                         f.Name.ToLower().Contains("day"));
            if (keyField != null)
            {
                var group = new Dictionary<int, List<object>>();

                foreach (var row in rows)
                {
                    int keyIndex = Array.FindIndex(header, h => SnakeToCamel(h) == keyField.Name);
                    int key = Convert.ToInt32(row[keyIndex]);

                    if (!group.ContainsKey(key))
                        group[key] = new List<object>();

                    var nestedArrayField = subFields.First(sf => sf.FieldType.IsArray);
                    Type nestedType = nestedArrayField.FieldType.GetElementType();
                    var nestedFields = nestedType.GetFields();

                    object nestedObj = Activator.CreateInstance(nestedType);

                    foreach (var nf in nestedFields)
                    {
                        int col = Array.FindIndex(header, h => SnakeToCamel(h) == nf.Name);
                        if (col >= 0 && !string.IsNullOrWhiteSpace(row[col]))
                            nf.SetValue(nestedObj, ConvertValue(nf.FieldType, row[col]));
                    }

                    group[key].Add(nestedObj);
                }

                var result = new List<object>();
                foreach (var kvp in group.OrderBy(g => g.Key))
                {
                    object instance = Activator.CreateInstance(elementType);
                    keyField.SetValue(instance, kvp.Key);

                    var nestedField = elementType.GetFields().First(f => f.FieldType.IsArray);
                    nestedField.SetValue(instance, kvp.Value.ToArray());

                    result.Add(instance);
                }

                field.SetValue(model, result.ToArray());
            }
        }

        return model;
    }


    private static string SnakeToCamel(string name)
    {
        if (string.IsNullOrEmpty(name))
            return string.Empty;

        name = name.Trim();

        // Nếu không chứa '_', chỉ cần làm lowercase chữ cái đầu
        if (!name.Contains("_"))
            return char.ToLowerInvariant(name[0]) + name.Substring(1);

        // name_hero → nameHero
        var parts = name.ToLowerInvariant().Split('_');
        for (int i = 1; i < parts.Length; i++)
        {
            if (parts[i].Length == 0) continue;
            parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1);
        }

        return string.Join("", parts);
    }


    private static string Normalize(string name)
    {
        var parts = name.ToLower().Split('_').ToList();
        for (int i = 1; i < parts.Count; i++)
            parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
        return string.Join("", parts);
    }

    private static object ConvertValue(Type t, string value)
    {
        if (t.IsEnum) return Enum.Parse(t, value);
        return Convert.ChangeType(value, t);
    }
}
#endif

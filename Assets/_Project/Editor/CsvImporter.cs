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
        // Không dùng header nữa cho mapping, chỉ dùng rows (dữ liệu) + thứ tự field

        // CASE A: dataGroups là MẢNG (ví dụ: HeroModel[])
        if (modelType.IsArray)
        {
            Type elementType = modelType.GetElementType();   // HeroModel
            int count = rows.Count;

            Array array = Array.CreateInstance(elementType, count);

            for (int i = 0; i < count; i++)
            {
                string[] row = rows[i];
                int colIndex = 0; // luôn bắt đầu từ cột 0 cho mỗi dòng

                object element = BuildObject(elementType, row, ref colIndex);
                array.SetValue(element, i);
            }

            return array;
        }

        // CASE B: dataGroups là 1 OBJECT (SevenDayLoginModel,...)
        // Lấy từ dòng đầu tiên
        if (rows.Count == 0)
            return null;

        {
            string[] row = rows[0];
            int colIndex = 0;
            object model = BuildObject(modelType, row, ref colIndex);
            return model;
        }
    }


    private static bool IsSimpleType(Type t)
    {
        return t.IsPrimitive
               || t.IsEnum
               || t == typeof(string)
               || t == typeof(decimal);
    }

    private static object ConvertValue(Type t, string value)
    {
        if (string.IsNullOrEmpty(value))
            return t.IsValueType ? Activator.CreateInstance(t) : null;

        if (t.IsEnum)
        {
            // ignore case: "Money", "money", "MONEY" đều được
            return Enum.Parse(t, value, true);
        }

        return Convert.ChangeType(value, t);
    }

    /// <summary>
    /// Xây 1 object từ 1 dòng CSV theo THỨ TỰ FIELD.
    /// - Simple type (int, string, enum...) ăn 1 cột.
    /// - Object lồng object → đệ quy, ăn tiếp các cột sau.
    /// </summary>
    private static object BuildObject(Type type, string[] row, ref int colIndex)
    {
        object obj = Activator.CreateInstance(type);

        // Lấy tất cả field public instance theo đúng thứ tự khai báo
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var f in fields)
        {
            Type ft = f.FieldType;

            // Kiểu mảng / List / struct phức tạp khác hiện tại bỏ qua (chỉ xử lý object đơn)
            if (ft.IsArray)
            {
                // nếu sau này cần, sẽ xử lý riêng
                continue;
            }

            if (IsSimpleType(ft))
            {
                // ăn 1 cột
                if (colIndex >= row.Length)
                    break;

                string cell = row[colIndex];
                colIndex++;

                if (!string.IsNullOrWhiteSpace(cell))
                {
                    object val = ConvertValue(ft, cell);
                    f.SetValue(obj, val);
                }
            }
            else
            {
                // object lồng object → đệ quy
                object nested = BuildObject(ft, row, ref colIndex);
                f.SetValue(obj, nested);
            }
        }

        return obj;
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
}
#endif

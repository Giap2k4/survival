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
    private const string FEATURE_FOLDER = "Assets/_Project/Feature/";

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
        string fileName = Path.GetFileNameWithoutExtension(csvPath);

        // Xác định typeName và assetName dựa trên cấu trúc folder
        // - File ở root: Stats.csv → typeName = "Stats", assetName = "Stats"
        // - File ở subfolder: Skill/Skill_1.csv → typeName = "Skill", assetName = "Skill_1"
        string relativePath = csvPath.Replace("\\", "/").Replace(CSV_FOLDER, "");
        string[] pathParts = relativePath.Split('/');

        string typeName;
        string assetName;

        if (pathParts.Length > 1)
        {
            // File ở subfolder → dùng tên folder cho type, tên file cho asset
            // Ví dụ: Skill/Skill_1.csv → typeName = "Skill", assetName = "Skill_1"
            typeName = pathParts[0];
            assetName = fileName;
        }
        else
        {
            // File ở root → dùng tên file cho type, asset là {typeName}Collection
            // Ví dụ: Hero.csv → typeName = "Hero", assetName = "HeroCollection"
            typeName = fileName;
            assetName = $"{typeName}Collection";
        }

        // Tìm folder Resources trong tất cả subfolder của Feature
        string resourcePath = FindResourceFolder(typeName);
        if (resourcePath == null)
        {
            Debug.LogWarning($"⚠ Bỏ qua {fileName}. Không tìm thấy folder: {typeName}/Resources trong {FEATURE_FOLDER}");
            return;
        }

        // tìm scriptableObject collection
        Type collectionType = FindType($"{typeName}Collection");
        if (collectionType == null)
        {
            Debug.LogWarning($"⚠ Không tìm thấy class: {typeName}Collection");
            return;
        }

        string assetPath = $"{resourcePath}/{assetName}.asset";
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
            Debug.LogError($"❌ {typeName}Collection không có biến public dataGroups");
            return;
        }

        string csvContent = File.ReadAllText(csvPath);
        var rows = ParseCsv(csvContent);

        var header = rows[0];
        rows.RemoveAt(0);

        object modelBuilt = BuildModel(modelField.FieldType, header, rows);
        modelField.SetValue(so, modelBuilt);

        EditorUtility.SetDirty(so);
        Debug.Log($"<color=#00d1ff>✔ Imported → {Path.GetFileName(csvPath)} → {assetName}.asset (using {typeName}Collection)</color>");
    }

    private static string FindResourceFolder(string featureName)
    {
        // Tìm tất cả folder có tên featureName trong FEATURE_FOLDER
        var featureDirs = Directory.GetDirectories(FEATURE_FOLDER, featureName, SearchOption.AllDirectories);

        foreach (var dir in featureDirs)
        {
            string resourcePath = Path.Combine(dir, "Resources").Replace("\\", "/");
            if (Directory.Exists(resourcePath))
            {
                return resourcePath;
            }
        }

        return null;
    }

    private static List<string[]> ParseCsv(string text)
    {
        return text.Replace("\r", "")
                   .Split('\n')
                   .Where(l => !string.IsNullOrWhiteSpace(l))
                   .Select(l => ParseCsvLine(l))
                   .ToList();
    }

    /// <summary>
    /// Parse 1 dòng CSV, xử lý đúng giá trị có chứa dấu phẩy được bọc trong quotes
    /// Ví dụ: 1,Damage,"1,0,1",3 → ["1", "Damage", "1,0,1", "3"]
    /// </summary>
    private static string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        bool inQuotes = false;
        string currentValue = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(currentValue);
                currentValue = "";
            }
            else
            {
                currentValue += c;
            }
        }

        result.Add(currentValue); // Thêm giá trị cuối cùng
        return result.ToArray();
    }

    private static Type FindType(string name)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == name);
    }

    private static object BuildModel(Type modelType, string[] header, List<string[]> rows)
    {
        // CASE A: dataGroups là MẢNG
        if (modelType.IsArray)
        {
            Type elementType = modelType.GetElementType();
            return BuildArrayRecursive(elementType, rows, 0);
        }

        // CASE B: dataGroups là 1 OBJECT
        if (rows.Count == 0)
            return null;

        string[] row = rows[0];
        int colIndex = 0;
        return BuildObjectWithNestedArray(modelType, rows, ref colIndex, 0);
    }

    /// <summary>
    /// Build mảng đệ quy - hỗ trợ mảng lồng mảng
    /// startCol: cột bắt đầu để check xem row có phải element mới không
    /// </summary>
    private static object BuildArrayRecursive(Type elementType, List<string[]> rows, int startCol)
    {
        if (rows.Count == 0)
            return Array.CreateInstance(elementType, 0);

        // Tính số cột của các field đơn giản (không phải array) trong element
        int simpleFieldCols = CountSimpleFieldColumns(elementType);

        // Group rows theo logic: cột startCol có giá trị = element mới
        var groups = new List<List<string[]>>();
        List<string[]> currentGroup = null;

        foreach (var row in rows)
        {
            if (row.Length <= startCol) continue;

            // Cột startCol có giá trị → element mới
            if (!string.IsNullOrWhiteSpace(row[startCol]))
            {
                currentGroup = new List<string[]>();
                groups.Add(currentGroup);
            }

            if (currentGroup != null)
                currentGroup.Add(row);
        }

        // Build từng element
        Array result = Array.CreateInstance(elementType, groups.Count);

        for (int i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            int colIndex = startCol;
            object element = BuildObjectWithNestedArray(elementType, group, ref colIndex, startCol);
            result.SetValue(element, i);
        }

        return result;
    }

    /// <summary>
    /// Build object có thể chứa nested array
    /// </summary>
    private static object BuildObjectWithNestedArray(Type type, List<string[]> rows, ref int colIndex, int startCol)
    {
        object obj = Activator.CreateInstance(type);
        string[] firstRow = rows[0];

        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var f in fields)
        {
            Type ft = f.FieldType;

            if (ft.IsArray)
            {
                // Đây là array field → build đệ quy với startCol mới
                Type arrayElementType = ft.GetElementType();
                object nestedArray = BuildArrayRecursive(arrayElementType, rows, colIndex);
                f.SetValue(obj, nestedArray);
                // Không tăng colIndex ở đây vì array đã xử lý các cột của nó
            }
            else if (IsSimpleType(ft))
            {
                // Simple type → lấy giá trị từ row đầu tiên
                if (colIndex < firstRow.Length && !string.IsNullOrWhiteSpace(firstRow[colIndex]))
                {
                    object val = ConvertValue(ft, firstRow[colIndex]);
                    f.SetValue(obj, val);
                }
                colIndex++;
            }
            else
            {
                // Nested object (không phải array) → đệ quy
                object nested = BuildObjectWithNestedArray(ft, rows, ref colIndex, startCol);
                f.SetValue(obj, nested);
            }
        }

        return obj;
    }

    /// <summary>
    /// Đếm số cột của các field đơn giản (không phải array)
    /// </summary>
    private static int CountSimpleFieldColumns(Type type)
    {
        int count = 0;
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var f in fields)
        {
            if (f.FieldType.IsArray)
                continue; // Bỏ qua array

            if (IsSimpleType(f.FieldType))
                count++;
            else
                count += CountSimpleFieldColumns(f.FieldType);
        }
        return count;
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

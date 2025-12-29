using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public abstract class DataPlayerBase
{
    public static Dictionary<string, DataPlayerBase> dataInstance = new Dictionary<string, DataPlayerBase>();
    //public static List<DataPlayerBase> listInstance = new List<DataPlayerBase>();
    public abstract void Load();
    public abstract void Save();
    public virtual void Clear() { }

    public static void Init()
    {
        LoadAllDerivedTypes();
    }

    public static void LoadAllDerivedTypes()
    {
        // Lấy tất cả các Feature Manager kế thừa từ DataPlayer trong Assembly hiện tại
        var derivedTypes = Assembly.GetExecutingAssembly().GetTypes()
          .Where(type => type.IsClass && !type.IsAbstract && type.BaseType != null
                 && type.BaseType.IsGenericType
                 && type.BaseType.GetGenericTypeDefinition() == typeof(DataPlayer<>));

        foreach (var type in derivedTypes)
        {
            // Tạo instance của mỗi derived class (Cách này thực chất là tạo Object cho mỗi loại chứ k phải tạo DataPlayerBase)
            DataPlayerBase instance = (DataPlayerBase)Activator.CreateInstance(type);
            if (!dataInstance.ContainsKey(type.Name))
                dataInstance.Add(type.Name, instance);

            instance.Load();

            //listInstance.Add(instance);

            
        }
    }

    /// <summary>
    /// Lấy module từ Dictionary theo Type T1
    /// </summary>
    public static T1 GetModule<T1>() where T1 : DataPlayerBase
    {
        // Lấy tên của Type T1
        string typeName = typeof(T1).Name;

        // Kiểm tra trong Dictionary
        if (dataInstance.TryGetValue(typeName, out var instance))
        {
            return (T1)instance; // Tìm thấy, trả về instance
        }

        T1 newInstance = Activator.CreateInstance<T1>();

        if (!dataInstance.ContainsKey(typeName))
            dataInstance.Add(typeName, newInstance);
        //listInstance.Add(newInstance);

        return newInstance;
    }
}

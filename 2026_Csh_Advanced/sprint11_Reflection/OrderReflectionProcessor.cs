using System.Reflection;

namespace _2026_Csh_Advanced.sprint11_Reflection;

public static class OrderReflectionProcessor
{
    public static void GenerateAuditLog(object obj)
    {
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(obj);
            bool isSensitive = property.IsDefined(typeof(SensitiveDataAttribute), false);
            if (isSensitive) value = "********";
            Console.WriteLine(
                $"Property: {property.Name}, PropertyType.Name: {property.PropertyType.Name}, Value: {value}");
        }
    }

    public static bool Validate(object obj, out string errorMessage)
    {
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            var rangeAttr = property.GetCustomAttribute<OrderRangeAttribute>();
            if (rangeAttr != null)
            {
                var value = property.GetValue(obj);
                if (value is not double doubleValue) continue;
                if (doubleValue >= rangeAttr.Max || doubleValue <= rangeAttr.Min)
                {
                    errorMessage =
                        $"Property {property.Name} with value {doubleValue} is out of range [{rangeAttr.Min}, {rangeAttr.Max}]";
                    return false;
                }
            }
        }

        errorMessage = string.Empty;
        return true;
    }
}
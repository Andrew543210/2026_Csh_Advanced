namespace _2026_Csh_Advanced.sprint11_Reflection;

[AttributeUsage(AttributeTargets.Property)]
public class OrderRangeAttribute : Attribute
{
    public double Min { get; }
    public double Max { get; }

    public OrderRangeAttribute(double min, double max)
    {
        Min = min;
        Max = max;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class SensitiveDataAttribute : Attribute
{
    
}

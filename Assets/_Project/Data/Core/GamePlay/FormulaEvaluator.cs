using System;
using System.Data;
using System.Globalization;
using System.Linq;

public static class FormulaEvaluator
{
    /// <summary>
    /// a: value base, b: bonus level, c: level
    /// </summary>
    /// <param name="formula"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static float Evaluate(
        string formula,
        float a,
        float b,
        float c
    )
    {
        string exp = formula
            .Replace("a", a.ToString(CultureInfo.InvariantCulture))
            .Replace("b", b.ToString(CultureInfo.InvariantCulture))
            .Replace("c", c.ToString(CultureInfo.InvariantCulture));

        return Convert.ToSingle(
            new DataTable().Compute(exp, ""),
            CultureInfo.InvariantCulture
        );
    }

    public static int EvaluateLevel(float d, float a, float b)
    {
        if (b == 0)
            throw new DivideByZeroException("b must not be 0");

        float value = (d - a) / b;

        if (value < 0) return (int)value + 1;
        return (int)value + 2;
    }

    /// <summary>
    /// convert string sang mảng float
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float[] ConvertStringToFloat(string value)
    {
        return value
            .Split(',')
            .Select(s => float.Parse(
                s.Trim(),
                CultureInfo.InvariantCulture
            ))
            .ToArray();
    }
}

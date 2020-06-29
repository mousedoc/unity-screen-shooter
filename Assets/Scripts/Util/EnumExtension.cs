using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumExtension<T> where T : struct, IConvertible
{
    public static bool IsEnum { get => typeof(T).IsEnum; }

    public static int Count
    {
        get
        {
            if (IsEnum == false)
                return 0;

            return Enum.GetValues(typeof(T)).Length;
        }
    }

    public static IEnumerable<T> Enumerable
    {
        get
        {
            if (IsEnum == false)
                return null;

            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }

    public static T Parse(string input)
    {
        return (T)Enum.Parse(typeof(T), input);
    }

    public static List<T> List
    {
        get
        {
            if (IsEnum == false)
                return null;

            return Enumerable?.ToList();
        }
    }

    public static string[] Names
    {
        get
        {
            if (IsEnum == false)
                return null;

            return Enum.GetValues(typeof(T)).Cast<T>().Select(elem => elem.ToString()).ToArray();
        }
    }
}
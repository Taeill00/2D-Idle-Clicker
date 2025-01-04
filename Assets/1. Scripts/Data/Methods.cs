using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Methods 
{
    public static int notation = 0;

    public static string Notate(this BigDouble number)
    {
        switch (notation)
        {
            case 0:
                return "Lol";
            case 1:
                return "scienceee";
        }

        return "";
    }

    public static List<T> CreateList<T>(int capacity)
    {
        return Enumerable.Repeat(default(T), capacity).ToList();
    }

    public static void UpgradeCheck<T>(List<T> list, int length) where T : new() // 제네릭 타입엔 기본생성자를 가진 (ex 클래스같은 애들) 만 들어갈수있음
    {
        try
        {
            if(list.Count == 0)
                list = new T[length].ToList();

            while(list.Count < length)
                list.Add(new T());
        }
        catch 
        {
            list = CreateList<T>(length);
        }
    }
}

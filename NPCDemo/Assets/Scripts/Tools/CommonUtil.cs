using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 工具类
/// </summary>
public static class CommonUtil 
{
    /// <summary>
    /// 根据T值，计算贝塞尔曲线上面相对应的点
    /// </summary>
    /// <param name="t"></param>T值
    /// <param name="p0"></param>起始点
    /// <param name="p1"></param>控制点
    /// <param name="p2"></param>目标点
    /// <returns></returns>根据T值计算出来的贝赛尔曲线点
    private static Vector2 CalculateCubicBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector2 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    /// <summary>
    /// 获取存储贝塞尔曲线点的数组
    /// </summary>
    /// <param name="startPoint"></param>起始点
    /// <param name="controlPoint"></param>控制点
    /// <param name="endPoint"></param>目标点
    /// <param name="segmentNum"></param>采样点的数量
    /// <returns></returns>存储贝塞尔曲线点的数组
    public static Vector2[] GetBeizerList(Vector2 startPoint, Vector2 controlPoint, Vector2 endPoint, int segmentNum)
    {
        Vector2[] Path = new Vector2[segmentNum+1];
        Path[0] = startPoint;
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector2 pixel = CalculateCubicBezierPoint(t, startPoint,
                controlPoint, endPoint);
            Path[i] = pixel;

        }
        return Path;

    }


    static public double ToDouble(this string str)
    {
        double ret = 0;

        try
        {
            if (!String.IsNullOrEmpty(str))
            {
                bool success = double.TryParse(str, out ret);
                if (!success)
                    Debug.LogError("str无法转double---" + str);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }

    static public float ToFloat(this string str)
    {
        float ret = 0;

        try
        {
            if (!String.IsNullOrEmpty(str))
            {
                bool success = float.TryParse(str, out ret);
                if (!success)
                    Debug.LogError("str无法转float---" + str);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }
    static public byte ToByte(this string str)
    {
        byte ret = 0;

        try
        {
            if (!String.IsNullOrEmpty(str))
            {
                ret = Convert.ToByte(str);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }
    static public Int32 ToInt32(this string str)
    {
        Int32 ret = 0;

        try
        {
            if (!String.IsNullOrEmpty(str))
            {
                ret = Convert.ToInt32(str);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }
    static public UInt64 ToUInt64(this string str)
    {
        UInt64 ret = 0;

        try
        {
            if (!String.IsNullOrEmpty(str))
            {
                ret = Convert.ToUInt64(str);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }
    static public Int32 ToInt32(this double val)
    {
        Int32 ret = 0;

        try
        {

            ret = Convert.ToInt32(val);

        }
        catch (Exception)
        {
        }
        return ret;
    }


    /// <summary>
    /// 获取ab的x距离
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float GetDistanceX(Transform a,Transform b)
    {
        return Mathf.Abs(b.position.x - a.position.x);

    }

    /// <summary>
    /// 把2|10001|20$1|10002|30,2|10001|20$1|10002|30转换为{[[2,10001,20],[1,10002,30]],[[2,10001,20],[1,10002,30]]
    /// </summary>
    /// <returns></returns>
    public static List<List<List<int>>> SplitThreeCfg(string source)
    {
        string[] str = source.Split(',');
        List<List<List<int>>> res = new List<List<List<int>>>();

        for (int i = 0; i < str.Length; i++)
        {
            string str1 = str[i];
            string[] arr1 = str1.Split('$');
            List<List<int>> list2 = new List<List<int>>();
            for (int j = 0; j < arr1.Length; j++)
            {
                string str2 = arr1[j];
                string[] arr2 = str2.Split('|');
                List<int> list3 = new List<int>();
                for (int k = 0; k < arr2.Length; k++)
                {
                    string str3 = arr2[k];
                    list3.Add(str3.ToInt32());
                }
                list2.Add(list3);
            }
            res.Add(list2);
        }


        return res;
    }


    /// <summary>
    /// 把2|10001|20$1|10002|30转换为[[2,10001,20],[1,10002,30]]
    /// </summary>
    /// <returns></returns>
    public static List<List<int>> SplitCfg(string source)
    {
        string[] str = source.Split('$');
        List<List<int>> totalList = new List<List<int>>();

        for (int i = 0; i < str.Length; i++)
        {
            string singleStr = str[i];
            string[] singleArr = singleStr.Split('|');

            List<int> smallList = new List<int>();
            for (int j = 0; j < singleArr.Length; j++)
            {
                int theInt = singleArr[j].ToInt32();
                smallList.Add(theInt);
            }
            totalList.Add(smallList);
        }
        return totalList;
    }

    /// <summary>
    /// 把1|10002|30转换为[1,10002,30]
    /// </summary>
    /// <returns></returns>
    public static List<int> SplitCfgOneDepth(string source)
    {

        string[] singleArr = source.Split('|');

        List<int> res = new List<int>();
        for (int j = 0; j < singleArr.Length; j++)
        {
            int theInt = singleArr[j].ToInt32();
            res.Add(theInt);
        }

        return res;
    }

    /// <summary>
    /// 通过权重得到第几项
    /// </summary>
    /// <returns></returns>
    public static int GetIndexByWeight(List<int> weightList)
    {
        int totalWeight = 0;

        for (int i = 0; i < weightList.Count; i++)
        {
            totalWeight += weightList[i];
        }

        //上下限
        List<int> AList = new List<int>();
        List<int> BList = new List<int>();

        //int ultra = 1000;
        //int val = RandomManager.Next(1, (totalWeight + 1)* ultra);
        //val = 10000;
        //int index = val % totalWeight;

        int index = RandomManager.Next(1, (totalWeight + 1));

        for (int j = 0; j < weightList.Count; j++)
        {
            int currWeight = weightList[j];
            int A = 1;
            int B = 0;
            if (j > 0)
            {
                for (int k = 0; k < j; k++)
                {
                    A = A + weightList[k];
                }
            }
            else
            {
                A = 1;
            }
            B = A - 1 + currWeight;
            AList.Add(A);
            BList.Add(B);
        }
        int choosedIndex = 0;
        for (int i = 0; i < AList.Count; i++)
        {
            int theA = AList[i];
            int theB = BList[i];
            if (index >= theA && index <= theB)
            {
                choosedIndex = i;
                break;
            }
        }
        return choosedIndex;

    }
    /// <summary>
    /// 通过权重得到第几项string数组
    /// </summary>
    /// <returns></returns>
    public static int GetIndexByWeight(List<string> weightList)
    {
        int totalWeight = 0;
        List<int> weightIntList = new List<int>();
        for (int i = 0; i < weightList.Count; i++)
        {
            weightIntList.Add(weightList[i].ToInt32());
        }

        for (int i = 0; i < weightIntList.Count; i++)
        {
            totalWeight += weightIntList[i];
        }

        //上下限
        List<int> AList = new List<int>();
        List<int> BList = new List<int>();

        //int ultra = 1000;
        //int val = RandomManager.Next(1, (totalWeight + 1)* ultra);
        //val = 10000;
        //int index = val % totalWeight;

        int index = RandomManager.Next(1, (totalWeight + 1));

        for (int j = 0; j < weightIntList.Count; j++)
        {
            int currWeight = weightIntList[j];
            int A = 1;
            int B = 0;
            if (j > 0)
            {
                for (int k = 0; k < j; k++)
                {
                    A = A + weightIntList[k];
                }
            }
            else
            {
                A = 1;
            }
            B = A - 1 + currWeight;
            AList.Add(A);
            BList.Add(B);
        }
        int choosedIndex = 0;
        for (int i = 0; i < AList.Count; i++)
        {
            int theA = AList[i];
            int theB = BList[i];
            if (index >= theA && index <= theB)
            {
                choosedIndex = i;
                break;
            }
        }
        return choosedIndex;

    }


}

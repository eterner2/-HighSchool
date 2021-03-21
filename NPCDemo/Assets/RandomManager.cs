using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 随机数控制
/// </summary>
public class RandomManager 
{
    /// <summary>
    /// 下限包含上限不包含
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int Next(int a,int b)
    {
        var seed = Guid.NewGuid().GetHashCode();
        Random r = new Random(seed);
        int i= r.Next(a, b);
        return i;
    }
 
}

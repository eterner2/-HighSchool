using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// mao 时间类
/// </summary>
public class CGameTime
{
    static CGameTime inst;

    public static CGameTime Instance
    {
        get
        {
            if (inst == null)
                inst = new CGameTime();
            return inst;
        }
    }

    public void GetTimeStampFromApplicationStart()
    {
        //Time.realtimeSinceStartup
    }

    /// <summary>
    /// 时间戳转日期
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public DateTime GetDateTimeByTimeStamp(long timeStamp)
    {
        DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
        DateTime dt = startTime.AddSeconds(timeStamp);
        return dt;
    }

    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <returns></returns>
    public long GetTimeStamp()
    {
        TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }
    /// <summary>
    /// 获取时间戳 毫秒
    /// </summary>
    /// <returns></returns>
    public long GetTimeStampMiliSecond()
    {
        TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }


    /// <summary>
    /// 得到某天24点时间戳
    /// </summary>
    /// <returns></returns>
    public long GetTo24TimeStamp(DateTime date)
    {
       DateTime the24Time= Convert.ToDateTime(date.AddDays(1).ToString("D").ToString()).AddSeconds(-1);
        TimeSpan ts = the24Time - DateTime.Now;
        return (long)ts.TotalSeconds;
    }

    /// <summary>
    /// 传入当前时间戳，得到24点时间戳
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public long GetTo24TimeStampByTimeStamp(long timeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        DateTime dt = dateTime.AddSeconds(timeStamp);
        long res = GetTo24TimeStamp(dt);
        Debug.Log("举例24点还有" + res);
        return res;
    }
}

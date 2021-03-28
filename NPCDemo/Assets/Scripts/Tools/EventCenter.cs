using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件中心
/// </summary>
public class EventCenter 
{
    private static EventCenter inst = null;

    public static EventCenter Instance
    {
        get
        {
            if (inst == null)
            {
                inst = new EventCenter();
            }
            return inst;
        }
    }

    //
    public static Dictionary<TheEventType,List<Action>> eventDic = new Dictionary<TheEventType, List<Action>>();

    public static Dictionary<TheEventType,List<Action<object[]>>> eventDicWithParam = new Dictionary<TheEventType, List<Action<object[]>>>();

    /// <summary>
    /// 移除不带参数的消息
    /// </summary>
    /// <param name="theType"></param>
    /// <param name="callBack"></param>
    public static void Remove(TheEventType theType, Action callBack)
    {
        if (eventDic.ContainsKey(theType))
        {
            List<Action> theTypeList = eventDic[theType];
            if (theTypeList.Contains(callBack))
            {
                theTypeList.Remove(callBack);
            }
            if (theTypeList.Count == 0)
                eventDic.Remove(theType);
        }    
    }

    /// <summary>
    /// 移除带参数的消息
    /// </summary>
    /// <param name="theType"></param>
    /// <param name="callBack"></param>
    public static void Remove(TheEventType theType, Action<object[]> callBack)
    {
        if (eventDicWithParam.ContainsKey(theType))
        {
            List<Action<object[]>> theTypeList = eventDicWithParam[theType];
            if (theTypeList.Contains(callBack))
            {
                theTypeList.Remove(callBack);
            }
            if (theTypeList.Count == 0)
                eventDicWithParam.Remove(theType);
        }
    }
    public static void Register(TheEventType theType, Action callBack)
    {
        if (!eventDic.ContainsKey(theType))
            eventDic.Add(theType, new List<Action>());

        if (!eventDic[theType].Contains(callBack))
        {
            eventDic[theType].Add(callBack);
        }
    }

    public static void Register(TheEventType theType, Action<object[]> callBack)
    {
        if (!eventDicWithParam.ContainsKey(theType))
            eventDicWithParam.Add(theType, new List<Action<object[]>>());
        if (!eventDicWithParam[theType].Contains(callBack))
        {
            eventDicWithParam[theType].Add(callBack);
        }
    }

    public static void Broadcast(TheEventType type)
    {
        if (eventDic.ContainsKey(type))
        {
            List<Action> theCallBackList = eventDic[type];

            for(int i = 0; i < theCallBackList.Count; i++)
            {
                theCallBackList[i]();
            }
        }
    }
    /// <summary>
    /// d
    /// </summary>
    /// <param name="type"></param>
    public static void Broadcast(TheEventType type ,params object[] param)
    {
        if (eventDicWithParam.ContainsKey(type))
        {
            List<Action<object[]>> theCallBackList = eventDicWithParam[type];

            for (int i = 0; i < theCallBackList.Count; i++)
            {
                theCallBackList[i](param);
            }

        }
        if (eventDic.ContainsKey(type))
        {
            List<Action> theCallBackList = eventDic[type];

            for (int i = 0; i < theCallBackList.Count; i++)
            {
                theCallBackList[i]();
            }
        }
    }
    /// <summary>
    /// 清除所有事件
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
        eventDicWithParam.Clear();
    }

    ///// <summary>
    ///// 用法
    ///// </summary>

    //public void RegisterTest()
    //{
    //    EventCenter.Register(EventType.None, Foo);
    //}


    //public void Foo(params object[] t)
    //{

    //}

    //public void BroadcastTest()
    //{
    //    int a = 0;
    //    int b = 1;
    //    EventCenter.Broadcast(EventType.None, a, b);
    //}
}


/// <summary>
/// 事件类型
/// </summary>
public enum TheEventType
{
    None=0,
    DayTimeProcess,//一天时间流动
    GetStudyScore,//得到学习分
    
}

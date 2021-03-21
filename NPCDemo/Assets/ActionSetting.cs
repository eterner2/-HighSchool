using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ActionSetting
{
    public int id;//id
    public string theName;
    public ActionType type;
    public string propertyChange;//该行为对某属性的影响 用|和$隔开
    public ActionSocializationProperty socializationProperty;//社交属性
    public int costTime;//耗费时间
}
/// <summary>
/// 每日选择类型
/// </summary>
public enum ActionType
{
    None = 0,
    PlayBRPG,//玩桌游
    KaraOK,//卡拉OK
    StrollAtBehindMountain,//后山闲逛
    Shopping,//去逛街
    GoLibraryStudy,//去图书馆学习
    SleepAtDormitory,//宿舍睡觉
    BeVolunteerAtLib,//在图书馆当志愿者
    DistributeLeaflets,//发传单
    PlayGame,//打游戏
}

/// <summary>
/// 行为的社交属性
/// </summary>
public enum ActionSocializationProperty
{
    None = 0,
    MustAlone = 1,//必须自己来
    CanWithOthers = 2,//可以和其他人一起
    MustWithOthers = 3,//必须和其它人一起
}
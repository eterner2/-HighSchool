

using System;
using UnityEngine;

public class ConstantVal
{
    static UInt64 onlyId { get; set; }

    static UInt16 growId;//自增长

    public static UInt64 SetId
    {
        get
        {
            RoleManager.Instance._CurGameInfo.TheId++;
            return RoleManager.Instance._CurGameInfo.TheId;
            //growId++;
            //onlyId = (UInt64)(CGameTime.Instance.GetTimeStamp() * 100000000 + growId * 1000 + UnityEngine.Random.Range(0, 1000));
            //return onlyId;
        }
    }
    /// <summary>
    /// 所有panel暂时放该文件夹
    /// </summary>
    public const string PanelPath = "UIPanel";
    public const string propertyIconFolderPath = "TestRes/Common/Property/";//属性icon文件夹
    public const string bigMapFolderPath = "TestRes/BigMap/";//大地图icon文件夹
    public const string actionSceneFolderPath = "TestRes/ActionScene/";//行动场景文件夹
    public const string verticalDrawFolderPath = "TestRes/Common/PeopleVerticalDraw/";//人物立绘文件夹
    public const string maleIcon = "icon_man";//男头像
    public const string femaleIcon = "icon_girl";//女头像

    public const string battleHitEffectPath = "TestRes/Battle/effect/BattleHitEffect";//受击
    public const string loseHPEffectPath = "TestRes/Battle/effect/LoseHPEffect";//掉血

    /// <summary>
    /// 通过Panel名字获取路径
    /// </summary>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public static string GetPanelPath(string panelName)
    {
        return PanelPath + "/" + panelName;
    }

    /// <summary>
    /// 获取文件在流目录的路径
    /// </summary>
    /// <returns></returns>
    public static string GetFileInStreamPath(string file)
    {
#if UNITY_IOS
        return "file://"+ Application.streamingAssetsPath + "/" + file;
#else
        return Application.streamingAssetsPath + "/" + file;
#endif
    }

    /// <summary>
    /// 获取文件在持久化目录的路径
    /// </summary>
    /// <returns></returns>
    public static string GetFileInPersistentPath(string file)
    {
        //#if UNITY_IOS
        // return "file://" + Application.persistentDataPath + "/" + file;
        //#else
        return Application.persistentDataPath + "/" + file;
        //#endif
    }

    /// <summary>
    /// 获取version的持久化目录
    /// </summary>
    /// <returns></returns>
    public static string GetVersionPersistentPath()
    {
        //#if UNITY_IOS
        return Application.persistentDataPath + "/theVersion.txt";

        //#else
        //return Application.persistentDataPath + "/theVersion.txt";
        //#endif
    }
    /// <summary>
    /// 获取version的流目录
    /// </summary>
    /// <returns></returns>
    public static string GetVersionStreamPath()
    {
#if UNITY_IOS
        return "file://" +Application.streamingAssetsPath + "/theVersion.txt";
#else
        return Application.streamingAssetsPath + "/theVersion.txt";
#endif
    }
}

/// <summary>
/// 属性id
/// </summary>
public enum PropertyIdType
{
    Study=10001,//学习
    Art=10002,//艺术
    Physical=10003,//体育
    Money=10004,//钱
    TiLi=10005,//体力
    Mood=10006,//心情
    SelfControl=10007,//自制
    Charm=10008,//魅力

    //考试战斗属性
    Hp = 10009,//精力
    Attack = 10010,//学习能力
    Defense = 10011,//学习心态
    CritRate = 10012,//暴击率
    Speed = 10013,//速度
    SkillAdd=10014,//技能增伤
    CritNum=10015,//爆伤

    Score=10016,//分数

    StudyCharm = 10017,//学习魅力
    StudyDefense =10018,//学习抵抗
    PhysicalCharm = 10019,//体育魅力
    PhysicalDefense= 10020,//体育抗性
    ArtCharm=10021,//艺术魅力
    ArtDefense=10022,//艺术抗性
}

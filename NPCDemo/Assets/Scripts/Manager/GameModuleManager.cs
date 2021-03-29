using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏模块类 目前分战斗模块 工作日模块和周末模块三大模块
/// </summary>
public class GameModuleManager : CommonInstance<GameModuleManager>
{
    public GameModuleType curGameModule;

    public override void Init()
    {
        curGameModule = (GameModuleType)RoleManager.Instance._CurGameInfo.CurGameModule;
        base.Init();

    }
}

/// <summary>
/// 当前是哪个模块
/// </summary>
public enum GameModuleType
{
    None=0,
    WeekDay=1,//教室工作日模块
    WeekEnd=2,//周末模块
    Battle=3,//战斗模块
}
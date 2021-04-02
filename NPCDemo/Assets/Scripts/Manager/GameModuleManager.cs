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


    /// <summary>
    /// 换模块
    /// </summary>
    /// <param name="gameModuleType"></param>
    public void ChangeGameModule(GameModuleType gameModuleType)
    {
        PanelManager.Instance.BlackMask(BlackMaskType.PingPong, () => 
        {


            RoleManager.Instance._CurGameInfo.CurGameModule = (int)gameModuleType;
            curGameModule = gameModuleType; //(GameModuleType)RoleManager.Instance._CurGameInfo.CurGameModule;

            


            PanelManager.Instance.InitPanel(curGameModule);

        }
        
        );

    }

    /// <summary>
    /// 初始化模块的系统
    /// </summary>
    public void InitGameModuleManager(GameModuleType gameModuleType)
    {

    }


}

/// <summary>
/// 当前是哪个模块
/// </summary>
public enum GameModuleType
{
    None=0,
    WeekDay=1,//教室工作日模块
    Home=2,//家模块
    Battle=3,//战斗模块
    BigMap=4,//大地图
    SingleOutsideScene,//单个外面场景
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏模块类 目前分战斗模块 工作日模块和周末模块三大模块
/// </summary>
public class GameModuleManager : CommonInstance<GameModuleManager>
{
    public GameModuleType curGameModule;
    //public int curEnterActionId;//当前进入的活动场景

    public override void Init()
    {
        curGameModule = (GameModuleType)RoleManager.Instance._CurGameInfo.CurGameModule;
        base.Init();

    }
    /// <summary>
    /// 进入该模块 要初始化
    /// </summary>
    /// <param name="gameModuleType"></param>
    public void InitGameModule(GameModuleType gameModuleType)
    {
        InitGameModuleManager(gameModuleType);
        ChangeGameModule(gameModuleType);
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



            //初始化模块的panel
            PanelManager.Instance.InitPanel(curGameModule);

        }
        
        );

    }

    /// <summary>
    /// 初始化模块的系统
    /// </summary>
    public void InitGameModuleManager(GameModuleType gameModuleType)
    {
        switch (gameModuleType)
        {

            case GameModuleType.Home:
                //邀约
                SocializationManager.Instance.StartNewInvite();
                break;
            //case GameModuleType.BigMap:
            //    OpenPanel<BigMapPanel>(trans_commonPanelParent);
            //    OpenPanel<StatusPanel>(trans_layer2);

            //    break;
            //case GameModuleType.SingleOutsideScene:

            //    OpenPanel<OutsidePanel>(trans_commonPanelParent, SocializationManager.Instance.action_planDic[GameModuleManager.Instance.curEnterActionId]);
            //    //OpenPanel<BigMapPanel>(trans_commonPanelParent);
            //    OpenPanel<StatusPanel>(trans_layer2);

            //    break;
        }
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
using Framework.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 行动准备界面
/// </summary>
public class ActionReadyPanel : PanelBase
{
    public Text txt_label;
    public Text txt_des;
    public Image img;
    public ActionSetting actionSetting;
    public Transform grid;
    public List<ActionReadyPanelPeopleView> actionReadyPanelPeopleViewList = new List<ActionReadyPanelPeopleView>();
    public Text txt_choosedPeopleNum;//选择了多少个人
    public Button btn_invitePeople;//邀请人
    public People choosedWithPeople;//选了谁
    public Button btn_go;
    public Button btn_close;
    public Transform trans_award;//奖励

    public override void Init(params object[] args)
    {
        base.Init(args);
        int id = (int)args[0];
        SocializationManager.Instance.SetTmpPreferedActionId(id);
        actionSetting = DataTable.FindActionSetting(id);

        EventCenter.Register(TheEventType.NPCAppliedInvite, OnNPCAppliedInvite);
        addBtnListener(btn_invitePeople, () =>
        {
            PanelManager.Instance.OpenPanel<CellPhonePanel>(PanelManager.Instance.trans_layer2, CellphoneHandleType.Invite);
        });

        addBtnListener(btn_go, () =>
         {
            //和他一起去
            if (choosedWithPeople != null)
             {
                 RoleManager.Instance.playerPeople.Record("和" + choosedWithPeople.protoData.Name + "一起" + actionSetting.name);
                 choosedWithPeople.Record("和" + RoleManager.Instance.playerPeople + "一起" + actionSetting.name);
                 SocializationManager.Instance.AddPlan(actionSetting.name, actionSetting.id.ToInt32(), RoleManager.Instance.playerPeople, choosedWithPeople);
                //分别拒绝掉各自的鱼
                 SocializationManager.Instance.RefusePeopleWhoInviteMe(RoleManager.Instance.playerPeople, choosedWithPeople, actionSetting.name);
                 SocializationManager.Instance.ForgivePeopleWhoIInvite(RoleManager.Instance.playerPeople, choosedWithPeople, actionSetting.name);

                 SocializationManager.Instance.RefusePeopleWhoInviteMe(choosedWithPeople, RoleManager.Instance.playerPeople, actionSetting.name);
                 SocializationManager.Instance.ForgivePeopleWhoIInvite(choosedWithPeople, RoleManager.Instance.playerPeople, actionSetting.name);

                //这里执行其它npc的邀约
                SocializationManager.Instance.HandleInvite(RoleManager.Instance.allPeopleList);
                 GameModuleManager.Instance.InitGameModule(GameModuleType.SingleOutsideScene);

                 //其它NPC执行邀约完毕以后，要给玩家发个信息
                 SocializationManager.Instance.NPCReactionAfterPeopleChoosePlan(choosedWithPeople);


             }
             else
             {
                //弹窗问独自去还是和人一起去

                PanelManager.Instance.OpenCommonHint("确定独自一个人" + actionSetting.name + "吗？", () =>
                 {
                     RoleManager.Instance.playerPeople.Record("决定独自去" + actionSetting.name);
                     SocializationManager.Instance.AddPlan(actionSetting.name, actionSetting.id.ToInt32(), RoleManager.Instance.playerPeople);
                     //拒绝掉我的鱼

                     SocializationManager.Instance.HandleInvite(RoleManager.Instance.allPeopleList);
                     SocializationManager.Instance.NPCReactionAfterPeopleChoosePlan(choosedWithPeople);

                     GameModuleManager.Instance.InitGameModule(GameModuleType.SingleOutsideScene);
                 },
                      null);
             }

         });

        addBtnListener(btn_close, () =>
        {
            PanelManager.Instance.ClosePanel(this);
        });

       
  
    }

    

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        ShowValidWithPeople();
        txt_label.SetText(actionSetting.name);
        txt_des.SetText(actionSetting.des);
        int theBigMapId = actionSetting.bigMapId.ToInt32();
        img.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.actionSceneFolderPath + DataTable.FindBigMapSetting(theBigMapId).sceneImgName);
        ShowChoosedPeopleNum();
    }

    /// <summary>
    /// 显示可以一起去的人
    /// </summary>
    void ShowValidWithPeople()
    {
        for (int i = 0; i < RoleManager.Instance.playerPeople.protoData.ValidWithPeople.Count; i++)
        {
            UInt64 onlyId = RoleManager.Instance.playerPeople.protoData.ValidWithPeople[i];
            People people = RoleManager.Instance.FindPeopleWithOnlyId(onlyId);
            ActionReadyPanelPeopleView actionReadyPanelPeopleView = PanelManager.Instance.OpenSingle<ActionReadyPanelPeopleView>(grid, people);
            actionReadyPanelPeopleView.parentPanel = this;
            actionReadyPanelPeopleViewList.Add(actionReadyPanelPeopleView);
        }
    }

    /// <summary>
    /// NPC答应了邀请
    /// </summary>
    void OnNPCAppliedInvite()
    {
        ClearValidPeople();
        ShowValidWithPeople();
        ShowChoosedPeopleNum();
    }

    /// <summary>
    /// 选择了同伴
    /// </summary>
   public void OnChoosedFriend(ActionReadyPanelPeopleView view)
    {
        for (int i = 0; i < actionReadyPanelPeopleViewList.Count; i++)
        {
            ActionReadyPanelPeopleView theView = actionReadyPanelPeopleViewList[i];
            if (view== theView)
            {
                theView.Choose(true);
            }
            else
            {
                theView.Choose(false);
            }
        }
        ShowChoosedPeopleNum();
    }

    void ShowChoosedPeopleNum()
    {
        int num = 0;
        for(int i=0;i< actionReadyPanelPeopleViewList.Count; i++)
        {
            if (actionReadyPanelPeopleViewList[i].choosed)
            {
                choosedWithPeople = actionReadyPanelPeopleViewList[i].people;
                num++;

            }

        }
        txt_choosedPeopleNum.SetText("选择同伴("+num+"/"+"1)");

    }

    public override void Clear()
    {
        base.Clear();
        ClearValidPeople();
        EventCenter.Remove(TheEventType.NPCAppliedInvite, OnNPCAppliedInvite);

    }

    /// <summary>
    /// 清理掉格子的人
    /// </summary>
    void ClearValidPeople()
    {
        PanelManager.Instance.CloseAllSingle(grid);
        actionReadyPanelPeopleViewList.Clear();
        choosedWithPeople = null;

    }
}


public enum ActionIdType
{
    None=0,
    DoMockExam=10005,//学习
}
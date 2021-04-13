using Framework.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
///选择行为按钮
/// </summary>
public class ChooseActionBtnView : SelfAdaptionChooseBtnView
{
    ActionSetting actionSetting;
    public Button btn_choose;
    People withPeople = null;
    public override void Init(params object[] args)
    {
        base.Init(args);
        int theId = (int)args[1];
        actionSetting = DataTable.FindActionSetting(theId);
        txt.SetText(actionSetting.name);


    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        addBtnListener(btn_choose, () =>
        {
            //这里直接弹出战斗准备窗口 （看要不要选天赋）

            PanelManager.Instance.OpenPanel<ActionReadyPanel>(PanelManager.Instance.trans_layer2, actionSetting.id.ToInt32());

            //PanelManager.Instance.OpenCommonHint("是否要去" + actionSetting.name, () =>
            //  {
            //      //GameModuleManager.Instance.curEnterActionId = actionSetting.id.ToInt32();
            //      //和他一起去
            //      if (appliedActionWithPeopleOnlyId != 0)
            //      {
            //          RoleManager.Instance.playerPeople.Record("答应了" +withPeople.protoData.Name+"的邀请，一起"+ actionSetting.name);
            //          withPeople.Record(RoleManager.Instance.playerPeople + "答应了邀请，一起" +actionSetting.name);
            //          SocializationManager.Instance.AddPlan(actionSetting.name, actionSetting.id.ToInt32(), RoleManager.Instance.playerPeople, withPeople);
            //          //分别拒绝掉各自的鱼
            //          SocializationManager.Instance.RefusePeopleWhoInviteMe(RoleManager.Instance.playerPeople, withPeople, actionSetting.name);
            //          SocializationManager.Instance.ForgivePeopleWhoIInvite(RoleManager.Instance.playerPeople, withPeople, actionSetting.name);

            //          SocializationManager.Instance.RefusePeopleWhoInviteMe(withPeople, RoleManager.Instance.playerPeople, actionSetting.name);
            //          SocializationManager.Instance.ForgivePeopleWhoIInvite(withPeople, RoleManager.Instance.playerPeople, actionSetting.name);

            //          //这里执行其它npc的邀约
            //          SocializationManager.Instance.HandleInvite(RoleManager.Instance.allPeopleList);
            //          GameModuleManager.Instance.InitGameModule(GameModuleType.SingleOutsideScene);
            //      }
            //      else
            //      {  
            //          //弹窗问独自去还是和人一起去

            //          PanelManager.Instance.OpenCommonHint("想独自" + actionSetting.name + "还是邀请好友一起？", () =>
            //               {
            //                   RoleManager.Instance.playerPeople.Record("决定独自去" + actionSetting.name);
            //                   SocializationManager.Instance.AddPlan(actionSetting.name, actionSetting.id.ToInt32(), RoleManager.Instance.playerPeople);
            //                   SocializationManager.Instance.HandleInvite(RoleManager.Instance.allPeopleList);
            //                   GameModuleManager.Instance.InitGameModule(GameModuleType.SingleOutsideScene);
            //               },
            //               ()=> 
            //               {
            //                   SocializationManager.Instance.tmpPreferedActionId = actionSetting.id.ToInt32();
            //                   PanelManager.Instance.ClosePanel(parentPanel);
            //                   PanelManager.Instance.OpenPanel<CellPhonePanel>(PanelManager.Instance.trans_layer2, CellphoneHandleType.Invite);
            //               },
            //               "独自",
            //               "邀请好友");
            //      }
               
        
            //  },null);
        });
    }


    //public void SetSize(Vector2 vec)
    //{
    //    Vector2 bgVec = new Vector2(vec.x - rectXOffsetContent_BG, vec.y - rectYOffsetContent_BG);
    //    rect_bg.sizeDelta = bgVec;
    //    rect.sizeDelta = vec;
    //}
}

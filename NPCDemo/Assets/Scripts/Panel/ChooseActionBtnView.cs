using Framework.Data;
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
    public override void Init(params object[] args)
    {
        base.Init(args);
        int theId = (int)args[0];
        actionSetting = DataTable.FindActionSetting(theId);
        txt.SetText(actionSetting.name);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        addBtnListener(btn_choose, () =>
        {
            PanelManager.Instance.OpenCommonHint("是否要去" + actionSetting.name, () =>
              {
                  //GameModuleManager.Instance.curEnterActionId = actionSetting.id.ToInt32();
                  //暂定独自去
                  RoleManager.Instance.playerPeople.Record("决定独自去" + actionSetting.name);
                  SocializationManager.Instance.AddPlan(actionSetting.name, actionSetting.id.ToInt32(), RoleManager.Instance.playerPeople);
                  //这里执行其它npc的邀约
                  SocializationManager.Instance.HandleInvite(RoleManager.Instance.allPeopleList);
                  GameModuleManager.Instance.InitGameModule(GameModuleType.SingleOutsideScene);
              },null);
        });
    }


    //public void SetSize(Vector2 vec)
    //{
    //    Vector2 bgVec = new Vector2(vec.x - rectXOffsetContent_BG, vec.y - rectYOffsetContent_BG);
    //    rect_bg.sizeDelta = bgVec;
    //    rect.sizeDelta = vec;
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 点击人物头像弹出的交互选项
/// </summary>
public class ChoosePeopleInteractPanel : SelfAdaptionChoosePanel
{

    public override void Init(params object[] args)
    {
        base.Init(args);

        People people = args[1] as People;
        // string[] actionIdArr = bigMapSetting.actions.Split('|');

        //for (int i = 0; i < actionIdArr.Length; i++)
        //{
        //    int theId = actionIdArr[i].ToInt32();
        //    selfAdaptionChooseBtnViewList.Add(PanelManager.Instance.OpenSingle<ChooseActionBtnView>(grid, theId));

        //}
        if (!RoleManager.Instance.CheckIfMyWetalkFriend(people, RoleManager.Instance.playerPeople))
        {
            selfAdaptionChooseBtnViewList.Add(PanelManager.Instance.OpenSingle<ChoosePeopleInteractionBtnView>(grid,this, people, PeopleInteractType.AddWeTalk));
        }

    }

    public override void Clear()
    {
        base.Clear();
        //PanelManager.Instance.CloseAllSingle(grid);
        //selfAdaptionChooseBtnViewList.Clear();
    }

}

/// <summary>
/// 和人交互类型
/// </summary>
public enum PeopleInteractType
{
    None=0,
    AddWeTalk=1,//要求加微信
}

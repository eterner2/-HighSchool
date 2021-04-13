using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 准备界面
/// </summary>
public class ActionReadyPanelPeopleView : PeopleView
{
    public GameObject obj_toggle;
    public bool choosed;
    public ActionReadyPanel parentPanel;
    public override void OnOpenIng()
    {
        base.OnOpenIng();
        addBtnListener(btn, () =>
        {
            parentPanel.OnChoosedFriend(this);
        });
        choosed = false;
        Choose(choosed);
    }

    public void Choose(bool choose)
    {
        choosed = choose;
        obj_toggle.gameObject.SetActive(choosed);
    }
    //public override void Clear()
    //{
    //    base.Clear();

    //}
}

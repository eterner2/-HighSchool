using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutsidePanel : TestPanel
{
    public Image img; 
    public string actionName;//行为

    public Transform trans_grid;//格子
    public Button btn_backClassRoom;

    public void Init(params object[] args)
    {
        Clear();
        string outsideName = (string)args[0];
        SingleAction action= NewBehaviourScript.Instance.actionScriptable.FindActionByOutSideName(outsideName);
        img.sprite = action.sprt;
        GenerateActionListPeople(action.name);
        btn_backClassRoom.onClick.RemoveAllListeners();
        btn_backClassRoom.onClick.AddListener(() =>
        {
            NewBehaviourScript.Instance.BackClassRoom();
        });
    }

    /// <summary>
    /// 生成正在进行某个行为的人
    /// </summary>
    /// <param name="actionName"></param>
    public void GenerateActionListPeople(string actionName)
    {
        for(int i = 0; i < NewBehaviourScript.Instance.planList.Count; i++)
        {
            Plan plan = NewBehaviourScript.Instance.planList[i];
            if (plan.actionName == actionName)
            {
                SingleGroup singleGroup = NewBehaviourScript.Instance.GenerateEntity(ObjectPoolSingle.SingleGroup) as SingleGroup;
                singleGroup.transform.SetParent(trans_grid, false);
                singleGroup.Init(plan);
            }
        }



    }

    public void Clear()
    {
      NewBehaviourScript.Instance.ClearAllChildEntity(trans_grid);
    }
}

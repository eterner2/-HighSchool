using Framework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutsidePanel : PanelBase
{
    public Image img; 
    public string actionName;//行为

    public Transform trans_grid;//格子
    public Button btn_backClassRoom;

    public List<Plan> curActionPlanList;//当前行为的计划
    public BigMapSetting bigMapSetting;//大地图

    public override void Init(params object[] args)
    {
        Clear();
        curActionPlanList = args[0] as List<Plan>;
       
        int actionId = curActionPlanList[0].actionId;

        ActionSetting actionSetting = DataTable.FindActionSetting(actionId);
        BigMapSetting bigMapSetting = DataTable.FindBigMapSetting(actionSetting.bigMapId.ToInt32());

        //string outsideName = (string)args[0];
        //SingleAction action= NewBehaviourScript.Instance.actionScriptable.FindActionByOutSideName(outsideName);
        img.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.bigMapFolderPath + bigMapSetting.iconName);//  action.sprt;
        GenerateActionListPeople(curActionPlanList);
        //btn_backClassRoom.onClick.RemoveAllListeners();
        //btn_backClassRoom.onClick.AddListener(() =>
        //{
        //    NewBehaviourScript.Instance.BackClassRoom();
        //});

        addBtnListener(btn_backClassRoom, ()=>GameModuleManager.Instance.ChangeGameModule(GameModuleType.BigMap));
    }

    /// <summary>
    /// 生成正在进行某个行为的人
    /// </summary>
    /// <param name="actionName"></param>
    public void GenerateActionListPeople(List<Plan> curPlanList)
    {
        for(int i = 0; i < curPlanList.Count; i++)
        {
            Plan plan = curPlanList[i];


            SingleGroupView singleGroup = PanelManager.Instance.OpenSingle<SingleGroupView>(trans_grid, plan);

            
        }



    }

    public override void Clear()
    {
        base.Clear();
        PanelManager.Instance.CloseAllSingle(trans_grid);
    }
    //{
    //  NewBehaviourScript.Instance.ClearAllChildEntity(trans_grid);
    //}
}

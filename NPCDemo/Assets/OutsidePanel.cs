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

    public Button btn_continue;//继续按钮
    public Image img_bar;//bar


    public override void Init(params object[] args)
    {
        Clear();
        curActionPlanList = args[0] as List<Plan>;
       
        int actionId = curActionPlanList[0].actionId;

        ActionSetting actionSetting = DataTable.FindActionSetting(actionId);
        BigMapSetting bigMapSetting = DataTable.FindBigMapSetting(actionSetting.bigMapId.ToInt32());

        EventCenter.Register(TheEventType.ActionProcess, OnActionProcess);
        EventCenter.Register(TheEventType.ActionPause, ActionPause);

        img.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.actionSceneFolderPath + bigMapSetting.sceneImgName);//  action.sprt;
        GenerateActionListPeople(curActionPlanList);
        //btn_backClas sRoom.onClick.RemoveAllListeners();
        //btn_backClassRoom.onClick.AddListener(() =>
        //{
        //    NewBehaviourScript.Instance.BackClassRoom();
        //});

        addBtnListener(btn_backClassRoom, ()=>GameModuleManager.Instance.ChangeGameModule(GameModuleType.BigMap));
        addBtnListener(btn_continue, () =>
        {
            GameActionManager.Instance.ContinueAction();
        });
        trans_grid.gameObject.SetActive(false);
        btn_continue.gameObject.SetActive(false);
        //如果是图书馆，则直接弹考试界面

        if ((ActionType)RoleManager.Instance._CurGameInfo.PlayerPeople.ChoosedActionId
            == ActionType.GoLibraryStudy)
        {
            PanelManager.Instance.OpenPanel<ExamPreparePanel>(PanelManager.Instance.trans_layer2);
                
        }
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

    /// <summary>
    /// 继续
    /// </summary>
    void ActionPause(object[] param)
    {
        bool pause = (bool)param[0];
        if (pause)
        {
            trans_grid.gameObject.SetActive(true);
            btn_continue.gameObject.SetActive(true);
        }
        else
        {
            trans_grid.gameObject.SetActive(false);
            btn_continue.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 行为继续
    /// </summary>
    void OnActionProcess(object[] param)
    {
        float process = (float)param[0];
        img_bar.fillAmount = process / (float)100;

    }
    public override void Clear()
    {
        base.Clear();
        PanelManager.Instance.CloseAllSingle(trans_grid);
        img_bar.fillAmount = 0;
        EventCenter.Remove(TheEventType.ActionProcess, OnActionProcess);
        EventCenter.Remove(TheEventType.ActionPause, ActionPause);

    }


    //{
    //  NewBehaviourScript.Instance.ClearAllChildEntity(trans_grid);
    //}
}

using Framework.Data;
using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyPanel : PanelBase
{
    public Transform trans_grid;
    public Transform trans_examProGrid;//战斗技能格子

    public List<SinglePropertyView> singlePropertyViewList = new List<SinglePropertyView>();//非战斗技能
    public List<SinglePropertyView> examPropertyViewList = new List<SinglePropertyView>();//战斗技能

    public Image img_upgradeBar;//升级进度
    public Text txt_upgradeBar;//升级进度
    public Text txt_lv;//等级
    public Button btn_close;

    public override void Init(params object[] args)
    {
        base.Init(args);
        EventCenter.Register(TheEventType.GetStudyScore, GetStudyScore);
        addBtnListener(btn_close, () =>
         {
             PanelManager.Instance.ClosePanel(this);
         });
    }


    public override void OnOpenIng()
    {
        base.OnOpenIng();

        List<SinglePropertyData> singlePropertyDataList = RoleManager.Instance.FindCommonPropertyDataList();

        for(int i = 0; i < singlePropertyDataList.Count; i++)
        {
            singlePropertyViewList.Add(PanelManager.Instance.OpenSingle<SinglePropertyView>(trans_grid, singlePropertyDataList[i]));

        }
        List<SinglePropertyData> examPropertyDataList = RoleManager.Instance.FindExamPropertyDataList();
        for (int i = 0; i < examPropertyDataList.Count; i++)
        {
            examPropertyViewList.Add(PanelManager.Instance.OpenSingle<SinglePropertyView>(trans_examProGrid, examPropertyDataList[i]));

        }


        RefreshShow();
    }

    public override void Clear()
    {
        base.Clear();
        PanelManager.Instance.CloseAllSingle(trans_grid);
        PanelManager.Instance.CloseAllSingle(trans_examProGrid);

        singlePropertyViewList.Clear();
        examPropertyViewList.Clear();
    }
    /// <summary>
    /// 获取学习属性
    /// </summary>
    public void GetStudyScore(object param)
    {
        //GameObject.Find("SinglePropertyView/txt_num").GetComponent<Text>().SetText(CurScore.ToString());
        RefreshShow();
    }

    public void RefreshShow()
    {
        for(int i=0;i< singlePropertyViewList.Count; i++)
        {
            singlePropertyViewList[i].RefreshShow();
        }
        int expLimit = 0;
        LevelInfo levelInfo = RoleManager.Instance.GetPeopleLevelInfo(0);
        if (levelInfo.beforeLevel < DataTable._peopleUpgradeList.Count)
        {
            PeopleUpgradeSetting setting = DataTable._peopleUpgradeList[levelInfo.beforeLevel];
            expLimit = DataTable._peopleUpgradeList[levelInfo.beforeLevel].needExp.ToInt32();
            txt_upgradeBar.SetText(levelInfo.beforeExp + "/" + expLimit);
            img_upgradeBar.fillAmount = levelInfo.beforeExp / (float)expLimit;
        }
        else
        {
            img_upgradeBar.fillAmount = 1;
            txt_upgradeBar.SetText("已满级");
        }
        txt_lv.SetText(("LV.") + levelInfo.beforeLevel);


    }
}

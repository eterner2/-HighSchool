using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyPanel : PanelBase
{
    public Transform trans_grid;
    public List<SinglePropertyView> singlePropertyViewList = new List<SinglePropertyView>();

    public override void Init(params object[] args)
    {
        base.Init(args);
        EventCenter.Register(TheEventType.GetStudyScore, GetStudyScore);
    }


    public override void OnOpenIng()
    {
        base.OnOpenIng();

        singlePropertyViewList.Add(PanelManager.Instance.OpenSingle<SinglePropertyView>(trans_grid, PropertyIdType.Study));
    }

    public override void Clear()
    {
        base.Clear();
        PanelManager.Instance.CloseAllSingle(trans_grid);
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
    }
}

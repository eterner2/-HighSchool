using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskPanel : PanelBase
{
    public Transform trans_animParent;
    public override void Init(params object[] args)
    {
        base.Init(args);
        EventCenter.Register(TheEventType.GetStudyScore, OnScoreAdd);

    }
    public override void OnOpenIng()
    {
        base.OnOpenIng();
    }

    public override void Clear()
    {
        base.Clear();
        EventCenter.Remove(TheEventType.GetStudyScore, OnScoreAdd);
    }

    /// <summary>
    /// 分数增加
    /// </summary>
    public void OnScoreAdd(object[] param)
    {
        PanelManager.Instance.OpenSingle<FlyTxtView>(trans_animParent, (string)param[0]);
        
    }
  
    private void Update()
    {
        //processTest = _CurTimeData.DayProcess;

    }

}

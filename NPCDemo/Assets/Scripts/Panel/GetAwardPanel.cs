using RoleData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAwardPanel : PanelBase
{
    public Transform trans_grid;

    public List<AwardData> awardDataList = new List<AwardData>();
    public Action closeCallBack;
    public override void Init(params object[] args)
    {
        base.Init(args);
        awardDataList = args[0] as List<AwardData>;
        closeCallBack = args[1] as Action;
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        for(int i=0;i< awardDataList.Count; i++)
        {
            PanelManager.Instance.OpenSingle<AwardView>(trans_grid, awardDataList[i]);
        }
    }

    public override void Clear()
    {
        base.Clear();
        PanelManager.Instance.CloseAllSingle(trans_grid);
    }
    public override void OnClose()
    {
        base.OnClose();
        closeCallBack?.Invoke();
    }
}

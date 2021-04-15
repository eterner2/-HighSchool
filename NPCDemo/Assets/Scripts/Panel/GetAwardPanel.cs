using RoleData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAwardPanel : PanelBase
{
    public Transform trans_grid;

    public List<SinglePropertyData> proDataList = new List<SinglePropertyData>();
    public Action closeCallBack;
    public override void Init(params object[] args)
    {
        base.Init(args);
        proDataList = args[0] as List<SinglePropertyData>;
        closeCallBack = args[1] as Action;
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        for(int i=0;i< proDataList.Count; i++)
        {
            PanelManager.Instance.OpenSingle<SinglePropertyView>(trans_grid, proDataList[i]);
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

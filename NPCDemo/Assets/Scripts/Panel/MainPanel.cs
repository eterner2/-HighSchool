using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : PanelBase
{
    public Button btn_cellPhone;
    public GameObject obj_redPointInCellPhoneBtn;//手机按钮的红点

    public override void Init(params object[] args)
    {
        base.Init(args);
        EventCenter.Register(TheEventType.ShowMainPanelRedPoint, RedPointShow);

        addBtnListener(btn_cellPhone, () =>
        {
            PanelManager.Instance.OpenPanel<CellPhonePanel>(PanelManager.Instance.trans_layer2,CellphoneHandleType.Common);
        });
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        RedPointShow();
    }

    public void RedPointShow()
    {
        RedPointManager.Instance.SetRedPointUI(obj_redPointInCellPhoneBtn, RedPointType.CellPhone, 0);

    }

    public override void Clear()
    {
        base.Clear();
        EventCenter.Remove(TheEventType.ShowMainPanelRedPoint, RedPointShow);

    }
}

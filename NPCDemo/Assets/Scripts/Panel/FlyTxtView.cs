using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyTxtView : SingleViewBase
{
    //public Transform trans_txtInitPos;
    public Text txt;
    public override void Init(params object[] args)
    {
        base.Init(args);
        txt.SetText((string)args[0]);
    }
    public override void OnOpenIng()
    {
        base.OnOpenIng();
        transform.DOKill();
        transform.localPosition = Vector2.zero;
        transform.DOLocalMoveY(100, 1f).OnComplete(() =>
        {
            PanelManager.Instance.CloseSingle(this);
        });
    }
}

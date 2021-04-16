using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//掉血
public class LoseHPEffect : FinishKillEffect
{
    public Transform trans_start;
    public Transform trans_end;
    public Text txt;
    public override void Init(params object[] args)
    {
        base.Init(args);
        Vector3 pos = (Vector3)args[0];// as Vector3;
        int num = (int)args[1];
        txt.SetText(num.ToString());
        transform.position = pos;
        txt.transform.DOKill();
        txt.transform.position = trans_start.position;

        txt.transform.DOMove(trans_end.position, 1f);
    }
}

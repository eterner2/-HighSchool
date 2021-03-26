using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackMaskPanel : PanelBase
{
    public float changeTimer;
    public float changeTime = 1;
    public Action finishCB;
    public Image img;
    public BlackMaskType blackMaskType;
    //Newtonsoft.Json.Serialization.Action
    public override void Init(params object[] args)
    {
        base.Init(args);
        if (args.Length > 0)
        {
            blackMaskType = (BlackMaskType)args[0];
            //finishCB = args[1];
        }
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        changeTimer = 0;
        img.gameObject.SetActive(true);
        img.DOKill();

        float startA=0;
        float endA=0;
        //关闭
        if (blackMaskType == BlackMaskType.Close)
        {
            startA = 0;
            endA = 1;
        }
        else
        {
            startA = 1;
            endA = 0;
        }


        img.color = new Color(0, 0, 0, startA);

        img.DOFade(endA, changeTime).OnComplete(() =>
        {
            if (finishCB != null)
                finishCB();
            if (blackMaskType == BlackMaskType.Close)
                GameTimeManager.Instance.StartNewDay();
            else
            {
                PanelManager.Instance.ClosePanel(this);
            }
        });
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// 关闭的时候此处发一条消息 
    /// </summary>
    public override void OnClose()
    {
        base.OnClose();

    }
}
/// <summary>
/// 黑幕类型
/// </summary>
public enum BlackMaskType
{
    None=0,
    Open=1,
    Close=2,
}

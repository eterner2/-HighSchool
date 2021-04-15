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
    public Action pingPongCloseFinishCB;//黑幕完全打开后才执行的
    public Image img;
    public BlackMaskType blackMaskType;
    bool startMove = false;
    bool callBackCalled;//调用了回调
    float moveTimer;
    float moveTime1;
    float moveTime2;
    //Newtonsoft.Json.Serialization.Action
    public override void Init(params object[] args)
    {
        base.Init(args);
        if (args.Length > 0)
        {
            blackMaskType = (BlackMaskType)args[0];
            finishCB = args[1] as Action;
            pingPongCloseFinishCB = args[2] as Action;
        }
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        changeTimer = 0;
        img.gameObject.SetActive(true);
        img.DOKill();

        moveTimer = 0;
        startMove = true;
        callBackCalled = false;
        if (blackMaskType != BlackMaskType.PingPong)
        {
            moveTime1 = changeTime;
            float startA = 0;
            float endA = 0;
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
                //if (finishCB != null)
                //    finishCB();
            });


        }
        //变黑-action-变亮
        else
        {
            moveTime1 = changeTime / 2;
            moveTime2 = changeTime;

            img.color = new Color(0, 0, 0, 0);

            img.DOFade(1, changeTime/2).OnComplete(() =>
            {
                //if (finishCB != null)
                //    finishCB();

                img.DOFade(0, changeTime / 2).OnComplete(() =>
                {
                    //PanelManager.Instance.ClosePanel(this);
                });
            });
        }
      
    }

    //不要写在DOTWEEN里面 而是update控制 否则报错会被dotween捕捉成警告 影响调试（坑！）
    private void Update()
    {
        if (startMove)
        {
            moveTimer += Time.deltaTime;
            if (blackMaskType != BlackMaskType.PingPong)
            {
                if (moveTimer >= moveTime1)
                {
                    if (finishCB != null)
                        finishCB();
                    startMove = false;
                }

            }
            //变黑-action-变亮
            else
            {
                if (moveTimer >= moveTime1
                    &&moveTimer<moveTime2 && !callBackCalled)
                {
                    if (finishCB != null)
                    {
                        finishCB();
                        callBackCalled = true;
                    }
                }
                else if (moveTimer >= moveTime2)
                {
                    PanelManager.Instance.ClosePanel(this);
                    pingPongCloseFinishCB?.Invoke();
                }
   
            }
        }
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
    PingPong=3,//如果是pingpong则变黑-action-透明-移除
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBase : MonoBehaviour,Entity
{
    public ObjectPoolSingle objType { get; set; }
    public bool isTmpObj { get; set; }
    public GameObject obj { get; set; }

    public Button btn_emptyClose;//空白关闭
    public Dictionary<Button, UnityEngine.Events.UnityAction> btnListenerDic = new Dictionary<Button, UnityEngine.Events.UnityAction>();

    public Transform trans_content;//内容，会缩放的

    public virtual void Init(params object[] args)
    {
        Clear();
    }

    /// <summary>
    /// 给组件赋值
    /// </summary>
    public virtual void OnOpenIng()
    {
        if (btn_emptyClose != null)
        {
            addBtnListener(btn_emptyClose, 
                ()=>PanelManager.Instance.ClosePanel(this));
        }
    }

    //public virtual void Closed()
    //{
    //    //EntityManager.Instance.CloseEntity(this);

    //    //OnClose();
    //    //HideObj();
    //}

    /// <summary>
    /// 增加按钮点击事件
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="listener"></param>
    public virtual void addBtnListener(Button btn, UnityEngine.Events.UnityAction listener, bool changeScale = true)
    {
        if (btn == null)
        {
            Debug.LogError("按钮没赋值");

            return;
        }
        if (!btnListenerDic.ContainsKey(btn))
        {
            btn.onClick.AddListener(listener);
            btnListenerDic.Add(btn, listener);

            //if (changeScale)
            //    btn.onClick.AddListener(() => UIUtil.BtnScale(btn.transform));
            //if (AuditionManage.S != null)
            //    btn.onClick.AddListener(() => AuditionManage.S.Play(0));
        }
    }
    /// <summary>
    /// 移除按钮点击事件
    /// </summary>
    public void RemoveBtnClick()
    {
        if (btnListenerDic != null && btnListenerDic.Count > 0)
        {
            foreach (KeyValuePair<Button, UnityEngine.Events.UnityAction> kv in btnListenerDic)
            {
                Button btn = kv.Key;
                UnityEngine.Events.UnityAction action = kv.Value;
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners();
                }
                //    btn.onClick.RemoveListener(action);

                //btn.onClick.AddListener(() => BtnScale(btn.transform));

            }
            btnListenerDic.Clear();

        }
    }

    public virtual void Clear()
    {

    }

    public virtual void OnClose()
    {


    }
}

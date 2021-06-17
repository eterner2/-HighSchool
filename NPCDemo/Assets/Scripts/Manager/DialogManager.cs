using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoInstance<DialogManager>
{
   
    /// <summary>
    /// 创建普通对话
    /// </summary>
    /// <param name="dialogDataList"></param>
    /// <param name="endCallBack"></param>
    public void CreateDialog(List<DialogData> dialogDataList,Action endCallBack)
    {
        PanelManager.Instance.OpenPanel<DialogPanel>(PanelManager.Instance.trans_layer2,DialogType.Common, dialogDataList,endCallBack);
    }

    /// <summary>
    /// 创建选项对话
    /// </summary>
    /// <param name="dialogDataList"></param>
    /// <param name="endCallBack"></param>
    public void CreateDialog(List<DialogData> dialogDataList,string btn1Str, Action btn1Callback,string btn2Str,Action btn2Callback)
    {
        PanelManager.Instance.OpenPanel<DialogPanel>(PanelManager.Instance.trans_layer2, DialogType.Common, dialogDataList, btn1Str,btn1Callback,btn2Str,btn2Callback);
    }
}

public class DialogData
{
   public People belong;
    public string content;
    public DialogData(People p, string str)
    {
        belong = p;
        content = str;
    }
}

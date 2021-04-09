using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoInstance<DialogManager>
{
   

    public void CreateDialog(List<DialogData> dialogDataList,Action endCallBack)
    {
        PanelManager.Instance.OpenPanel<DialogPanel>(PanelManager.Instance.trans_layer2, dialogDataList,endCallBack);
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

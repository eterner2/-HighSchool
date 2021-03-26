using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_singleRecord : TestPanel
{
    public override void Init(params object[] args)
    {
        string txt = (string)args[0];
        GetComponent<UnityEngine.UI.Text>().text = txt;
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, GetComponent<UnityEngine.UI.Text>().preferredHeight);
        //int lineCount=
    }
}

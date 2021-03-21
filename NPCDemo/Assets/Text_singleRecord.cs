using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_singleRecord : Entity
{
    public void Init(string txt)
    {
        GetComponent<UnityEngine.UI.Text>().text = txt;
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, GetComponent<UnityEngine.UI.Text>().preferredHeight);
        //int lineCount=
    }
}

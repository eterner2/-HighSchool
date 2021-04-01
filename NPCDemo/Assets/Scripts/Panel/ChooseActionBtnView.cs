using Framework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 自适应选择按钮
/// </summary>
public class ChooseActionBtnView : SelfAdaptionChooseBtnView
{
    //public Text txt;
    //public RectTransform rect;
    //public RectTransform rect_bg;

    //public float rectXOffsetContent_BG=10;//x补偿 bg距离content大小
    //public float rectYOffsetContent_BG =5;//y补偿 bg距离content大小

    //public float rectXOffsetBg_TXT = 10;//bg x补偿 txt距离bg的大小
    //public float rectYOffsetBg_TXT = 5;//bg y补偿 txt距离bg的大小

    //public ActionSetting actionSetting;
    public override void Init(params object[] args)
    {
        base.Init(args);
        int theId = (int)args[0];
        actionSetting = DataTable.FindActionSetting(theId);
        txt.SetText(actionSetting.name);
    }

    public override void OnOpenIng()
    {
        //int theId = (int)args[0];
        //actionSetting = DataTable.FindActionSetting(theId);
        //txt.SetText(actionSetting.name);
        base.OnOpenIng();
        //txt.preferredHeight;
        //rect_bg.sizeDelta = new Vector2(txt.preferredWidth, rect_bg.sizeDelta.y);
        //rect.sizeDelta = rect_bg.sizeDelta;
        //SetSize(new Vector2(txt.preferredWidth+ rectXOffsetBg_TXT + rectXOffsetContent_BG, rect_bg.sizeDelta.y+ rectYOffsetBg_TXT + rectYOffsetContent_BG));
    }


    //public void SetSize(Vector2 vec)
    //{
    //    Vector2 bgVec = new Vector2(vec.x - rectXOffsetContent_BG, vec.y - rectYOffsetContent_BG);
    //    rect_bg.sizeDelta = bgVec;
    //    rect.sizeDelta = vec;
    //}
}

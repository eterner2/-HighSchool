using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonHintPanel : PanelBase
{

    public HintData hintData;
   // public Transform trans_blockRange;//这个范围内点击不会关闭一般用bg
    public Button btn_ok;//右边的按钮 ok
    public Button btn_cancel;//左边的按钮 取消
    public Text txt_content;//内容

    public override void Init(object[] args)
    {
        base.Init(args);
        this.hintData = args[0] as HintData;
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        // SetBlockParent(trans_blockRange);
        RegisterBtnClick();
        ShowDetail();
    }

    /// <summary>
    /// 按钮点击事件
    /// </summary>
    void RegisterBtnClick()
    {
        addBtnListener(btn_ok, OnOkClick);
        addBtnListener(btn_cancel, OnCancelClick);
        addBtnListener(btn_emptyClose, ()=> 
        {
            PanelManager.Instance.ClosePanel(this);
        });
    }

    void ShowDetail()
    {
        this.txt_content.text = hintData.content;

        string okBtnTxt = "确定";
        string cancelBtnTxt = "取消";

        if (hintData.str_okBtn != "")
            okBtnTxt = hintData.str_okBtn;
        if (hintData.str_cancelBtn != "")
            cancelBtnTxt = hintData.str_cancelBtn;

        this.btn_ok.GetComponentInChildren<Text>().SetText(okBtnTxt);
        this.btn_cancel.GetComponentInChildren<Text>().SetText(cancelBtnTxt);

    }

    /// <summary>
    /// 点击ok
    /// </summary>
    void OnOkClick()
    {
        if (hintData.okCallBack != null)
            hintData.okCallBack.Invoke();
        PanelManager.Instance.ClosePanel(this);
    }

    /// <summary>
    /// 点击取消
    /// </summary>
    void OnCancelClick()
    {
        if (hintData.cancelCallBack != null)
            hintData.cancelCallBack.Invoke();
        PanelManager.Instance.ClosePanel(this);

    }


}

/// <summary>
/// 弹窗数据
/// </summary>
public class HintData
{
    public Action okCallBack;       //点击ok
    public Action cancelCallBack;   //点击取消
    public string content;//内容
    public string str_cancelBtn;//取消按钮的内容
    public string str_okBtn;//ok按钮的内容

}

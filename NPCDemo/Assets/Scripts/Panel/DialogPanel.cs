using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : PanelBase
{
    public Button btn_next;
    public Transform trans_imgLeftPeople;
    public Transform trans_imgRightPeople;
    public Transform trans_txtLeftPeople;
    public Transform trans_txtRightPeople;
    public Image img_verticalDraw;
    public Text txt;

   // People people;
    bool left;

    List<DialogData> dialogList = new List<DialogData>();
    Action endCallBack;
    int curIndex = -1;
    People curSpeekPeople;

    public DialogType dialogType;

    public Transform trans_choose;//对话完后弹出选项
    public Button btn1;//选项1
    public Text txt_btn1;//选项1txt
    public Button btn2;//选项2
    public Text txt_btn2;//选项2txt

    public Action btn1ChooseCallback;//选择了选项1
    public Action btn2ChooseCallback;//选择了选项2


    public override void Init(params object[] args)
    {
        base.Init(args);

        dialogType = (DialogType)args[0];
        dialogList = args[1] as List<DialogData>;

        if (dialogType == DialogType.Common)
        {
            endCallBack = args[2] as Action;
            trans_choose.gameObject.SetActive(false);
        }
        else
        {
            string btn1Str = (string)args[2];
            btn1ChooseCallback = args[3] as Action;

            string btn2Str = (string)args[4];
            btn2ChooseCallback = args[5] as Action;

            txt_btn1.SetText(btn1Str);
            txt_btn2.SetText(btn2Str);

            trans_choose.gameObject.SetActive(true);

            addBtnListener(btn1, () =>
            {
                if (btn1ChooseCallback != null)
                    btn1ChooseCallback();
                PanelManager.Instance.ClosePanel(this);

            });

            addBtnListener(btn2, () =>
            {
                if (btn2ChooseCallback != null)
                    btn2ChooseCallback();
                PanelManager.Instance.ClosePanel(this);

            });

        }



        addBtnListener(btn_next, () =>
        {
            if (curIndex < dialogList.Count-1)
            {
                curIndex++;
                if (dialogList[curIndex].belong != curSpeekPeople)
                {
                    left = !left;
                }
                NextDialog();
            }
            //结束对话
            else
            {
                if (dialogType == DialogType.Common)
                {
                    if (endCallBack != null)
                        endCallBack();
                    PanelManager.Instance.ClosePanel(this);
                }
                else
                {
                    trans_choose.gameObject.SetActive(true);
                }

            }
        });
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        left = true;
        curIndex = 0;

        NextDialog();

    }


    void NextDialog()
    {
        curSpeekPeople= dialogList[curIndex].belong;

        if (curSpeekPeople != null)
        {
            img_verticalDraw.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.verticalDrawFolderPath + curSpeekPeople.verticalDrawName);
            img_verticalDraw.gameObject.SetActive(true);

        }
        else
        {
            img_verticalDraw.gameObject.SetActive(false);
            txt.transform.position = trans_txtRightPeople.position;

        }
        if (left)
        {
            img_verticalDraw.transform.position = trans_imgLeftPeople.transform.position;
            txt.transform.position = trans_txtLeftPeople.position;
        }
        else
        {
            img_verticalDraw.transform.position = trans_imgRightPeople.transform.position;
            txt.transform.position = trans_txtRightPeople.position;
        }
        txt.SetText(dialogList[curIndex].content);
    }

}

/// <summary>
/// 对话类型
/// </summary>
public enum DialogType
{
    None=0,
    Common,
    Choose,//有选项的对话
}

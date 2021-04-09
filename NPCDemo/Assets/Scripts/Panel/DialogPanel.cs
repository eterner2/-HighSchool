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
    public override void Init(params object[] args)
    {
        base.Init(args);


        dialogList = args[0] as List<DialogData>;
        endCallBack = args[1] as Action;
        

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
                if (endCallBack != null)
                    endCallBack();
                PanelManager.Instance.ClosePanel(this);
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

        img_verticalDraw.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.verticalDrawFolderPath + curSpeekPeople.verticalDrawName);

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

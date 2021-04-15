using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseActionPanel : SelfAdaptionChoosePanel
{


    public override void Init(params object[] args)
    {
        base.Init(args);
        //Vector3 theVec = (Vector3)args[0];
        //this.contentPos = new Vector3(theVec.x, theVec.y,transform.position.z);
        BigMapSetting bigMapSetting = args[1] as BigMapSetting;
        //string str1 = (string)args[1];
        //string str2 = (string)args[2];
        //string str3 = (string)args[3];

        string[] actionIdArr = bigMapSetting.actions.Split('|');

        //如果答应了和谁一起做事

        for(int i = 0; i < actionIdArr.Length; i++)
        {
            int theId = actionIdArr[i].ToInt32();
       
            selfAdaptionChooseBtnViewList.Add(PanelManager.Instance.OpenSingle<ChooseActionBtnView>(grid, this, theId));

        }


    }



    public override void OnOpenIng()
    {
        base.OnOpenIng();
    }


   

}

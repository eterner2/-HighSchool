using Framework.Data;
using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePropertyView : SingleViewBase
{
    //public PropertyIdType propertyId;
    //PropertySetting setting;
    SinglePropertyData singlePropertyData;
    public Text txt_proName;
    public Text txt_proNum;

    public override void Init(params object[] args)
    {
        base.Init(args);
        singlePropertyData = args[0] as SinglePropertyData;
        //propertyId = (PropertyIdType)args[0];
        //setting = DataTable.FindPropertySetting((int)propertyId);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        RefreshShow();
    }

    public void RefreshShow()
    {
        txt_proName.SetText(DataTable.FindPropertySetting(singlePropertyData.PropertyId).name);
        txt_proNum.SetText(singlePropertyData.PropertyNum.ToString());
    }


}

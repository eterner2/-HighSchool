using Framework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePropertyView : SingleViewBase
{
    public PropertyIdType propertyId;
    PropertySetting setting;

    public Text txt_proName;
    public Text txt_proNum;

    public override void Init(params object[] args)
    {
        base.Init(args);
        propertyId = (PropertyIdType)args[0];
        setting = DataTable.FindPropertySetting((int)propertyId);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        RefreshShow();
    }

    public void RefreshShow()
    {
        txt_proName.SetText(setting.name);
        txt_proNum.SetText(RoleManager.Instance.FindPropertyNum(propertyId).ToString());
    }


}

using Framework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleMapView : SingleViewBase
{
    public BigMapIdType BigMapIdType;
    public Image img_icon;
    public Text txt_name;

    public BigMapSetting bigMapSetting;
    public override void Init(params object[] args)
    {
        base.Init(args);
        bigMapSetting = DataTable.FindBigMapSetting((int)BigMapIdType);

    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        img_icon.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.bigMapFolderPath+ bigMapSetting.iconName);
        txt_name.SetText(bigMapSetting.name);
    }


}

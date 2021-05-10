using Framework.Data;
using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPropertyView : SingleViewBase
{

    PropertySetting propertySetting;
    SinglePropertyData singlePropertyData;

    public Image img_icon;
    public Image img_bar;
    public Text txt_num;

    public override void Init(params object[] args)
    {
        base.Init(args);
        //propertySetting = args[0] as PropertySetting;
        singlePropertyData = args[0] as SinglePropertyData;
        propertySetting = DataTable.FindPropertySetting(singlePropertyData.PropertyId);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();

        if (singlePropertyData.PropertyLimit > 0)
        {
            txt_num.SetText(singlePropertyData.PropertyNum + "/" + singlePropertyData.PropertyLimit);
            img_bar.fillAmount = singlePropertyData.PropertyNum/(float)singlePropertyData.PropertyLimit;
        }
        else
        {
            txt_num.SetText(singlePropertyData.PropertyNum.ToString());
            img_bar.fillAmount = 0;


        }
        img_icon.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.propertyIconFolderPath + propertySetting.iconName);
    }


}

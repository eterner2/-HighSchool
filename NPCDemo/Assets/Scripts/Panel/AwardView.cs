using Framework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwardView : SingleViewBase
{
    public Image img_icon;
    public Text txt;
    public AwardData awardData;

    public override void Init(params object[] args)
    {
        base.Init(args);
        awardData = args[0] as AwardData;

    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        img_icon.sprite = awardData.sprt;
        txt.SetText(awardData.num.ToString());
    }

}

public enum AwardType
{
    None=0,
    Item=1,
    Property=2,
}

public class AwardData
{
    public AwardType awardType;
    public int id;
    public int num;
    public Sprite sprt;

    public AwardData(AwardType type,int id,int num)
    {
        this.awardType = type;
        this.id = id;
        this.num = num;
        switch (type)
        {
            case AwardType.Property:
                PropertySetting setting = DataTable.FindPropertySetting(id);
                sprt = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.propertyIconFolderPath + setting.iconName);
                break;

        }
    }
}

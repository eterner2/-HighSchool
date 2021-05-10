using Framework.Data;
using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanel : PanelBase
{
    public Transform trans_grid;

    public override void Init(params object[] args)
    {
        base.Init(args);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();

        PropertyData propertyData = RoleManager.Instance._CurGameInfo.PlayerPeople.PropertyData;
        int count = propertyData.PropertyIdList.Count;
        for (int i=0;i< count; i++)
        {
            int id = propertyData.PropertyIdList[i];
            SinglePropertyData singleData = propertyData.PropertyDataList[i];
            PropertySetting setting = DataTable.FindPropertySetting(id);
            if (setting.showInStatusPanel == "1")
            {
                PanelManager.Instance.OpenSingle<StatusPropertyView>(trans_grid, singleData);
            }
        }

    }

    public override void Clear()
    {
        base.Clear();
        PanelManager.Instance.CloseAllSingle(trans_grid);
    }
}

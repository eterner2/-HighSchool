using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModulePeopleView : PeopleView
{
    public override void OnOpenIng()
    {
        base.OnOpenIng();
        addBtnListener(btn, () =>
        {
            if (people.protoData.OnlyId != RoleManager.Instance.playerPeople.protoData.OnlyId)
            {
                PanelManager.Instance.OpenPanel<ChoosePeopleInteractPanel>(PanelManager.Instance.trans_layer2, transform.position, people);
            }
        });
    }
}

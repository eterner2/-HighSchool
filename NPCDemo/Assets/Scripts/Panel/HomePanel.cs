using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel : PanelBase
{
    public Button btn_Action;//行动 去大地图

    public override void Init(params object[] args)
    {
        base.Init(args);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();

        addBtnListener(btn_Action, () =>
        {
            GameModuleManager.Instance.ChangeGameModule(GameModuleType.BigMap);
        });
    }
}

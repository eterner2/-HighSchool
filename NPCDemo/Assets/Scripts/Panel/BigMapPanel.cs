using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigMapPanel : PanelBase
{
    public List<SingleMapView> singleMapViewList = new List<SingleMapView>();
    public Button btn_back;//返回
    public override void Init(params object[] args)
    {
        base.Init(args);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();

        int count = singleMapViewList.Count;
        for(int i = 0; i < singleMapViewList.Count; i++)
        {
            singleMapViewList[i].Init(null);
            singleMapViewList[i].OnOpenIng();
        }

        addBtnListener(btn_back, () =>
        {
            GameModuleManager.Instance.ChangeGameModule(GameModuleType.Home);
        });
    }

}


/// <summary>
/// 大地图id
/// </summary>
public enum BigMapIdType
{
    None=0,
    Mall=10001,
    Library=10002,
    NetBar=10003,
    ArtStreet=10004,
    Gym=10005,
}
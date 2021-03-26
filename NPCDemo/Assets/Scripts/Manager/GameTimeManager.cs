using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏时间管理 所有时间在此管理
/// </summary>
public class GameTimeManager 
{
    private static GameTimeManager inst = null;

    public static GameTimeManager Instance
    {
        get
        {
            if (inst == null)
            {
                inst = new GameTimeManager();
            }
            return inst;
        }

    }

    /// <summary>
    /// 结束今天
    /// </summary>
    public void EndDay()
    {
        Action newDay = StartNewDay;

        PanelManager.Instance.OpenPanel<BlackMaskPanel>(PanelManager.Instance.trans_commonPanelParent, BlackMaskType.Close, newDay);
    }
    /// <summary>
    /// 开始新的一天
    /// </summary>
    public void StartNewDay()
    {
        PanelManager.Instance.ClosePanel(PanelManager.Instance.GetPanel<BlackMaskPanel>());

        Action finishMask = delegate ()
        {
            PanelManager.Instance.ClosePanel(PanelManager.Instance.GetPanel<BlackMaskPanel>());
        };
        GameObject.Find("WorkDayPanel").GetComponent<WorkDayPanel>().OnNewDayStart();
        PanelManager.Instance.OpenPanel<BlackMaskPanel>(PanelManager.Instance.trans_commonPanelParent, BlackMaskType.Open, finishMask);

    }
}

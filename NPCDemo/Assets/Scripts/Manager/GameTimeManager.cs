using Newtonsoft.Json.Serialization;
using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏时间管理 所有时间在此管理
/// </summary>
public class GameTimeManager:MonoInstance<GameTimeManager>
{
    //private static GameTimeManager inst = null;

    //public static GameTimeManager Instance
    //{
    //    get
    //    {
    //        if (inst == null)
    //        {
    //            inst = new GameTimeManager();
    //        }
    //        return inst;
    //    }

    //}

    public TimeData _CurTimeData;
    public int processTest;
    public override void Init()
    {
        _CurTimeData = RoleManager.Instance._CurGameInfo.TimeData;

        int singlePhase = 100 / 9;
        int left = 0;
        int right = 0;
        for (int i = 0; i < 8; i++)
        {
            right += singlePhase;
            dayPhaseRangeArr[i] = new Vector2Int(left, right);
            left += singlePhase;
        }
        dayPhaseRangeArr[8] = new Vector2Int(left, 100);
        lastDayPhaseIndex = GetPhaseIndex((int)_CurTimeData.DayProcess);

        //时间开始走暂时放在这里
        if(RoleManager.Instance._CurGameInfo.CurGameModule==(int)GameModuleType.WeekDay)
            startMove = true;
    }

    //public int curYear;//第几年
    //public int curMoon;//第几月

    public float dayProcessSpeed=5;//走一天的时间（单位秒 后续配表）
    public bool startMove = false;//开始走
    public Vector2Int[] dayPhaseRangeArr=new Vector2Int[9];//一天的阶段（暂时平均分成9段）
    public int lastDayPhaseIndex =-1;//上个阶段（这个值改变代表上了一节课）

    /// <summary>
    /// 结束今天
    /// </summary>
    public void EndDay()
    {
        //Action endDayMask = delegate 
        //{
        //    PanelManager.Instance.ClosePanel(PanelManager.Instance.GetPanel<BlackMaskPanel>());

        //};

        //PanelManager.Instance.OpenPanel<BlackMaskPanel>(PanelManager.Instance.trans_commonPanelParent, BlackMaskType.Close);
        PanelManager.Instance.BlackMask(BlackMaskType.Close,()=> 
        {
            PanelManager.Instance.ClosePanel(PanelManager.Instance.GetPanel<BlackMaskPanel>());
            DayPlus();
        });

    }
    /// <summary>
    /// 开始新的一天
    /// </summary>
    public void StartNewDay()
    {

        Action finishMask = delegate ()
        {
            PanelManager.Instance.ClosePanel(PanelManager.Instance.GetPanel<BlackMaskPanel>());
            lastDayPhaseIndex = 0;
            startMove = true;
            EventCenter.Broadcast(TheEventType.OnNewDayStart);

        };
        //GameObject.Find("WorkDayPanel").GetComponent<WorkDayPanel>().OnNewDayStart();
        PanelManager.Instance.OpenPanel<BlackMaskPanel>(PanelManager.Instance.trans_commonPanelParent, BlackMaskType.Open, finishMask);

    }

    private void Update()
    {
   
        if (startMove)
        {
        
            //return;
            DayProcess(Time.deltaTime);
        }
    }

    /// <summary>
    /// 日期过程
    /// </summary>
    void DayProcess(float deltaTime)
    {

        float theProcessAdd = (100 / dayProcessSpeed) * deltaTime;
        _CurTimeData.DayProcess += (theProcessAdd);
        EventCenter.Broadcast(TheEventType.DayTimeProcess, _CurTimeData.DayProcess);

        //一天过去了
        if (_CurTimeData.DayProcess >= 100)
        {
            _CurTimeData.DayProcess = 100;
            startMove = false;
            EndDay();
            //DayPlus();
        }
        else
        {
            int curDayPhaseIndex = GetPhaseIndex((int)_CurTimeData.DayProcess);
            if (curDayPhaseIndex > 0)
            {
                if (curDayPhaseIndex > lastDayPhaseIndex)
                {
                    lastDayPhaseIndex = curDayPhaseIndex;
                    //得到分数
                    RoleManager.Instance.GetStudyScore();

                }
            }
        }



    }

    /// <summary>
    /// 一天过去
    /// </summary>
    public void DayPlus()
    {
        _CurTimeData.DayProcess = 0;
        lastDayPhaseIndex = 0;

        _CurTimeData.Day++;
        _CurTimeData.TheWeekDay++;
        _CurTimeData.DayBeforeExam--;
        if (_CurTimeData.Day >= 31)
        {
            MoonPlus();
        }
        if (_CurTimeData.TheWeekDay >= 8)
        {
            WeekPlus();
        }
        EventCenter.Broadcast(TheEventType.OnNewDayStart);

        PanelManager.Instance.BlackMask(BlackMaskType.Open, () =>
        {
            PanelManager.Instance.ClosePanel(PanelManager.Instance.GetPanel<BlackMaskPanel>());
            startMove = true;
        });
        //Action finishMask = delegate ()
        //{
        //    PanelManager.Instance.ClosePanel(PanelManager.Instance.GetPanel<BlackMaskPanel>());
        //    EventCenter.Broadcast(TheEventType.OnNewDayStart);
        //    startMove = true;

        //};
        //GameObject.Find("WorkDayPanel").GetComponent<WorkDayPanel>().OnNewDayStart();
        //PanelManager.Instance.OpenPanel<BlackMaskPanel>(PanelManager.Instance.trans_commonPanelParent, BlackMaskType.Open, finishMask);

    }

    /// <summary>
    /// 一周过去
    /// </summary>
    public void WeekPlus()
    {
        _CurTimeData.TheWeekDay = 1;
        //_CurTimeData.wee
    }
    /// <summary>
    /// 一个月过去
    /// </summary>
    public void MoonPlus()
    {
        _CurTimeData.Day = 1;
        _CurTimeData.Month++;
        if(_CurTimeData.Month >= 13)
        {
            YearPlus();
        }
    }
    /// <summary>
    /// 一年过去
    /// </summary>
    public void YearPlus()
    {
        _CurTimeData.Year++;
        _CurTimeData.Month = 1;
    }

    /// <summary>
    /// 当前是哪个阶段
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public int GetPhaseIndex(int process)
    {
        for(int i = 0; i < dayPhaseRangeArr.Length-1; i++)
        {
            if (process >= dayPhaseRangeArr[i].x && process < dayPhaseRangeArr[i].y)
            {
                return i;
            }
        }
        return -1;
    }
}

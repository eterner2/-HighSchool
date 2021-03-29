using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkDayPanel : PanelBase
{
    public Image img_processBar;
    public Text txt_dayBeforeExam;
    public Text txt_weekDay;

    public float totalTime = 5;//5秒走完
    //public float moveSpeed = 1;
   // bool startMove = false;

    //public float singleCourseTime;//单节课时间
    //public float singleCourseTimer;//单节课获得知识计时器
    //public int initDayBeforeExam = 300;//后续从gametime取
    //public int curWeekDay = 1;            //后续从gametime取
    //public float init
    // Start is called before the first frame update


    public override void Init(params object[] args)
    {
        base.Init(args);

        EventCenter.Register(TheEventType.DayTimeProcess,OnDayTimeProcess);
        EventCenter.Register(TheEventType.OnNewDayStart, OnNewDayStart);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
    }


    void Start()
    {
        //startMove = true;
        RefreshShow();
        //singleCourseTime = totalTime / 9;
    }

    /// <summary>
    /// 一天的时间变化
    /// </summary>
    void OnDayTimeProcess(object[] obj)
    {
        //int process = (int)obj[0];
        img_processBar.fillAmount = GameTimeManager.Instance._CurTimeData.DayProcess / (float)100;


    }

    // Update is called once per frame
    void Update()
    {
        //if (startMove)
        //{
        //    img_processBar.fillAmount += Time.deltaTime / (float)5;

        //    if (img_processBar.fillAmount < 1)
        //    {
        //        singleCourseTimer += Time.deltaTime;
        //        if (singleCourseTimer >= singleCourseTime)
        //        {
        //            singleCourseTimer = 0;
        //            //得到分数
        //            RoleManager.Instance.GetStudyScore();
        //        }

        //    }
        //    else
        //    {
        //        //新的一天
        //        startMove = false;
        //        GameTimeManager.Instance.EndDay();
        //    }

        //}   
    }

    public void RefreshShow()
    {
        txt_dayBeforeExam.SetText("距离高考还有" + GameTimeManager.Instance._CurTimeData.DayBeforeExam + "天");
        txt_weekDay.SetText("星期" + UIUtil.ChangeArabicToChinese(GameTimeManager.Instance._CurTimeData.TheWeekDay));
    }

    public void FinishDay()
    {

    }

    public void OnNewDayStart()
    {
        //startMove = true;
        //initDayBeforeExam--;
        //curWeekDay++;
        //singleCourseTimer = 0;
        img_processBar.fillAmount = 0;
        RefreshShow();
    }

    public override void Clear()
    {
        EventCenter.Remove(TheEventType.DayTimeProcess,  OnDayTimeProcess);
        EventCenter.Remove(TheEventType.OnNewDayStart, OnNewDayStart);

        base.Clear();
    }
}

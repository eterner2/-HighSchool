using Framework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : CommonInstance<AchievementManager>
{

    public override void Init()
    {
        base.Init();
        InitExamAchievement();

    }
    /// <summary>
    /// 初始化考试成就
    /// </summary>
    void InitExamAchievement()
    {
        for (int i = 0; i < DataTable._examList.Count; i++)
        {
            ExamSetting setting = DataTable._examList[i];
            if (setting.initLevel == "1")
            {
                RoleManager.Instance.playerPeople.protoData.Achievement.UnlockedExamIdList.Add(setting.id.ToInt32());
            }
        }
    }
}

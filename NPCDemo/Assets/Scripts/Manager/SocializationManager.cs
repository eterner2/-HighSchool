using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocializationManager : CommonInstance<SocializationManager>
{
    
    public void InviteCalc()
    {
        //定计划
        for (int i = 0; i < allPeopleList.Count; i++)
        {
            People me = allPeopleList[i];
            List<People> tmpList = new List<People>();

            //tmpList.Clear();

            int actionIndex = RandomManager.Next(0, actionScriptable.singleActionList.Count);
            me.actionName = actionScriptable.singleActionList[actionIndex].name;

            //邀请0-5人(不能重复邀请
            int num = RandomManager.Next(0, 6);
            if (num > 0)
            {
                for (int j = 0; j < num; j++)
                {
                    People choosed = null;
                    while (choosed == null
                        || choosed == me
                        || tmpList.Contains(choosed))
                    {
                        int index = RandomManager.Next(0, allPeopleList.Count);
                        choosed = allPeopleList[index];
                    }
                    tmpList.Add(choosed);
                    me.Invite(choosed);
                    me.Record("邀请" + choosed.name + me.actionName);
                    choosed.Record("被" + me.name + "邀请" + me.actionName);

                }
            }
            else
            {
                //planList.Add(new Plan(me.actionName, me, null));
                me.Record("想今天一个人" + me.actionName);
            }
        }

    }

}

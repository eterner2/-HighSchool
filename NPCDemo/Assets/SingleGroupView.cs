using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGroupView : SingleViewBase
{
    public Transform trans_grid;
    //public PeopleView p1;
    public GameObject obj_line;
    //public PeopleView p2;
   
    public override void Init(params object[] args)
    {
        Plan plan = args[0] as Plan;
        //PeopleView peopleView = GenerateEntity(ObjectPoolSingle.PeopleView) as PeopleView;
        //peopleView.transform.SetParent(trans_peopleGrid, false);
        for(int i = 0; i < plan.peopleList.Count; i++)
        {
            PeopleView peopleView = PanelManager.Instance.OpenSingle<ActionModulePeopleView>(trans_grid, plan.peopleList[i]);
        }
        if (plan.peopleList.Count == 2)
        {
            //p1.Init(plan.peopleList[0]);
           // p2.gameObject.SetActive(true);
           // p2.Init(plan.peopleList[1]);
            obj_line.gameObject.SetActive(true);
        }
        else
        {
           // p1.Init(plan.peopleList[0]);
           // p2.gameObject.SetActive(false);
            obj_line.gameObject.SetActive(false);

        }
    }

    public override void Clear()
    {
        base.Clear();
        PanelManager.Instance.CloseAllSingle(trans_grid);
    }
}

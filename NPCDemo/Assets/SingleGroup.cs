using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGroup : Entity
{
    public PeopleView p1;
    public GameObject obj_line;
    public PeopleView p2;
   
    public void Init(Plan plan)
    {
        //PeopleView peopleView = GenerateEntity(ObjectPoolSingle.PeopleView) as PeopleView;
        //peopleView.transform.SetParent(trans_peopleGrid, false);
        if (plan.peopleList.Count == 2)
        {
            p1.Init(plan.peopleList[0]);
            p2.gameObject.SetActive(true);
            p2.Init(plan.peopleList[1]);
            obj_line.gameObject.SetActive(true);
        }
        else
        {
            p1.Init(plan.peopleList[0]);
            p2.gameObject.SetActive(false);
            obj_line.gameObject.SetActive(false);

        }
    }
}

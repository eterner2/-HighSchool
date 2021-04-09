using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePeopleInteractionBtnView : SelfAdaptionChooseBtnView
{
    public Button btn;
    public People people;
    public PeopleInteractType interactType;
    public override void Init(params object[] args)
    {
        base.Init(args);

        people = args[0] as People;
        interactType = (PeopleInteractType)args[1];

        switch (interactType)
        {
            case PeopleInteractType.AddWeTalk:
                txt.SetText("加微信");
                addBtnListener(btn, () =>
                {
                    PeopleInteractManager.Instance.AskForWetalkNum(RoleManager.Instance.playerPeople, people);
                });
                break;
        }
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
    }
}

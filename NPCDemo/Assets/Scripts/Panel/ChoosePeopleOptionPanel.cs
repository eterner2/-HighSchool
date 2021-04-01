using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 点击人的选项
/// </summary>
public class ChoosePeopleOptionPanel : SelfAdaptionChoosePanel
{
    public override void Init(params object[] args)
    {
        base.Init(args);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
    }
}

public enum ChoosePeopleOptionType
{
    Chat=1,//闲聊 对A
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanel :MonoBehaviour, Entity
{
    public ObjectPoolSingle objType { get; set; }
    public bool isTmpObj { get; set; }
    public GameObject obj { get; set; }

    public virtual void Init(params object[] args)
    {
    }

    public void OnClose()
    {
    }
}

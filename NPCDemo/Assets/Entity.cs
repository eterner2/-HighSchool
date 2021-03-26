using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Entity //: MonoBehaviour
{
     ObjectPoolSingle objType { get; set; }
     bool isTmpObj { get; set; }
     GameObject obj { get; set; }

    void Init(params object[] args);


    void OnClose();
}

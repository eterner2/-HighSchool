using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSAnimSingle : MonoBehaviour
{
    public Transform trans_startPos;
    public Transform trans_endPos;
    public Transform trans_obj;
    public AnimationCurve animationCurve;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            trans_obj.position = trans_startPos.position;
            trans_obj.DOMove(trans_endPos.position, 1f).SetEase(animationCurve);

        }
    }
}

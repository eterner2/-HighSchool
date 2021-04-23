using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishKillEffect :MonoBehaviour, Entity
{
    public ObjectPoolSingle objType { get; set; }
    public bool isTmpObj { get; set; }
    public GameObject obj { get; set; }

    public Animator anim;
    public float totalTime;
    public float timer = 0;
    public string animName;
    public virtual void Init(params object[] args)
    {
        timer = 0;
        if(anim!=null)
        anim.Play(animName, 0);

    }

    public void OnClose()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= totalTime)
        {
            EntityManager.Instance.CloseEntity(this);
        }
    }
}

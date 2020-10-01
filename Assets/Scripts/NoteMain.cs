using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMain : MonoBehaviour
{
    //決め打ちされる数値たち
    [NonSerialized] public float goal_Y = -7.4f;
    [NonSerialized] public float size_X = 11;
    [NonSerialized] public float size_Y = 17;

    [SerializeField] bool EnableAnimationCurve;
    [SerializeField] public AnimationCurve Curve;
    [SerializeField] public float LandingTime;//着弾時刻

    private GameObject child;
    
    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //curveにしたがって動く、1.0で着弾
        float curveTime = (TimeManager.time - LandingTime) + 1;
        float curveValue = Curve.Evaluate(curveTime);
        child.transform.localScale = new Vector2(size_X, size_Y) * (1 - curveValue);
        child.transform.localPosition = new Vector2(0, goal_Y - curveValue * goal_Y);

    }
}

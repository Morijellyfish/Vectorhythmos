using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] public float time_serialize;
    static public float time;
    // Start is called before the first frame update
    void Start()
    {
        time = time_serialize;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }
}

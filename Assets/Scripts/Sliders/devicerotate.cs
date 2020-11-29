using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class devicerotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        var g = Input.gyro.attitude;
        transform.localRotation = new Quaternion(-g.x,-g.y,g.z,g.w);


    }
}

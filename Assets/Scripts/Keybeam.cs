using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Keybeam : MonoBehaviour
{
    public bool hold=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var m = GetComponent<MeshRenderer>().material;
        m.EnableKeyword("_EMISSION");
        m.SetColor("_EmissionColor", hold ? new Color(0.7f, 0.7f, 0.7f):new Color(1,1,1));
        hold = false;
    }
}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNoteEvent : MonoBehaviour
{
    public bool holding = false;
    HoldNoteMesh mesh_script;
    // Start is called before the first frame update
    void Start()
    {
        mesh_script = GetComponent<HoldNoteMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mesh_script.points[0].LandingTime <= TimeManager.time)
        {
            if (holding)
            {
                holding = false;
                Debug.Log("holding:true");
            }
            else
            {
                Debug.Log("holding:false");
            }
        }
    }
    public void Hold()
    {
        holding = true;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapNoteEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Tap()
    {
        float t_diff = GetComponent<TapNoteMesh>().LandingTime - TimeManager.time;
        if (Mathf.Abs(t_diff)<=0.080f)//80ms
        {
            Debug.Log("Tapped:" + this.gameObject.name);
            Destroy(this.gameObject);
        }

    }
}

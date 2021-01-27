using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNoteEvent : MonoBehaviour
{
    private List<int> fingers = new List<int>();
    public bool holding = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fingers = fingers.Intersect(TapMainRay.fingers).ToList();
        if (fingers.Count != 0)
        {
            holding = true;
        }
        else
        {
            holding = false;
        }
    }
    public void Tap(int finger)
    {
        fingers.Add(finger);
    }
    
}

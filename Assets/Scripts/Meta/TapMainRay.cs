using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapMainRay : MonoBehaviour
{
    static public List<int> fingers = new List<int>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount != 0)
        {
            foreach (var touch in Input.touches)
            {
                Vector3 fingerpos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -10.0f));
                fingerpos.z = 0;
                Vector3 raystart = (Vector3.Normalize(fingerpos - new Vector3(0, 5, 0)) * 9) + new Vector3(0, 5, 0);
                var hit = Physics2D.RaycastAll(new Vector2(raystart.x, raystart.y), new Vector2(0, 5) - new Vector2(raystart.x, raystart.y),100f);//note

                DrawRay(hit,raystart);//debug
                
                if (touch.phase == TouchPhase.Began)
                {
                    fingers.Add(touch.fingerId);
                    if (hit.Length != 0)
                    {
                        TapnoteTap(LayerSort(hit, 8));//Tapnotes
                        HoldnoteTap(LayerSort(hit, 9), touch.fingerId);//Holdnotes
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    fingers.Remove(touch.fingerId);
                }
                
                var keys = LayerSort(hit, 31);//keybeam
                foreach(var obj in keys)
                {
                    obj.transform.parent.GetComponent<Keybeam>().hold = true;
                }

            }
        }
    }

    void DrawRay(RaycastHit2D[] hit,Vector3 raystart)
    {
        if (hit.Length != 0)
        {
            Debug.DrawRay(raystart, new Vector3(0, 5, 0) - raystart, Color.yellow, 0.01f);

        }
        else
        {
            Debug.DrawRay(raystart, new Vector3(0, 5, 0) - raystart, Color.blue, 0.01f);
        }

    }

    GameObject[] LayerSort(RaycastHit2D[] hit,int layer)
    {
        GameObject[] objs=new GameObject[hit.Length];
        int i=0;
        foreach(var obj in hit)
        {
            if (obj.transform.gameObject.layer == layer)
            {
                objs[i] = obj.transform.gameObject;
                i++;
            }
        }
        Array.Resize(ref objs, i);
        return objs;
    }
    
    void TapnoteTap(GameObject[] taps)
    {
        if (taps.Length != 0)
        {
            float[] times = new float[taps.Length];
            int i = 0;
            foreach (var obj in taps)
            {
                times[i] = obj.GetComponent<TapNoteMesh>().LandingTime;
                i++;
            }
            float min = times.Min();
            int minkey = Array.IndexOf(times, min);

            while (minkey != -1)
            {
                taps[minkey].GetComponent<TapNoteEvent>().Tap();
                times[minkey] = times[minkey] + 1;
                minkey = Array.IndexOf(times, min);

            }
        }
    }

    void HoldnoteTap(GameObject[] taps,int finger)
    {
        foreach(var obj in taps)
        {
            obj.GetComponent<HoldNoteEvent>().Tap(finger);
        }
    }
}

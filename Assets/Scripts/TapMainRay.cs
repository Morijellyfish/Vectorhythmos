﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapMainRay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TouchSupported:" + Input.touchSupported);

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
                var hit = Physics2D.RaycastAll(new Vector2(raystart.x, raystart.y), new Vector2(0, 5) - new Vector2(raystart.x, raystart.y));

                //(Debug)DrawRay
                if (hit.Length != 0)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        Debug.DrawRay(raystart, new Vector3(0, 5, 0) - raystart, Color.red, 0.01f);
                    }
                    else
                    {
                        Debug.DrawRay(raystart, new Vector3(0, 5, 0) - raystart, Color.yellow, 0.01f);
                    }
                }
                else
                {
                    Debug.DrawRay(raystart, new Vector3(0, 5, 0) - raystart, Color.blue, 0.01f);
                }

                
                if (hit.Length != 0)
                {
                    //tap
                    if (touch.phase == TouchPhase.Began) {
                        float time;
                        int min;
                        time = hit[0].transform.parent.gameObject.GetComponent<NoteMain>().LandingTime;
                        min = 0;
                        for (int i=0; i < hit.Length; i++)
                        {
                            var tmp = hit[i].transform.parent.gameObject.GetComponent<NoteMain>().LandingTime;
                            if (time > tmp)
                            {
                                time = tmp;
                                min = i;
                            }
                        }
                        hit[min].transform.parent.GetComponent<NoteMain>().Tap();

                    }
                }

            }
        }

    }
}

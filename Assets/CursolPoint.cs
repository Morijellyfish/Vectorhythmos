using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursolPoint : MonoBehaviour
{
    Vector3 fingerpos;
    Vector3 fingerdeff;
    Vector3 fingerspeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount != 0)
        {
            var touch = Input.touches[0];
            if (touch.phase== TouchPhase.Began)
            {
                fingerpos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -10.0f));
                fingerpos.z = 0;
            }
            else
            {
                fingerdeff=Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -10.0f))-fingerpos;
                fingerpos += fingerdeff;
                fingerdeff.z = 0;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                fingerspeed = fingerdeff;
            }

        }
        else
        {
            transform.position += fingerspeed;
            fingerspeed *= 0.8f;
        }
        transform.position += fingerdeff;
        fingerdeff = Vector3.zero;


    }
}

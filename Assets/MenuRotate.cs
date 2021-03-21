using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRotate : MonoBehaviour
{
    float r;
    float r_start;
    float r_origin;
    float r_diff;
    float r_speed;
    // Start is called before the first frame update
    void Start()
    {
        r_origin = 45;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount !=0)
        {
            var touch = Input.touches[0];
            
            if (touch.phase == TouchPhase.Began)
            {
                var fingerpos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -10.0f));
                Vector2 Ve = new Vector2(fingerpos.x, fingerpos.y - 5).normalized;
                r_start = (Mathf.Atan2(Ve.x, Ve.y) * (180 / Mathf.PI));
                r_start = (r_start + 360) % 360;
                r = (Mathf.Atan2(Ve.x, Ve.y) * (180 / Mathf.PI));
            }
            else
            {
                var fingerpos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -10.0f));
                Vector2 Ve = new Vector2(fingerpos.x, fingerpos.y - 5).normalized;
                r_diff = ((Mathf.Atan2(Ve.x, Ve.y) * (180 / Mathf.PI))+360)%360-r;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                r_speed = r_diff;
                r_origin += r - r_start;
                r_start = 0;
                r_diff = 0;
                r = 0;
            }
        }
        else
        {
            r_origin += r_speed*2;
            r_speed *= 0.95f;
            if (-0.5<r_speed&&r_speed < 0.5)
            {
                Debug.Log(r_origin % 45);
                float tmp = r_origin % 45;
                if ((0<tmp && tmp< 2) || (0>tmp && tmp>=-2))
                {
                    Debug.Log("clit+");
                    r_origin -= tmp;
                }else if(0>tmp&&tmp<=-43)
                {
                    Debug.Log("clit-");
                    r_origin -= 45 + tmp;
                }else if (tmp >= 43)
                {
                    Debug.Log("clit44");
                    r_origin += 45 - tmp;
                }
                else
                {
                    if (45/2<tmp || (tmp<0 && -45/2<tmp))
                    {
                        Debug.Log("bi");
                        r_origin += 1;
                    }
                    if ((tmp>0 && 45/2>tmp) || (tmp<0 && -45/2>tmp))
                    {
                        Debug.Log("mi");
                        r_origin -= 1;
                    }
                }
            }
        }
        //Debug.Log("rotation:"+(r - r_start).ToString());
        r += r_diff;
        r_diff = 0;
        r = (r + 360)%360;
        transform.rotation = Quaternion.Euler(0, 0, -((r-r_start)+r_origin));


    }
}

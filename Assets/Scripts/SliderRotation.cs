using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderRotation : MonoBehaviour
{
    public static float r=200;
    GameObject[] hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawRay(transform.GetChild(0).transform.position);
        Vector2 raystart = new Vector2(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y);
        hit = LayerSort(Physics2D.RaycastAll(new Vector2(raystart.x, raystart.y), new Vector2(0, 5) - new Vector2(raystart.x, raystart.y), 100f),15);
        

    }
    private void FixedUpdate()
    {
        float diff = r - (transform.eulerAngles.z+200);
        char c='N';
        if (diff <= -5)
        {
            c = 'R';
        }
        if (5 <= diff)
        {
            c = 'L';
        }
        if (c != 'N')
        {
            foreach (GameObject obj in hit)
            {
                obj.GetComponent<SliderNoteEvent>().Slide(c);
            }
        }
        r = transform.eulerAngles.z+200;

    }

    void DrawRay(Vector3 raystart)
    {
        Debug.DrawRay(raystart, new Vector3(0, 5, 0) - raystart, Color.blue, 0.01f);

    }
    GameObject[] LayerSort(RaycastHit2D[] hit, int layer)
    {
        GameObject[] objs = new GameObject[hit.Length];
        int i = 0;
        foreach (var obj in hit)
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
}

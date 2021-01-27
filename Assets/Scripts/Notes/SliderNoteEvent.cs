using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderNoteEvent : MonoBehaviour
{
    public void Slide(char c)
    {
        if (GetComponent<SliderNoteMesh>().direction == c)
        {
            Debug.Log(c);
        }
    }
}

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
        Debug.Log(this.gameObject.name);
        Destroy(this.gameObject);

    }
}

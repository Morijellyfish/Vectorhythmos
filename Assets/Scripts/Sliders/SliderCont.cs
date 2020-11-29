using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderCont : MonoBehaviour
{
    [SerializeField] GameObject phone;
    [SerializeField] GameObject horizon_phone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var z=((phone.transform.rotation.eulerAngles - horizon_phone.transform.rotation.eulerAngles).z);
        transform.parent.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -z));
    }
}

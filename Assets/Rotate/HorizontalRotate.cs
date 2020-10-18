using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRotate : MonoBehaviour
{

    [SerializeField] float a;
    [SerializeField] GameObject sphere;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var q = (sphere.transform.position);
        transform.rotation = Quaternion.LookRotation(q, Vector3.up) * Quaternion.Euler(new Vector3(0, 0, a));
    }
}

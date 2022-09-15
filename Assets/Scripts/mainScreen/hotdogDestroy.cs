using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hotdogDestroy : MonoBehaviour
{
    private Transform location;
    public GameObject hotdog;

    void Start()
    {
        location = hotdog.GetComponent<Transform>();
    }

    void Update()
    {
        Debug.Log(location.position);
        if(location.position.y < -5 )
        {
            Debug.Log("Destroy attempted");
            Destroy(GetComponentInParent<Transform>().transform.gameObject);
        }
    }
}

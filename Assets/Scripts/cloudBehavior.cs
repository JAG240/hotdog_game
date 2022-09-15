using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudBehavior : MonoBehaviour
{
    private float speed = 0.02f;
    private Vector3 size = new Vector3(6f, 6f, 0f);
    private float destructionLimit;
    Transform cloudLoc;

    void Start()
    {
        cloudLoc = GetComponent<Transform>();
    }

    void Update()
    {
        cloudLoc.Translate(speed * -1 * Time.deltaTime, 0, 0);
        if(cloudLoc.position.x < destructionLimit)
        {
            Destroy(this.gameObject);
        }
    }

    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void setNewSize(Vector3 newSize)
    {
        size = newSize;
        GetComponent<Transform>().localScale = size;
    }

    public void setDestructionLimit(float x)
    {
        destructionLimit = x;
    }
}

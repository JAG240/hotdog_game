using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Unity.Mathematics;
using UnityEngine;

public class hotdogPhysics : MonoBehaviour
{
    public GameObject lineDrawManager;
    private Transform hotdogPos;
    private List<Vector2> linePoints;
    private bool startCheck = false;
    [SerializeField] private float force = 1f;
    [SerializeField] private float pointOffset = 0.5f;

    void Start()
    {
        hotdogPos = GetComponent<Transform>();
    }

    
    void Update()
    {
        if(startCheck)
        {
            StartCoroutine(lineCheck());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered");
        if(collision.gameObject.tag == "Line")
        {
            GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
    }

    public void physicsStart()
    {
        linePoints = lineDrawManager.GetComponent<physicsDrawLine>().touchPos;
        startCheck = true;
        GetComponentInChildren<Animator>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exited");
        if(collision.gameObject.tag == "Line")
        {
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }

    private float getRotation(Vector2 newPoint, Vector2 oldPoint)
    {
        float slope = (newPoint.y - oldPoint.y) / (newPoint.x - oldPoint.x);
        float angle = Mathf.Atan(slope);
        return angle;
    }

    private IEnumerator lineCheck()
    {
        startCheck = false;
        for (int i = 0; linePoints.Count > i; i++)
        {
            if (hotdogPos.position.x > linePoints[i].x - pointOffset && hotdogPos.position.x < linePoints[i].x + pointOffset && hotdogPos.position.y < linePoints[i].y + pointOffset && hotdogPos.position.y > linePoints[i].y - pointOffset)
            {
                if (i != linePoints.Count - 1)
                {
                    float d = Vector2.Distance(linePoints[i], linePoints[i + 1]);
                    float angle = getRotation(linePoints[i], linePoints[i + 1]);
                    Vector2 addedForce = new Vector2(d * Mathf.Cos(angle) * force, d * Mathf.Sin(angle) * force);
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 14.24f * Time.deltaTime));
                    GetComponent<Rigidbody2D>().AddForce(addedForce * Time.deltaTime);
                    Debug.Log("Added force: " + addedForce * Time.deltaTime);
                }
                else
                {
                    float d = Vector2.Distance(linePoints[i - 1], linePoints[i]);
                    float angle = getRotation(linePoints[i - 1], linePoints[i]);
                    Vector2 addedForce = new Vector2(d * Mathf.Cos(angle) * force, d * Mathf.Sin(angle) * force);
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 14.24f * Time.deltaTime));
                    GetComponent<Rigidbody2D>().AddForce(addedForce * Time.deltaTime);
                    Debug.Log("Added force: " + addedForce * Time.deltaTime);
                }
            }
        }

        yield return new WaitForFixedUpdate();
        startCheck = true;
    }
}

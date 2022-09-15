using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class hotdogBehaviorBackup : MonoBehaviour
{
    public int fingID;
    public GameObject lineDrawManager;
    [SerializeField] private float horForce = -50;
    [SerializeField] private float verForce = -200;

    void Start()
    {

    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Line")
        {
            StartCoroutine(linePhysics(lineDrawManager.GetComponent<drawLine>().touchPos));
        }
    }


    public void physicsStart()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    IEnumerator linePhysics(List<Vector2> points)
    {
        for(int i = 0; i < points.Count - 1; i++)
        {
            if (points[i].x > GetComponent<Transform>().position.x)
            {
                yield return new WaitForSeconds(0.05f);
                runPhysics(points[i], points[i + 1]);
            }
            else
            { 

            }
        }

        yield return null;
    }

    private void runPhysics(Vector2 curPoint, Vector2 nextPoint)
    {
        if(GetComponent<Rigidbody2D>().velocity.y == 100)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(nextPoint.x - curPoint.x * horForce, 0));
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(nextPoint.x - curPoint.x * horForce, nextPoint.y - curPoint.y * verForce));
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Line")
        {
            StopCoroutine(linePhysics(lineDrawManager.GetComponent<drawLine>().touchPos));
        }
    }
}

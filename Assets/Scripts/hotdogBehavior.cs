using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Unity.Mathematics;
using UnityEngine;

public class hotdogBehavior : MonoBehaviour
{
    public GameObject lineDrawManager;
    public GameObject grill;
    private Transform hotdogPos;
    private float lineSpeed = 0.1f;
    [SerializeField] private float lineAcc = .01f;
    [SerializeField] private float maxLineSpeed = 0.001f;
    [SerializeField] private float lineSpeedMultiplier = 0.08f;
    [SerializeField] private float exitVelocityMultiplierY = 300f;
    [SerializeField] private float exitVelocityMultiplierX = 600f;
    private Vector2 exitVelocity;
    private bool linePhysicsState = false;
    public GameObject lineMarker;
    private bool startMovement = true;
    private SpriteRenderer[] sprites;
    private Rigidbody2D[] bodies;

    void Start()
    {
        hotdogPos = GetComponent<Transform>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        bodies = GetComponentsInChildren<Rigidbody2D>();
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.enabled = false;
        }
        foreach(Rigidbody2D body in bodies)
        {
            body.isKinematic = true;
        }
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Line" && !linePhysicsState)
        {
            Debug.Log("Line Entered");
            foreach(Rigidbody2D body in bodies)
            {
                body.isKinematic = true;
            }
            StartCoroutine(linePhysics(lineDrawManager.GetComponent<drawLine>().touchPos));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void physicsStart()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        grill.GetComponent<grillBehavoir>().setOpen(true);
        StartCoroutine(grillOpen());
        foreach(Rigidbody2D body in bodies)
        {
            body.isKinematic = false;
        }
    }

    IEnumerator linePhysics(List<Vector2> points)
    {
        if (!linePhysicsState)
        {
            linePhysicsState = true;
            Debug.Log("Line Physics started");

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].x > hotdogPos.position.x)
                {

                    if (startMovement)
                    {
                        if(i < points.Count - 1)
                        {
                            lineSpeed = Vector2.Distance(points[i], points[i + 1]);
                        }
                        else
                        {
                            lineSpeed = Vector2.Distance(points[i - 1], points[i]);
                        }
                        StartCoroutine(moveToPosition(new Vector2(points[i].x, points[i].y), lineSpeed * lineSpeedMultiplier));
                    }

                    if (i > 0 && i < points.Count - 1)
                    {

                        setRotation(points[i], points[i - 1]);

                        float startAngle = getRotation(points[i], points[i - 1]);
                        float endAngle = getRotation(points[i + 1], points[i]);
                        float deltaAngle = Mathf.Abs(endAngle - startAngle);

                        if (deltaAngle > 40)
                        {
                            Debug.Log("Exting line due to angle");
                            setExitVelocity(points[i], points[i - 1]);
                            break;
                        }
                    }

                    yield return new WaitUntil(() => startMovement == true);

                    speedCheck();

                    if (points[i] == points[points.Count - 1])
                    {
                        setExitVelocity(points[i], points[i - 1]);
                    }
                }
            }

            exitLine();
            yield return new WaitForSeconds(0.5f);
            linePhysicsState = false;
        }
    }


    private void exitLine()
    {
        foreach (Rigidbody2D body in bodies)
        {
            body.isKinematic = false;
        }
        foreach (Rigidbody2D body in bodies)
        {
            body.AddForce(new Vector2(exitVelocity.x * exitVelocityMultiplierX, exitVelocity.y * exitVelocityMultiplierY));
        }
        Debug.Log("Exited Line: " + new Vector2(exitVelocity.x * exitVelocityMultiplierX, exitVelocity.y * exitVelocityMultiplierY));
    }

    private void setRotation(Vector2 newPoint, Vector2 oldPoint)
    {
        float angle = getRotation(newPoint, oldPoint);
        float curAngle = GetComponent<Transform>().rotation.z;
        transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(curAngle, angle - 95f, 0.5f));
    }    

    private float getRotation(Vector2 newPoint, Vector2 oldPoint)
    {
        float slope = (newPoint.y - oldPoint.y) / (newPoint.x - oldPoint.x);
        float angle = Mathf.Rad2Deg * Mathf.Atan(slope);
        return angle;
    }

    private void setExitVelocity(Vector2 newPoint, Vector2 oldPoint)
    {
        exitVelocity = new Vector2(newPoint.x - oldPoint.x, newPoint.y - oldPoint.y);
        float xRat = Mathf.Clamp(exitVelocity.x / Mathf.Abs(exitVelocity.y), -1f, 1f);
        float yRat = Mathf.Clamp(exitVelocity.y / exitVelocity.x, -1f, 1f);
        exitVelocity = new Vector2(xRat, yRat);
        Debug.Log("Exit Velocity Set: " + exitVelocity);
    }

    //This method is used to indicate where line breaks exists
    /*private void checkLine(List<Vector2> points)
    {
        for(int x  = 0; x < points.Count; x++)
        {
            if(x > 0 && x < points.Count - 1)
            {
                float startAngle = getRotation(points[x], points[x - 1]);
                float endAngle = getRotation(points[x + 1], points[x]);
                float deltaAngle = Mathf.Abs(endAngle - startAngle);
                if(deltaAngle > 60)
                {
                    Instantiate(lineMarker, new Vector3(points[x].x, points[x].y, 2f), quaternion.identity);
                }
            }
        }
    }*/

    IEnumerator moveToPosition(Vector2 point, float time)
    {
        startMovement = false;
        float elapsedTime = 0f;
        Vector3 startPos = hotdogPos.position;

        while (elapsedTime < time)
        {
            hotdogPos.position = Vector2.Lerp(startPos, point, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        startMovement = true;
    }

    private void speedCheck()
    {
        if (lineSpeed > maxLineSpeed && lineSpeed - lineAcc > maxLineSpeed)
        {
            lineSpeed -= lineAcc;
        }
        else if (lineSpeed > maxLineSpeed && lineSpeed - lineAcc < maxLineSpeed)
        {
            lineSpeed = maxLineSpeed;
        }
        else if (lineSpeed < maxLineSpeed)
        {
            lineSpeed = maxLineSpeed;
        }
    }

    private IEnumerator grillOpen()
    {
        yield return new WaitForSeconds(0.1f);
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.enabled = true;
        }
        yield return new WaitForSeconds(0.1f);
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 100f));
        foreach(Rigidbody2D body in bodies)
        {
            body.AddForce(new Vector2(0f, 400f));
        }
    }
   
    //This method is used to check if the hotdog is touching the line 
    /*private bool hotdogInLine()
    {
        return GetComponent<CapsuleCollider2D>().IsTouching(GameObject.FindGameObjectWithTag("Line").GetComponent<EdgeCollider2D>());
    }*/
}

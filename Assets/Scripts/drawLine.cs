using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class drawLine : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject curLine;
    public GameObject playButton;
    public GameObject hotSpot;

    [SerializeField] private Vector2 hotSpotOffset = new Vector2(0.5f, 0.5f);
    [SerializeField] private Vector2 lowerCamBoundry = new Vector2(13f, 6f);
    [SerializeField] private Vector2 upperCamBoundry = new Vector2(20f, 20f);
    public LineRenderer lineRend;
    public EdgeCollider2D edgeCol;
    public List<Vector2> touchPos;
    public Camera mainCam;

    public float maxLineSize = 25f;
    public float curLineSize = 0f;
    private bool hotSpotCheck = false;
    private Vector2 orginPos;
    
    void Start()
    {
        playButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        
    }

    void Update()
    {
        camCheck();
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchLoc = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 hotSpotLoc = hotSpot.GetComponent<Transform>().position;
            if(touchLoc.x > hotSpotLoc.x - hotSpotOffset.x && touchLoc.x < hotSpotLoc.x + hotSpotOffset.x && touchLoc.y > hotSpotLoc.y - hotSpotOffset.y && touchLoc.y < hotSpotLoc.y + hotSpotOffset.y)
            {
                hotSpotCheck = true;
            }
            else
            {
                hotSpotCheck = false;
                orginPos = touchLoc;
            }
        }
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && hotSpotCheck == true)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if(touchPos.Count == 0)
            {
                createLine(touch);
            }
            else if(curLineSize > 0 && maxLineSize > curLineSize)
            {
                updateLine(touchPosition);
            }
        }
        else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && hotSpotCheck == true)
        {
            Vector2 tempTouchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if(Vector2.Distance(tempTouchPos, touchPos[touchPos.Count - 1]) > .5f)
            {
                if(curLineSize < maxLineSize)
                {
                    updateLine(tempTouchPos);
                }
                else
                {
                    playButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
                }
            }
        }
        else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && hotSpotCheck == false)
        {
            moveCamera();
        }
        else if(Input.touchCount == 0 && curLineSize > 0)
        {
            lineRend.Simplify(0.15f);
        }
        else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && curLineSize > 0|| Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Canceled && curLineSize > 0)
        {
            updateHotspot();
            hotSpotCheck = false;
        }
    }

    void createLine(Touch newTouch)
    {
        curLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRend = curLine.GetComponent<LineRenderer>();
        edgeCol = curLine.GetComponent<EdgeCollider2D>();
        touchPos.Clear();
        touchPos.Add(Camera.main.ScreenToWorldPoint(newTouch.position));
        touchPos.Add(Camera.main.ScreenToWorldPoint(newTouch.position));
        lineRend.SetPosition(0, touchPos[0]);
        lineRend.SetPosition(1, touchPos[1]);
        edgeCol.points = touchPos.ToArray();
    }

    void updateLine(Vector2 newTouchPos)
    {
        touchPos.Add(newTouchPos);
        lineRend.positionCount++;
        lineRend.SetPosition(lineRend.positionCount - 1, newTouchPos);
        edgeCol.points = touchPos.ToArray();
        curLineSize += Vector2.Distance(touchPos[touchPos.Count - 2], touchPos[touchPos.Count - 1]);
    }

    void updateHotspot()
    {
        hotSpot.transform.position = new Vector3(touchPos[touchPos.Count - 1].x, touchPos[touchPos.Count - 1].y, 0);
    }

    void camCheck()
    {
        if(mainCam.transform.position.x < lowerCamBoundry.x)
        {
            Vector2 startPos = mainCam.transform.position;
            mainCam.transform.position = new Vector3(Mathf.Lerp(startPos.x, lowerCamBoundry.x, 1f), startPos.y, -10f);
        }

        if (mainCam.transform.position.y < lowerCamBoundry.y)
        {
            Vector2 startPos = mainCam.transform.position;
            mainCam.transform.position = new Vector3(startPos.x ,Mathf.Lerp(startPos.y, lowerCamBoundry.y, 1f), -10f);
        }

        if(mainCam.transform.position.x > upperCamBoundry.x)
        {
            Vector2 startPos = mainCam.transform.position;
            mainCam.transform.position = new Vector3(Mathf.Lerp(startPos.x, upperCamBoundry.x, 1f), startPos.y, -10f);
        }

        if(mainCam.transform.position.y > upperCamBoundry.y)
        {
            Vector2 startPos = mainCam.transform.position;
            mainCam.transform.position = new Vector3(startPos.x, Mathf.Lerp(startPos.y, upperCamBoundry.y, 1f), -10f);
        }
    }

    private void moveCamera()
    {
        Vector3 camPos = mainCam.transform.position;

        //lower limit camera check
        if (camPos.x <= lowerCamBoundry.x && camPos.y > lowerCamBoundry.y)
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (orginPos.x - currentPos.x > 0)
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else
            {
                mainCam.transform.Translate(new Vector2(0, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
        }
        else if (camPos.y <= lowerCamBoundry.y && camPos.x > lowerCamBoundry.x && camPos.x < upperCamBoundry.x)
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (orginPos.y - currentPos.y > 0)
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, 0f));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
        }

        else if (camPos.x <= lowerCamBoundry.x && camPos.y <= lowerCamBoundry.y)
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (orginPos.x - currentPos.x > 0 && orginPos.y - currentPos.y > 0)
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else if (orginPos.x - currentPos.x > 0 && orginPos.y - currentPos.y < 0)
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, 0f));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else if (orginPos.x - currentPos.x < 0 && orginPos.y - currentPos.y > 0)
            {
                mainCam.transform.Translate(new Vector2(0f, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
        }

        //upper limit camera check 
        else if(camPos.x >= upperCamBoundry.x && camPos.y < upperCamBoundry.y && camPos.y > lowerCamBoundry.y)
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (orginPos.x - currentPos.x < 0)
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else
            {
                mainCam.transform.Translate(new Vector2(0, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
        }
        else if (camPos.y >= upperCamBoundry.y && camPos.x < upperCamBoundry.x)
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (orginPos.y - currentPos.y < 0)
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, 0f));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
        }
        else if (camPos.x >= upperCamBoundry.x && camPos.y >= upperCamBoundry.y)
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (orginPos.x - currentPos.x < 0 && orginPos.y - currentPos.y < 0)
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else if (orginPos.x - currentPos.x < 0 && orginPos.y - currentPos.y > 0)
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, 0f));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else if (orginPos.x - currentPos.x > 0 && orginPos.y - currentPos.y < 0)
            {
                mainCam.transform.Translate(new Vector2(0f, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
        }
        else if(camPos.x >= upperCamBoundry.x && camPos.y <= lowerCamBoundry.y)
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if(orginPos.x - currentPos.x < 0 && orginPos.y - currentPos.y > 0)
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else if(orginPos.x - currentPos.x > 0 && orginPos.y - currentPos.y > 0)
            {
                mainCam.transform.Translate(new Vector2(0f, orginPos.y - currentPos.y));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else if(orginPos.x - currentPos.x < 0 && orginPos.y - currentPos.y < 0)
            {
                mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, 0f));
                orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
        }

        //default case
        else
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            //if camPos.x + (orgin.x - current.x) < -1 && camPos.x + (orgin.x - current.x) > 1
            //boundry - current = x 

            mainCam.transform.Translate(new Vector2(orginPos.x - currentPos.x, orginPos.y - currentPos.y));
            orginPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
    }
}

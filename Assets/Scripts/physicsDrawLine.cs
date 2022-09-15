using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class physicsDrawLine : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject curLine;
    public GameObject playButton;
    public GameObject hotSpot;

    [SerializeField] private Vector2 hotSpotOffset = new Vector2(0.5f, 0.5f);
    public LineRenderer lineRend;
    public EdgeCollider2D edgeCol;
    public List<Vector2> touchPos;

    [SerializeField] private float maxLineSize = 10f;
    private float curLineSize = 0f;
    private bool hotSpotCheck = false;
    
    void Start()
    {

    }

    void Update()
    {
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
            }
        }
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && hotSpotCheck == true)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if(curLineSize == 0)
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
            if(Vector2.Distance(tempTouchPos, touchPos[touchPos.Count - 1]) > .1f)
            {
                if(curLineSize < maxLineSize)
                {
                    updateLine(tempTouchPos);
                }
                else
                {
                    playButton.SetActive(true);
                }
            }
        }
        else if(Input.touchCount == 0 && curLineSize > 0)
        {
            lineRend.Simplify(0.15f);
        }
        else if(Input.touchCount == 0)
        {

        }
        else if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
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
}

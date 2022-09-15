using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class org_drawLine : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject curLine;

    public LineRenderer lineRend;
    public EdgeCollider2D edgeCol;
    public List<Vector2> touchPos;
    
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            createLine();
        }
        if(Input.GetMouseButton(0))
        {
            Vector2 tempTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(Vector2.Distance(tempTouchPos, touchPos[touchPos.Count - 1]) > .1f )
            {
                updateLine(tempTouchPos);
            }
        }
    }

    void createLine()
    {
        curLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRend = curLine.GetComponent<LineRenderer>();
        edgeCol = curLine.GetComponent<EdgeCollider2D>();
        touchPos.Clear();
        touchPos.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        touchPos.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
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
    }
}

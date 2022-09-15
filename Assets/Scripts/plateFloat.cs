using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plateFloat : MonoBehaviour
{
    [SerializeField] private bool run = true;
    [SerializeField] private float amp = 0.5f;
    [SerializeField] private float freq = 1f;
    private Vector3 tempPos;
    private Vector3 posOffset;

    void Start()
    {
        posOffset = transform.position;
    }

    void Update()
    {
        if(run)
        {
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * freq) * amp;

            transform.position = tempPos;
        }
    }
}

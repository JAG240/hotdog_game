using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playButtonBehavior : MonoBehaviour
{
    public GameObject hotdog;

    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        
    }

    public void startLevel()
    {
        hotdog.GetComponent<hotdogBehavior>().physicsStart();
        this.gameObject.SetActive(false);
    }

    public void enableButton()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = true;
    }
}

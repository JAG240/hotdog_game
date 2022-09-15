using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grillBehavoir : MonoBehaviour
{
    private Animator grillAnim;
    public GameObject Lid;
    private bool lidSpawned = false;

    void Start()
    {
        grillAnim = GetComponent<Animator>();
    }

    
    void Update()
    {
        if(grillAnim.GetBool("open") == true && !lidSpawned)
        {
            lidSpawned = true;
            StartCoroutine(spawnLid());
        }
    }

    public bool getOpen()
    {
        return grillAnim.GetBool("open");
    }

    public void setOpen(bool state)
    {
        grillAnim.SetBool("open", state);
    }

    private IEnumerator spawnLid()
    {
        yield return new WaitForSeconds(0.247f);
        Vector3 spawnLoc = GetComponent<Transform>().position + new Vector3(0f, 0.88f, 0f);
        GameObject tempLid = Instantiate(Lid, spawnLoc, Quaternion.identity);
        tempLid.GetComponent<Rigidbody2D>().AddForce(new Vector3(-150f, 200f, 180f));
    }
}

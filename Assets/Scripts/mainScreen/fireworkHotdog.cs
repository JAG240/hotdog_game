using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class fireworkHotdog : MonoBehaviour
{
    public GameObject hotdog;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float waitTime = 1.5f;
    [SerializeField] private float expandSize =10f;
    private float offset = 0.7f;
    private CapsuleCollider2D[] colliders;

    void Start()
    {
        StartCoroutine(explode());
        colliders = GetComponentsInChildren<CapsuleCollider2D>();
    }

    void Update()
    {
        GetComponent<Transform>().Translate(new Vector2(speed, 0f) * Time.deltaTime);
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(waitTime);
        //GetComponent<CapsuleCollider2D>().enabled = false;
        foreach(CapsuleCollider2D col in colliders)
        {
            col.enabled = false;
        }
        GetComponent<Transform>().localScale.Set(Mathf.Lerp(6f, expandSize, 0.1f), Mathf.Lerp(6f, expandSize, 0.1f), 0f);
        yield return new WaitForSeconds(0.5f);
        Vector2 tempPos = GetComponent<Transform>().position;
        for(int i = 0; i < 4; i++)
        {
            //GameObject tempDog = Instantiate(hotdog, new Vector3(tempPos.x, tempPos.y, 0f), quaternion.identity);
            if(i == 0)
            {
                //tempDog.GetComponentsInChildren<Rigidbody2D>().AddForce(new Vector2(400f, 0f));
                GameObject tempDog = Instantiate(hotdog, new Vector3(tempPos.x + offset, tempPos.y, 0f), quaternion.identity);
                Rigidbody2D[] bodies = tempDog.GetComponentsInChildren<Rigidbody2D>();
                foreach(Rigidbody2D body in bodies)
                {
                    body.AddForce(new Vector2(1000f, 0f));
                }
            }
            else if(i == 1)
            {
                //tempDog.GetComponent<Rigidbody2D>().AddForce(new Vector2(-400f, 0f));
                GameObject tempDog = Instantiate(hotdog, new Vector3(tempPos.x - offset, tempPos.y, 0f), quaternion.identity);
                Rigidbody2D[] bodies = tempDog.GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D body in bodies)
                {
                    body.AddForce(new Vector2(-1000f, 0f));
                }
            }
            else if(i == 2)
            {
                GameObject tempDog = Instantiate(hotdog, new Vector3(tempPos.x, tempPos.y + offset, 0f), quaternion.identity);
                tempDog.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, -90f));
                //tempDog.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 500f));
                Rigidbody2D[] bodies = tempDog.GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D body in bodies)
                {
                    body.AddForce(new Vector2(0f, 1500f));
                }
            }
            else if(i == 3)
            {
                GameObject tempDog = Instantiate(hotdog, new Vector3(tempPos.x, tempPos.y - offset, 0f), quaternion.identity);
                tempDog.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, -90f));
                //tempDog.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -400f));
                Rigidbody2D[] bodies = tempDog.GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D body in bodies)
                {
                    body.AddForce(new Vector2(0f, -1000f));
                }
            }
        }
        Destroy(this.gameObject);
    }
}

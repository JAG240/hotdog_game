using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class cloudManager : MonoBehaviour
{
    [SerializeField] private float maxY = 0f, minY = 0f;
    [SerializeField] private float spawnX = 0f;
    private bool cloudCheck = true;
    public GameObject cloud1, cloud2, cloud3, cloud1blur, cloud2blur, cloud3blur;
    private float size;
    private float speed;

    void Start()
    {
        
    }

    void Update()
    {
        if(cloudCheck == true)
        {
            StartCoroutine(makeCloud());
            cloudCheck = false;
        }
    }

    IEnumerator makeCloud()
    {
        int cloud = UnityEngine.Random.Range(1, 6);
        float yLoc = UnityEngine.Random.Range(minY, maxY);
        GameObject tempCloud;
        switch (cloud)
        {
            case 1:
                tempCloud = (GameObject)Instantiate(cloud1, new Vector3(spawnX, yLoc, 0f), quaternion.identity);
                break;
            case 2:
                tempCloud = (GameObject)Instantiate(cloud2, new Vector3(spawnX, yLoc, 0f), quaternion.identity);
                break;
            case 3:
                tempCloud = (GameObject)Instantiate(cloud3, new Vector3(spawnX, yLoc, 0f), quaternion.identity);
                break;
            case 4:
                tempCloud = (GameObject)Instantiate(cloud1blur, new Vector3(spawnX, yLoc, 0f), quaternion.identity);
                break;
            case 5:
                tempCloud = (GameObject)Instantiate(cloud2blur, new Vector3(spawnX, yLoc, 0f), quaternion.identity);
                break;
            case 6:
                tempCloud = (GameObject)Instantiate(cloud3blur, new Vector3(spawnX, yLoc, 0f), quaternion.identity);
                break;
            default:
                tempCloud = null;
                break;
        }

        if(cloud < 4)
        {
            size = UnityEngine.Random.Range(4f, 10f);
            speed = UnityEngine.Random.Range(1.5f, 0.4f);
        }
        else
        {
            size = UnityEngine.Random.Range(10f, 15f);
            speed = UnityEngine.Random.Range(1f, 0.3f);
        }

        cloudBehavior tempCloudScript = tempCloud.GetComponent<cloudBehavior>();
        tempCloudScript.setNewSize( new Vector3(size, size, 0));
        tempCloudScript.setDestructionLimit(spawnX - 40f);
        tempCloudScript.setSpeed(speed);
        DontDestroyOnLoad(tempCloud);
        yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
        cloudCheck = true;
    }
}

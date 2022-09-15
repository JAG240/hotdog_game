using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainScreenEvents : MonoBehaviour
{
    public GameObject fireworkHotdog;
    public GameObject hotdog;
    public GameObject mainScreenParent;
    public GameObject sceneManager;
    [SerializeField] private float spawnXMin;
    [SerializeField] private float spawnXMax;
    [SerializeField] private float spawnYMin;
    [SerializeField] private float spawnyMax;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    [SerializeField] private ParticleSystem transition;

    void Start()
    {
        StartCoroutine(hotdogCelebration());
        DontDestroyOnLoad(mainScreenParent);
        transition.Stop(true);
        DontDestroyOnLoad(transition);
    }

    void Update()
    {
        
    }

    IEnumerator hotdogCelebration()
    {
        while (true)
        {
            float spawnX = UnityEngine.Random.Range(spawnXMin, spawnXMax);
            float spawnY = UnityEngine.Random.Range(spawnYMin, spawnyMax);
            float cooldown = UnityEngine.Random.Range(minTime, maxTime);
            Instantiate(fireworkHotdog, new Vector3(spawnX, spawnY, 0f), Quaternion.Euler(0f, 0f, 90f));
            yield return new WaitForSeconds(cooldown);
        }
    }

    public void stopEvent()
    {
        StopAllCoroutines();
        transition.Play(true);
        StartCoroutine(changeScene("lvl1"));
    }

    /*private void spawnHotdogs()
    {
        for (int i = 0; i < 20; i++)
        {
            for (float x = 3; x < 24; x++)
            {
                GameObject hotdogGroup = Instantiate(hotdog, new Vector3(x, 13f, 0f), Quaternion.identity) ;
                hotdogGroup.transform.SetParent(mainScreenParent.transform);
            }
        }
    }*/

    IEnumerator changeScene(string name)
    {
        yield return new WaitForSeconds(1f);
        sceneManager.GetComponent<sceneMangerScript>().loadLevel(name);
        yield return new WaitForSeconds(1f);
        Component[] hotdogs = GetComponentsInChildren<CapsuleCollider2D>();
        foreach(CapsuleCollider2D dog in hotdogs)
        {
            Destroy(dog);
        }
    }
}

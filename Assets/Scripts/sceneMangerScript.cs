using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneMangerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void loadLevel(string name)
    {
        StartCoroutine(loadLevelTrans(name));
    }

    public IEnumerator loadLevelTrans(string name)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }
}

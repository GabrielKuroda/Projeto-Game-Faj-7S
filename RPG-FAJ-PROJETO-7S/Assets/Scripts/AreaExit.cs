using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    public string areaToLoad;

    public string areaTransitionName;

    public AreaEntrance theEntrance;

    private bool shouldLoad; 

    void Start()
    {
        theEntrance.transitionName = areaTransitionName;
    }

    void Update()
    {
        if (shouldLoad)
        {
            shouldLoad = false;
            SceneManager.LoadScene(areaToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            shouldLoad = true;
            RPGController.Instance.areaTransitionName = areaTransitionName;
        }
    }
}

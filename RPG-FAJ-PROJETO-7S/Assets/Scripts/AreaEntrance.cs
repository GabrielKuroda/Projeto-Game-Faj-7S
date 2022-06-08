using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{

    public string transitionName;

    void Start()
    {
        if(transitionName == RPGController.Instance.areaTransitionName)
        {
            RPGController.Instance.transform.position = transform.position;
        }
        UIFade.Instance.FadeFromBlack();
        RPGController.Instance.canMove = true;
    }

    void Update()
    {
        
    }
}

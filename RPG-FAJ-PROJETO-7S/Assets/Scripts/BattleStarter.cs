using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    public BattleType[] potentialBattles;
    private bool inArea;
    public bool activateOnStay;
    public float timeBetweenBattles;
    private float betweenBattleCounter;

    void Start()
    {
        betweenBattleCounter = Random.Range(timeBetweenBattles*.5f, timeBetweenBattles*1.5f);
    }

    void Update()
    {
        if(inArea){
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
                betweenBattleCounter -= Time.deltaTime;
            }

            if(betweenBattleCounter <= 0)
            {
                betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
                Debug.Log("Iniciou batalha");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            inArea = true;   
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inArea = false;
        }
    }
}
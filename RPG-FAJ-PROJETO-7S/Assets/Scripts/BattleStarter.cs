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
                
                StartCoroutine(StartBattleCo());
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

    public IEnumerator StartBattleCo()
    {
        UIFade.Instance.FadeToBlack();
        int selectedBattle = Random.Range(0, potentialBattles.Length);
        yield return new WaitForSeconds(1.5f);
        UIFade.Instance.FadeFromBlack();
        BattleManager.Instance.BattleStart();
        
    }
}

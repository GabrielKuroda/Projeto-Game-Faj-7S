using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : IPersistentSingleton<BattleManager>
{

    private bool battleActive;
    public bool turnWaiting;

    public GameObject battleScene;

    public int currentTurn;
    public int chanceToFlee = 35;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BattleStart()
    {
        if (!battleActive)
        {
            RPGController.Instance.canMove = false;
            battleActive = true;

            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);

            battleScene.SetActive(true);

            turnWaiting = true;
            currentTurn = 0;
        }
    }

    public void Flee()
    {
        int fleeSuccess = Random.Range(0, 100);
        if (fleeSuccess < chanceToFlee)
        {
            StartCoroutine(EndBattleCo());
        }
        else
        {
            Debug.Log("Não pode escapar da batalha");
        }
    }

    public IEnumerator EndBattleCo()
    {
        battleActive = false;
        yield return new WaitForSeconds(.5f);
        UIFade.Instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        battleScene.SetActive(false);
        UIFade.Instance.FadeFromBlack();
        currentTurn = 0;
        RPGController.Instance.canMove = true;
    }
}

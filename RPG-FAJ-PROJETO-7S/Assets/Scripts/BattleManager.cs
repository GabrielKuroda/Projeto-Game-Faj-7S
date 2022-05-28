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

    public Transform playerPositions;
    public Transform[] enemyPosition;

    public BattleChar playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BattleStart(string[] enemiesToSpaw)
    {
        if (!battleActive)
        {
            battleActive = true;
            RPGController.Instance.canMove = false;

            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);

            battleScene.SetActive(true);

            BattleChar newPlayer = Instantiate(playerPrefabs, playerPositions.position, playerPositions.rotation);

            newPlayer.transform.parent = playerPositions;
            activeBattlers.Add(newPlayer);

            for (int i = 0; i < enemiesToSpaw.Length; i++)
            {
                if (enemiesToSpaw[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpaw[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPosition[i].transform.position, enemyPosition[i].transform.rotation);
                            newEnemy.transform.parent = enemyPosition[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }

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

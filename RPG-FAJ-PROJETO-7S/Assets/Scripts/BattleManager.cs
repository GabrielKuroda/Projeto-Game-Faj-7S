using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public List<BattleChar> activeBattlers;

    public Text playerHpText;

    public int currentEnemy;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerTurn())
        {
            enemyTurn();
        }
    }

    public void BattleStart(string[] enemiesToSpaw)
    {
        if (!battleActive)
        {
            activeBattlers = new List<BattleChar>();
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
            UpdateUIStats();
            currentEnemy = 0;
            turnWaiting = true;
            currentTurn = 0;
        }
    }

    public void Flee()
    {
        if (isPlayerTurn()) {
            int fleeSuccess = Random.Range(0, 100);
            if (fleeSuccess < chanceToFlee)
            {
                StartCoroutine(EndBattleCo());
            }
            else
            {
                Debug.Log("Não pode escapar da batalha");
                currentTurn++;
            }
        }
        else
        {
            Debug.Log("Não é o seu turno");
        }
    }

    public void Attack()
    {
        if (isPlayerTurn())
        {
            DealDamage(1);
        }
        else
        {
            Debug.Log("Não é o seu turno");
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
    public void enemyTurn()
    {
        for(int i = 1; i < activeBattlers.Count; i++)
        {
            //StartCoroutine(EnemyAttack());
            DealDamage(0);
        }
        currentTurn = 0;
    }
    public IEnumerator EnemyAttack()
    {
        
        yield return new WaitForSeconds(1f);
        DealDamage(0);
    }

    public void DealDamage(int target)
    {
        float atkPwr = activeBattlers[currentTurn].strength;
        float defPwr = activeBattlers[target].defence;


        float damageCalc = (atkPwr / defPwr) * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);
        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);
        activeBattlers[target].currentHp -= damageToGive;
        currentTurn++;
        //Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);
        UpdateUIStats();
        ValidateIsDead();
    }

    public bool isPlayerTurn()
    {
        return currentTurn % 2 == 0;
    }
    public void UpdateUIStats()
    {
        playerHpText.text = activeBattlers[0].currentHp.ToString() + "/" + activeBattlers[0].maxHp.ToString();
    }

    public void ValidateIsDead()
    {
        if (activeBattlers[0].currentHp <= 0)
        {
            Debug.Log("Você morreu");
            StartCoroutine(EndBattleCo());
        }
        if(activeBattlers[1].currentHp <= 0)
        {
            Debug.Log("Você matou um inimigo");
            Destroy(enemyPosition[currentEnemy].GetChild(1).gameObject);
            activeBattlers.RemoveAt(1);
            currentEnemy++;
        }
        if (activeBattlers.Count == 1)
        {
            Debug.Log("Você venceu a batalha");
            StartCoroutine(EndBattleCo());
        }
    }

}

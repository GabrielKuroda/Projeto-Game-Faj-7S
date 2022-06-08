using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : IPersistentSingleton<BattleManager>
{

    public bool battleActive;
    private bool ableToAct;
    public bool turnWaiting;
    public bool isItemMenuOpen;

    public GameObject battleScene;
    public GameObject uiButtonsHolder;
    public GameObject itemMenu;

    public int currentTurn;
    public int chanceToFlee = 35;
    public int currentEnemy;
    
    public Transform playerPositions;
    public Transform[] enemyPosition;

    public BattleChar playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public DamageNumber damageNumber;

    public ItemButton[] itemButtons;

    public List<BattleChar> activeBattlers;

    public Text playerNameText;
    public Text playerHpText;
    public Text playerMpText;

    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    if (ableToAct)
                    {
                        uiButtonsHolder.SetActive(true);
                    }
                    else
                    {
                        uiButtonsHolder.SetActive(false);
                    }
                }
                else
                {
                    uiButtonsHolder.SetActive(false);
                    StartCoroutine(EnemyAttack());
                }
            }
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
            playerAnimator = newPlayer.GetComponent<Animator>();
            activeBattlers.Add(newPlayer);
            playerNameText.text = activeBattlers[0].charName;

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
            ableToAct = true;
            currentEnemy = 0;
            turnWaiting = true;
            currentTurn = 0;
        }
    }

    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
            ableToAct = true;
        }
        Debug.Log(currentTurn);
        turnWaiting = true;
        UpdateUIStats();
    }

    public void Flee()
    {
        if(!isItemMenuOpen) {
            int fleeSuccess = Random.Range(0, 100);
            if (fleeSuccess < chanceToFlee)
            {
                StartCoroutine(EndBattleCo());
            }
            else
            {
                Debug.Log("N�o pode escapar da batalha");
                currentTurn++;
            }
        }
    }

    public void Attack()
    {
        if(!isItemMenuOpen) {
            ableToAct = false;
            StartCoroutine(AttackAnimation());
        }
    }

    public void Ultimate()
    {
        if(!isItemMenuOpen) {
            if(activeBattlers[0].currentMp >= 25)
            {
                ableToAct = false;
                activeBattlers[0].currentMp -= 25;
                StartCoroutine(UltimateAnimation());
            }
            else {
                Debug.Log("Voc� n�o tem MP para utilizar a ultimate");
            }
        }
    }

    public IEnumerator AttackAnimation()
    {
        playerAnimator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(1f);
        DealDamage(1);
        playerAnimator.SetBool("isAttacking", false);
        NextTurn();
    }

    public IEnumerator UltimateAnimation()
    {
        playerAnimator.SetBool("isUltimate", true);
        yield return new WaitForSeconds(1f);
        DealUltimateDamage(1);
        playerAnimator.SetBool("isUltimate", false);
        NextTurn();
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
        Destroy(activeBattlers[0].gameObject);
    }

    public IEnumerator EnemyAttack()
    {
        turnWaiting = false;
        activeBattlers[currentTurn].GetComponent<Animator>().SetBool("isAttacking", true);
        yield return new WaitForSeconds(1f);
        DealDamage(0);
        activeBattlers[currentTurn].GetComponent<Animator>().SetBool("isAttacking", false);
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public void DealDamage(int target)
    {
        float atkPwr = activeBattlers[currentTurn].strength;
        float defPwr = activeBattlers[target].defence;


        float damageCalc = (atkPwr / defPwr) * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);
        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);
        activeBattlers[target].currentHp -= damageToGive;
        Instantiate(damageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);
        UpdateUIStats();
        ValidateIsDead();
    }

    public void DealUltimateDamage(int target)
    {
        float atkPwr = activeBattlers[currentTurn].strength;
        float defPwr = activeBattlers[target].defence;


        float damageCalc = (atkPwr / defPwr) * Random.Range(.9f, 1.1f) * 2;
        int damageToGive = Mathf.RoundToInt(damageCalc);
        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);
        activeBattlers[target].currentHp -= damageToGive;
        Instantiate(damageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);
        UpdateUIStats();
        ValidateIsDead();
    }

    public bool isPlayerTurn()
    {
        return currentTurn % 2 == 0 || currentTurn == 0;
    }
    public void UpdateUIStats()
    {
        playerHpText.text = activeBattlers[0].currentHp.ToString() + "/" + activeBattlers[0].maxHp.ToString();
        playerMpText.text = activeBattlers[0].currentMp.ToString() + "/" + activeBattlers[0].maxMp.ToString();
    }

    public void ValidateIsDead()
    {
        if (activeBattlers[0].currentHp <= 0)
        {
            activeBattlers[0].EnemyFade();
            Debug.Log("Voc� morreu");
            StartCoroutine(EndBattleCo());
        }
        if(activeBattlers[1].currentHp <= 0)
        {
            Debug.Log("Voc� matou um inimigo");
            activeBattlers[1].EnemyFade();
            activeBattlers.RemoveAt(1);
            currentEnemy++;
        }
        if (activeBattlers.Count == 1)
        {
            Debug.Log("Voc� venceu a batalha");
            StartCoroutine(EndBattleCo());
        }
    }

    public void showItens() 
    {
        for(int i = 0; i < itemButtons.Length; i++){
            itemButtons[i].buttonValue = i;
            if(GameManager.Instance.itensHeld[i] != ""){
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.Instance.GetItemDetails(GameManager.Instance.itensHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.Instance.numbOfItens[i].ToString();
            }else{
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void openItemMenu() 
    {
        if(itemMenu.activeInHierarchy){
                itemMenu.SetActive(false);
                isItemMenuOpen = false;
            }else{
                itemMenu.SetActive(true);
                isItemMenuOpen = true;
                showItens();
            }
    }

    public void closeItemMenu() 
    {
        itemMenu.SetActive(false);
        isItemMenuOpen = false;
    }

    public void useItem(int itemValue) 
    {
        int value = GameManager.Instance.GetItemDetails(GameManager.Instance.itensHeld[itemValue]).amountToChange;

        activeBattlers[0].currentHp += value;
        activeBattlers[0].currentMp += value;

        if(activeBattlers[0].currentHp > activeBattlers[0].maxHp){
            activeBattlers[0].currentHp = activeBattlers[0].maxHp;
        }

        if(activeBattlers[0].currentMp > activeBattlers[0].maxMp){
            activeBattlers[0].currentMp = activeBattlers[0].maxMp;
        }

        GameManager.Instance.RemoveItem(GameManager.Instance.GetItemDetails(GameManager.Instance.itensHeld[itemValue]).itemName);
    }
}
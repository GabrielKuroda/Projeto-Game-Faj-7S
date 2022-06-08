using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssencialsLoader : MonoBehaviour
{
    public GameObject UICanvas;
    public GameObject player;
    public GameObject gameManager;
    public GameObject battleManager;

    // Start is called before the first frame update
    void Start()
    {
        if (UIFade.Instance == null)
        {
            UIFade.Instance = Instantiate(UICanvas).GetComponent<UIFade>();
        }

        if (RPGController.Instance == null)
        {
            RPGController.Instance = Instantiate(player).GetComponent<RPGController>();
        }

        if (GameManager.Instance == null)
        {
            GameManager.Instance = Instantiate(gameManager).GetComponent<GameManager>();
        }

        if (BattleManager.Instance == null)
        {
            BattleManager.Instance = Instantiate(battleManager).GetComponent<BattleManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

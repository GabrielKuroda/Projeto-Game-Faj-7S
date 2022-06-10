using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{

    public bool isPlayer;
    public bool hasDied;
    private bool shouldFade;

    public string charName;
    public string[] movesAvailable;

    public int currentHp, maxHp, currentMp, maxMp, defence, strength;
    public float fadSpeed = 1f;

    public SpriteRenderer theSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFade)
        {

            theSprite.color = new Color(Mathf.MoveTowards(theSprite.color.r, 1f, fadSpeed * Time.deltaTime), 
                Mathf.MoveTowards(theSprite.color.g, 0f, fadSpeed * Time.deltaTime), 
                Mathf.MoveTowards(theSprite.color.b, 0f, fadSpeed * Time.deltaTime),
                Mathf.MoveTowards(theSprite.color.a, 0f, fadSpeed * Time.deltaTime));
            if (theSprite.color.a == 0)
            {

                gameObject.SetActive(false);
            }
        }
    }
    public void EnemyFade()
    {
        shouldFade = true;
    }
}

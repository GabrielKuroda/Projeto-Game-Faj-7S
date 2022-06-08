using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;
    public ItemButton[] itemButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2") && !BattleManager.Instance.battleActive){
            if(theMenu.activeInHierarchy){
                theMenu.SetActive(false);
                RPGController.Instance.canMove = true;
            }else{
                theMenu.SetActive(true);
                showItens();
                RPGController.Instance.canMove = false;
            }
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
}

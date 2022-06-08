using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private bool canPickup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canPickup && Input.GetButtonDown("Fire1") && RPGController.Instance.canMove){
            //Add o item no inventario
            GameManager.Instance.AddItem(GetComponent<Item>().itemName);
            //Apaga o Item da Scene
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        //Verifica se oq entrou foi o Player
        if(other.tag == "Player"){
            canPickup= true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        //Verifica se oq saiu foi o Player
        if(other.tag == "Player"){
            canPickup= false;
        }
    }
}

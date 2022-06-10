using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IPersistentSingleton<GameManager>
{

    public Action<string> OnLoadedSceneComplete;
    private string _currentScene;
    public string[] itensHeld;
    public int[] numbOfItens;
    public Item[] refereceItems;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoadScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if(asyncOperation == null)
        {
            Debug.LogError("Erro ao carregar a scene " + sceneName);
            return;
        }
        _currentScene = sceneName;
        asyncOperation.completed += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(AsyncOperation asyncOperation)
    {
        OnLoadedSceneComplete?.Invoke(_currentScene);
    }

    public Item GetItemDetails(string itemToGrab){
        for(int i = 0; i < refereceItems.Length;i++){
            if(refereceItems[i].itemName == itemToGrab){
                return refereceItems[i];
            }
        }
        return null;
    }

    public void AddItem(string itemToAdd){
        int newItemPosition = 0;
        bool foundSpace = false;

        for(int i = 0; i < itensHeld.Length; i++){
            if(itensHeld[i] == "" || itensHeld[i] == itemToAdd){
                newItemPosition = i;
                foundSpace = true;
                break;
            }
        }

        if(foundSpace){
            bool itemExists = false;
            for(int i =0; i < refereceItems.Length; i++){
                if(refereceItems[i].itemName == itemToAdd){
                    itemExists = true;
                    break;
                }
            }

            if(itemExists){
                itensHeld[newItemPosition] = itemToAdd;
                numbOfItens[newItemPosition]++;
            }else{
                Debug.LogError(itemToAdd + " Does not Exist!!");
            }
        }
    }

    public void RemoveItem(string itemToRemove){
        bool foundItem = false;
        int itemPosition = 0;

        for(int i = 0; i < itensHeld.Length; i++){
            if(itensHeld[i] == itemToRemove){
                foundItem = true;
                itemPosition = i;
                break;
            }
        }

        if(foundItem){
            numbOfItens[itemPosition]--;
            if(numbOfItens[itemPosition] <= 0){
                itensHeld[itemPosition] = "";
            }
        }else{
            Debug.LogError("Couldn't Find "+ itemToRemove);
        }
    }
}

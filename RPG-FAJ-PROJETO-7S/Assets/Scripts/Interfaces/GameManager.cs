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

}

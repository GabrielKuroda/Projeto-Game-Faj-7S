using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControllerFade : MonoBehaviour
{
    public static ControllerFade instanciaFade;
    public Image imageFade;
    public Color corInicial;
    public Color corFinal;
    public float tempoFade;
    public bool isFade;
    private float tempo;
    private void Awake()
    {
        instanciaFade = this;
    }
    IEnumerator StartFade()
    {
        isFade = true;
        tempo = 0f;
        while (tempo <= tempoFade)
        {
            imageFade.color = Color.Lerp(corInicial, corFinal, tempo / tempoFade);
            tempo += Time.deltaTime;
            yield return null;
        }
        isFade = false;
        if (!isFade)
        {
            Destroy(imageFade);
        }
    }

    void Start()
    {
        StartCoroutine(StartFade());
    }

    void Update()
    {
    }

}

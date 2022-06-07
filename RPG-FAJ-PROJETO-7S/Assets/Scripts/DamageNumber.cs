using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{

    public Text damageText;
    public float lifetime = 1f;
    public float moveSpeed = 1f;
    public float placementJitterX = .5f;
    public float placementJitterY = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject,lifetime);
        transform.position += new Vector3(0f,moveSpeed * Time.deltaTime,0f);
    }

    public void SetDamage(int damageAmount){
        damageText.text = damageAmount.ToString();
        Debug.Log("Dano do texto: " + damageAmount);
        transform.position += new Vector3(Random.Range(-placementJitterX, placementJitterX), Random.Range(1, placementJitterY), 0f);
    }
}

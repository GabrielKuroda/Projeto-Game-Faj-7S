using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINpc : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform npc, pointA, pointB;
    public SpriteRenderer npcSpriteRenderer;
    private Vector3 destinyPoint;
    public bool isIdle = true;
    public float speed;

    public Animator _animator;


    void Start()
    {
        npc.position = pointA.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle)
        {
            StartCoroutine("Walk");
        }
        SetAnimationNpc();
    }

    IEnumerator Walk()
    {
        if (npc.position == pointA.position)
        {
            yield return new WaitForSeconds(2f);
            destinyPoint = pointB.position;
            yield return new WaitForSeconds(2f);
            npcSpriteRenderer.flipX = true;
        }
        if (npc.position == pointB.position)
        {
            yield return new WaitForSeconds(1f);
            destinyPoint = pointA.position;
            yield return new WaitForSeconds(1f);
            npcSpriteRenderer.flipX = false;
        }

        npc.position = Vector3.MoveTowards(npc.position, destinyPoint, speed);
    }

    public void SetAnimationNpc()
    {
        _animator.SetBool("Stopped", !isIdle);
    }

}
